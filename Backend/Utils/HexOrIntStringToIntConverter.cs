using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Utils
{
    /// <summary>
    /// Converts JSON string values that may be hexadecimal ("0x48"),
    /// decimal ("20"), or negative ("-4") into int.
    /// Used for Digimon memory offsets which mix these formats in the JSON database.
    /// </summary>
    public class HexOrIntStringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            var str = reader.GetString();
            if (string.IsNullOrEmpty(str)) return 0;

            try
            {
                if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    return Convert.ToInt32(str, 16);
                }
                return int.Parse(str);
            }
            catch
            {
                return 0;
            }
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
