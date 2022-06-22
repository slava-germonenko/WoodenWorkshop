using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Assets.Core.Dtos;

public record AssetsFilter : Paging
{
    public string? Search { get; set; }
    
    public int? FolderId { get; set; }
}