namespace WoodenWorkshop.Common.Core.Models;

public class BaseModel
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}