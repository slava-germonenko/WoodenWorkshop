namespace WoodenWorkshop.Common.Utils.Http.Query;

/// <summary>
/// Attribute to mark properties that must be ignored by the <see cref="QueryBuilder"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class QueryIgnoreAttribute : Attribute
{
}