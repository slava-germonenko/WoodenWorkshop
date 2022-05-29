namespace WoodenWorkshop.Common.Utils.Http.Query;

public record QuerySerializationOptions
{
    public CaseTransforms Case { get; set; } = CaseTransforms.CamelCase;

    public static QuerySerializationOptions Default() => new();
};