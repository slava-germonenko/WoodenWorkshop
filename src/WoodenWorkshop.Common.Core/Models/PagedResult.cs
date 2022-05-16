namespace WoodenWorkshop.Common.Core.Models;

public record PagedResult<TItem> : Paging
{
    public ICollection<TItem> Data { get; set; } = new List<TItem>();
    
    public int Total { get; set; }
}