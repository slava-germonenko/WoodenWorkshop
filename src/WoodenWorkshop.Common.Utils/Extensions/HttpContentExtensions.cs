using System.Net.Http.Json;
using System.Runtime.Serialization;

namespace WoodenWorkshop.Common.Utils.Extensions;

public static class HttpContentExtensions
{
    public static async Task<TData> ReadAsJsonAsync<TData>(this HttpContent httpContent)
    {
        var serializedContent = await httpContent.ReadFromJsonAsync<TData>();
        if (serializedContent is not null)
        {
            return serializedContent;
        }

        var typeToSerializeTo = typeof(TData);
        var typeName = typeToSerializeTo.FullName ?? typeToSerializeTo.Name;
        throw new SerializationException(
            $"Failed to serialize response: {await httpContent.ReadAsStringAsync()} as ${typeName}"
        );
    }
}