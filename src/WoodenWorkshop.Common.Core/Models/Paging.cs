namespace WoodenWorkshop.Common.Core.Models;

public record Paging
{
    public const int DefaultOffset = 0;

    public const int DefaultCount = 50;

    public int Offset { get; set; } = DefaultOffset;

    public int Count { get; set; } = DefaultCount;
}