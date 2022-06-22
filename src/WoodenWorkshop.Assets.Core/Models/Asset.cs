using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Assets.Core.Models;

public class Asset : SoftDeletableModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public Uri? BlobUri { get; set; }
    
    public int? FolderId { get; set; }
}