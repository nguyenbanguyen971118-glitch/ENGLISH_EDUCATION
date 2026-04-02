namespace backend.DTOs;

public class ApiResponseDto<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = "SUCCESS";
    public string? MessageKey { get; set; }
    public string? ErrorCode { get; set; }
    public T? Data { get; set; }

    public static ApiResponseDto<T> Ok(
        T data,
        string message = "Thành công",
        string code = "SUCCESS",
        string messageKey = "COMMON.SUCCESS")
    {
        return new ApiResponseDto<T>
        {
            Success = true,
            Message = message,
            Code = code,
            MessageKey = messageKey,
            Data = data
        };
    }

    public static ApiResponseDto<T> Fail(
        string message,
        string? errorCode = null,
        string? messageKey = null)
    {
        var resolvedCode = string.IsNullOrWhiteSpace(errorCode) ? "ERROR_UNKNOWN" : errorCode;
        return new ApiResponseDto<T>
        {
            Success = false,
            Message = message,
            Code = resolvedCode,
            MessageKey = messageKey ?? resolvedCode,
            ErrorCode = resolvedCode
        };
    }
}
