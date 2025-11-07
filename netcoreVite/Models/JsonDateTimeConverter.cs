using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotVue.Models;

public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _formats = { 
        "yyyy-MM-ddTHH:mm:ss.ffffff", // 默认格式
        "yyyy-MM-ddTHH:mm:ss.fffffff", // 默认格式
        "yyyy/MM/dd HH:mm:ss",         // 你的目标格式
        "yyyy/MM/d HH:mm:ss",  
        "yyyy-MM-ddTHH:mm:ss"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateTime.TryParseExact(reader.GetString(), _formats, 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        throw new JsonException($"无法解析时间格式：{reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
    }
}