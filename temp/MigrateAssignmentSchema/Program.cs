using backend.Data;
using Microsoft.EntityFrameworkCore;

const string connectionString = "server=127.0.0.1;port=3307;database=quanlytrungtam;user=root;password=root";

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseMySql(connectionString, ServerVersion.Parse("8.0.45-mysql"))
    .Options;

await using var context = new AppDbContext(options);
var applied = await context.Database.GetAppliedMigrationsAsync();
var pending = await context.Database.GetPendingMigrationsAsync();

Console.WriteLine("Applied migrations:");
foreach (var migration in applied)
{
    Console.WriteLine($"- {migration}");
}

Console.WriteLine("Pending migrations:");
foreach (var migration in pending)
{
    Console.WriteLine($"- {migration}");
}

await context.Database.MigrateAsync();
Console.WriteLine("Database migration completed.");
