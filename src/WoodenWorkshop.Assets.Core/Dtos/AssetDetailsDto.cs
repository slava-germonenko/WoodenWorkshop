namespace WoodenWorkshop.Assets.Core.Dtos;

public class AssetDetailsDto
{
    public int AssetId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public int? FolderId { get; set; } = null;
}