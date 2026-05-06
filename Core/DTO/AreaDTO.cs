using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class AreaDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }
    }
}