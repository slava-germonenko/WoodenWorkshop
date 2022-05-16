using WoodenWorkshop.Common.Core.Exceptions;

namespace WoodenWorkshop.Common.Core.Models;

public record BaseError
{
    public int ErrorCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public static BaseError FromException(CoreLogicException exception) => new()
    {
        ErrorCode = exception.ErrorCode,
        Message = exception.Message,
    };
}