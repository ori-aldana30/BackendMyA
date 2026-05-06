using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class ProductoDTO
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

    }
}
