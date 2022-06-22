using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.Assets.Core.Dtos;

public class AddAssetDto
{
    [Required]
    public string Name { get; }

    public Stream Blob { get; }

    public int? FolderId { get; }

    public AddAssetDto(string name, Stream blob, int? folderId = null)
    {
        Name = name;
        Blob = blob;
        FolderId = folderId;
    }
}