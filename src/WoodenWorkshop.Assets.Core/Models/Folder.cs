using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Assets.Core.Models;

public class Folder : SoftDeletableModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; } = null;
}