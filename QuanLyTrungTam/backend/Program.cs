using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Helpers;
using backend.Middlewares;
using backend.Services;
using backend.Services.Interfaces;
using backend.Repositories.Interfaces;
using backend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Hubs;
using backend.Swagger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Nạp cấu hình theo thứ tự: appsettings.json -> appsettings.{Environment}.json -> biến môi trường.
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// 1. Cấu hình CORS: cho phép FE dev gọi vào backend trong quá trình phát triển.
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 2. Lấy chuỗi kết nối từ appsettings.{Environment}.json hoặc biến môi trường.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Thiếu ConnectionStrings:DefaultConnection. Hãy cấu hình trong appsettings.Development.json hoặc biến môi trường."
    );
}

// 3. Thiết lập kết nối MySQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.45-mysql"))
);

// 4. Đăng ký các tầng Repository / Service / Helper.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IChatRealtimeNotifier, ChatRealtimeNotifier>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddSingleton<IRefreshSessionStore, InMemoryRefreshSessionStore>();
builder.Services.AddSingleton<IFirebasePushService, FirebasePushService>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<PermissionBootstrapService>();
builder.Services.AddScoped<UserBootstrapHelper>();
builder.Services.AddScoped<PermissionHelper>(); // Helper kiểm tra quyền chi tiết.
builder.Services.AddScoped<IScheduleChangeRequestService, ScheduleChangeRequestService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSection["Secret"] ?? "replace-this-in-production-please-at-least-32-chars";
var jwtIssuer = jwtSection["Issuer"] ?? "QuanLyTrungTamBackend";
var jwtAudience = jwtSection["Audience"] ?? "QuanLyTrungTamFrontend";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/hubs/chat"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// 5. Cấu hình Swagger để kiểm tra API nhanh trong môi trường dev.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Quản Lý Trung Tâm API",
        Version = "v1",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support Team"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.OperationFilter<ScheduleSwaggerExamplesOperationFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var bootstrapService = scope.ServiceProvider.GetRequiredService<PermissionBootstrapService>();
    await bootstrapService.EnsureInitializedAsync();

    var userBootstrapHelper = scope.ServiceProvider.GetRequiredService<UserBootstrapHelper>();
    var isUserBootstrapInitialized = await userBootstrapHelper.IsInitializedAsync();
    if (!isUserBootstrapInitialized)
    {
        await userBootstrapHelper.EnsureInitializedAsync();
    }
}
// --- THỨ TỰ MIDDLEWARE RẤT QUAN TRỌNG ---

// 6. Kích hoạt Swagger UI.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quản Lý Trung Tâm API v1");
    options.RoutePrefix = "swagger"; // Truy cập tại: http://localhost:PORT/swagger
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoginRateLimitMiddleware>();
app.UseMiddleware<LoginAuditMiddleware>();

// Thứ tự này đảm bảo: CORS trước -> có endpoint -> xác thực JWT -> kiểm tra quyền chi tiết -> áp dụng authorization chuẩn.
// 7. Kích hoạt CORS trước UseRouting để xử lý pre-flight request (OPTIONS) ngay lập tức.
app.UseCors("AllowReactApp");
app.UseRouting(); 
app.UseAuthentication();
app.UseMiddleware<AuthorizationMiddleware>();  
app.UseAuthorization();

// 8. Map controller endpoints.
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();