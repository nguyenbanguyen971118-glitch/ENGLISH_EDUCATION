using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backend.Swagger;

public sealed class ScheduleSwaggerExamplesOperationFilter : IOperationFilter
{
    public ScheduleSwaggerExamplesOperationFilter()
    {
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
        {
            return;
        }

        if (!string.Equals(actionDescriptor.ControllerName, "Schedule", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var examples = BuildRealExamples();

        if (string.Equals(actionDescriptor.ActionName, "Create", StringComparison.OrdinalIgnoreCase))
        {
            SetJsonExample(operation, examples.CreateRequest);
            SetResponseExample(operation, "200", examples.CreateResponse);
            SetResponseExample(operation, "400", BuildErrorResponseExample());
        }
        else if (string.Equals(actionDescriptor.ActionName, "Update", StringComparison.OrdinalIgnoreCase))
        {
            SetJsonExample(operation, examples.UpdateRequest);
            SetResponseExample(operation, "200", examples.UpdateResponse);
            SetResponseExample(operation, "400", BuildErrorResponseExample());
        }
        else if (string.Equals(actionDescriptor.ActionName, "GetSamplePayload", StringComparison.OrdinalIgnoreCase))
        {
            SetResponseExample(operation, "200", BuildSamplePayloadResponseExample(examples));
        }
    }

    private static void SetJsonExample(OpenApiOperation operation, IOpenApiAny example)
    {
        if (operation.RequestBody == null)
        {
            return;
        }

        if (!operation.RequestBody.Content.TryGetValue("application/json", out var mediaType))
        {
            return;
        }

        mediaType.Example = example;
    }

    private static void SetResponseExample(OpenApiOperation operation, string statusCode, IOpenApiAny example)
    {
        if (!operation.Responses.TryGetValue(statusCode, out var response))
        {
            return;
        }

        if (!response.Content.TryGetValue("application/json", out var mediaType))
        {
            return;
        }

        mediaType.Example = example;
    }

    private static RealScheduleExamples BuildRealExamples()
    {
        // Sử dụng dữ liệu mẫu thay vì truy vấn database (tránh lỗi DI ở startup)
        var fallbackClassId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var fallbackRoomId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var createDate = new DateTime(2026, 5, 8);

        var createRequest = new OpenApiObject
        {
            ["MaLopHoc"] = new OpenApiString(fallbackClassId.ToString()),
            ["MaPhongHoc"] = new OpenApiString(fallbackRoomId.ToString()),
            ["NgayHoc"] = new OpenApiString(createDate.ToString("yyyy-MM-ddTHH:mm:ss")),
            ["MaTietBatDau"] = new OpenApiInteger(1),
            ["MaTietKetThuc"] = new OpenApiInteger(2),
            ["TieuDe"] = new OpenApiString("Lớp 1 Sáng - Buổi học"),
            ["NoiDung"] = new OpenApiString("Nội dung buổi học theo lớp thực tế trong hệ thống")
        };

        var updateRequest = new OpenApiObject
        {
            ["MaLopHoc"] = new OpenApiString(fallbackClassId.ToString()),
            ["MaPhongHoc"] = new OpenApiString(fallbackRoomId.ToString()),
            ["NgayHoc"] = new OpenApiString(createDate.ToString("yyyy-MM-ddTHH:mm:ss")),
            ["MaTietBatDau"] = new OpenApiInteger(3),
            ["MaTietKetThuc"] = new OpenApiInteger(4),
            ["TieuDe"] = new OpenApiString("Buổi học cập nhật"),
            ["NoiDung"] = new OpenApiString("Nội dung cập nhật theo buổi học thực tế")
        };

        var createResponse = new OpenApiObject
        {
            ["id"] = new OpenApiString(Guid.NewGuid().ToString())
        };

        var updateResponse = new OpenApiObject
        {
            ["message"] = new OpenApiString("Cập nhật buổi học thành công."),
            ["id"] = new OpenApiString(Guid.NewGuid().ToString())
        };

        return new RealScheduleExamples(createRequest, updateRequest, createResponse, updateResponse);
    }

    private static IOpenApiAny BuildErrorResponseExample()
    {
        return new OpenApiObject
        {
            ["errors"] = new OpenApiArray
            {
                new OpenApiString("Trùng Phòng học: phòng này đã được sử dụng ở khung giờ đó."),
                new OpenApiString("Trùng Giảng viên: một hoặc nhiều giảng viên đang bận trong khung giờ đó.")
            }
        };
    }

    private static IOpenApiAny BuildSamplePayloadResponseExample(RealScheduleExamples examples)
    {
        return new OpenApiObject
        {
            ["create"] = examples.CreateRequest,
            ["update"] = examples.UpdateRequest,
            ["notes"] = new OpenApiArray
            {
                new OpenApiString("Dữ liệu mẫu trong Swagger được lấy từ các bản ghi thực tế trong hệ thống nếu có."),
                new OpenApiString("MaPhongHoc có thể là null nếu buổi học chưa chốt phòng."),
                new OpenApiString("MaTietBatDau phải nhỏ hơn hoặc bằng MaTietKetThuc.")
            }
        };
    }

    private sealed record RealScheduleExamples(
        IOpenApiAny CreateRequest,
        IOpenApiAny UpdateRequest,
        IOpenApiAny CreateResponse,
        IOpenApiAny UpdateResponse);
}
