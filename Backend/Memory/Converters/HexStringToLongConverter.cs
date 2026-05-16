using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Memory.Converters
{
    public class HexStringToLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (string.IsNullOrEmpty(str)) return 0;
            
            try
            {
                if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    return Convert.ToInt64(str, 16);
                }
                return long.Parse(str);
            }
            catch 
            { 
                return 0; 
            }
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue("0x" + value.ToString("X"));
        }
    }
}
