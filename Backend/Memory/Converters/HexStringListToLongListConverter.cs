using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Memory.Converters
{
    public class HexStringListToLongListConverter : JsonConverter<List<long>>
    {
        public override List<long> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bitMasks = new List<long>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                return bitMasks;
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var hexString = reader.GetString();
                if (string.IsNullOrEmpty(hexString))
                {
                    continue;
                }

                try
                {
                    if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    {
                        bitMasks.Add(Convert.ToInt64(hexString, 16));
                    }
                    else
                    {
                        bitMasks.Add(long.Parse(hexString));
                    }
                }
                catch
                {
                    bitMasks.Add(0);
                }
            }

            return bitMasks;
        }

        public override void Write(Utf8JsonWriter writer, List<long> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (long bitMask in value)
            {
                writer.WriteStringValue("0x" + bitMask.ToString("X"));
            }
            writer.WriteEndArray();
        }
    }
}
