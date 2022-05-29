namespace WoodenWorkshop.Common.Utils.Http.Query;

[AttributeUsage(AttributeTargets.Property)]
public class QueryParamAttribute : Attribute
{
    public string Name { get; }

    public QueryParamAttribute(string name)
    {
        Name = name;
    }
}