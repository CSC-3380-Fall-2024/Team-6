using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace Team6.Web.Extensions
{
    public static class TempDataExtensions
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key)
        {
            tempData.TryGetValue(key, out object? obj);
            return obj == null ? default : JsonSerializer.Deserialize<T>((string)obj);
        }
    }
}