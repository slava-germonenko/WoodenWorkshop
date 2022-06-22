using System.Linq.Expressions;

using WoodenWorkshop.Assets.Core.Models;
using WoodenWorkshop.Common.EntityFramework.Abstractions;

namespace WoodenWorkshop.Assets.Core.Dtos;

public class FoldersOrder : IOrderByDto<Folder>
{
    public bool Desc { get; }

    public Expression<Func<Folder, object>> KeySelector { get; }
    
    private FoldersOrder(bool desc, Expression<Func<Folder, object>> keySelector)
    {
        Desc = desc;
        KeySelector = keySelector;
    }

    public static FoldersOrder Default() => Create(string.Empty, string.Empty);

    public static FoldersOrder Create(string keyFieldName, string direction)
    {
        var desc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
        Expression<Func<Folder, object>> keySelector;
        switch (keyFieldName.ToLower())
        {
            case "name":
                keySelector = folder => folder.Name;
                break;
            default:
                keySelector = folder => folder.CreatedDate;
                desc = true;
                break;
        }

        return new FoldersOrder(desc, keySelector);
    }
}