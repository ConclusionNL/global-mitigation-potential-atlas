using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GMPA.Core.Models.ApiModels
{
    [JsonConverter(typeof(PolymorphismTypeConverter<IGridControlApiModel>))]
    public interface IGridControlApiModel 
    {
        public string Alias { get; set; }
    }

    public class PolymorphismTypeConverter<T> : JsonConverter<T>
    {
        public override T Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            T value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, default(T), options);
                    break;
                default:
                {
                    var type = value.GetType();
                    JsonSerializer.Serialize(writer, value, type, options);
                    break;
                }
            }
        }
    }
}