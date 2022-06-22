using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Assets.Core.Dtos;

public record FoldersFilter : Paging
{
    public string? Search { get; set; }

    public int? ParentId { get; set; }
}