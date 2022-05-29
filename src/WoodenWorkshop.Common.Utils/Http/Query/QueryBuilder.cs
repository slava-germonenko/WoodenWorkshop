using System.Reflection;
using System.Web;

namespace WoodenWorkshop.Common.Utils.Http.Query;

public class QueryBuilder
{
    private readonly QuerySerializationOptions _querySerializationOptions;

    private readonly Type _queryIgnoreType = typeof(QueryIgnoreAttribute);

    private readonly Type _queryAttributeType = typeof(QueryParamAttribute);

    private readonly Func<string, string> _caseConverter;

    public QueryBuilder() : this(QuerySerializationOptions.Default())
    {
    }
    
    public QueryBuilder(QuerySerializationOptions querySerializationOptions)
    {
        _querySerializationOptions = querySerializationOptions;
        _caseConverter = GetCaseConverterFunction(querySerializationOptions.Case);
    }

    public string BuildQuery(object queryObj)
    {
        var propertiesToSerialize = queryObj.GetType()
            .GetProperties()
            .Where(
                prop => prop.CanRead && prop.CustomAttributes.All(attr => attr.AttributeType != _queryIgnoreType)
            )
            .ToArray();

        var queryParams = propertiesToSerialize.ToDictionary(
                property => GetQueryParamName(property),
                property => GetQueryParamValue(property, queryObj)
            )
            .Where(param => param.Value is not null)
            .Select(param => $"{HttpUtility.UrlEncode(param.Key)}={HttpUtility.UrlEncode(param.Value)}");

        return "?" + string.Join('&', queryParams);
    }

    private string GetQueryParamName(PropertyInfo prop)
    {
        var queryParamAttr = prop.GetCustomAttribute<QueryParamAttribute>();
        if (queryParamAttr is not null)
        {
            return queryParamAttr.Name;
        }

        return _caseConverter(prop.Name);
    }

    private string? GetQueryParamValue(PropertyInfo prop, object target)
    {
        return prop.PropertyType == typeof(IQuerySerializable) 
            ? (prop.GetValue(target) as IQuerySerializable)?.ToQueryParam() 
            : prop.GetValue(target)?.ToString();
    }

    private static Func<string, string> GetCaseConverterFunction(CaseTransforms stringCase)
    {
        return stringCase switch
        {
            CaseTransforms.CamelCase => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName,
            _ => str => str,
        };
    }
}