namespace WoodenWorkshop.Common.Core.Exceptions;

public class CoreLogicException : Exception
{
    public int ErrorCode { get; }

    public CoreLogicException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public CoreLogicException(string message, int errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}