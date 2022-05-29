namespace WoodenWorkshop.Common.Core.Models;

public record DevError : BaseError
{
    public string? StackTrace { get; set; }
}