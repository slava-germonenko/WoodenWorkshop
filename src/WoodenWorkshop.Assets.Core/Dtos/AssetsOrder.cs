using System.Linq.Expressions;

using WoodenWorkshop.Assets.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Abstractions;

namespace WoodenWorkshop.Assets.Core.Dtos;

public record AssetsOrder : IOrderByDto<Asset>
{
    public bool Desc { get; }

    public Expression<Func<Asset, object>> KeySelector { get; }
    
    private AssetsOrder(bool desc, Expression<Func<Asset, object>> keySelector)
    {
        Desc = desc;
        KeySelector = keySelector;
    }

    public static AssetsOrder Default() => Create(string.Empty, string.Empty);
    
    public static AssetsOrder Create(string keyFieldName, string direction)
    {
        var desc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
        Expression<Func<Asset, object>> keySelector;
        switch (keyFieldName.ToLower())
        {
            case "name":
                keySelector = asset => asset.Name;
                break;
            default:
                keySelector = asset => asset.CreatedDate;
                desc = true;
                break;
        }

        return new AssetsOrder(desc, keySelector);
    }
}