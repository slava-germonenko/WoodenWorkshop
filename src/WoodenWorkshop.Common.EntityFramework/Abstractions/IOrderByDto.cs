using System.Linq.Expressions;

namespace WoodenWorkshop.Common.EntityFramework.Abstractions;

public interface IOrderByDto<T>
{
    public bool Desc { get; }
    
    public Expression<Func<T, object>> KeySelector { get; }
}