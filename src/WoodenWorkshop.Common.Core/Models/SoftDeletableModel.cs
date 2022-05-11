namespace WoodenWorkshop.Common.Core.Models;

public class SoftDeletableModel : BaseModel
{
    public DateTime? DeletedDate { get; set; }

    public bool Deleted => DeletedDate != null;
}