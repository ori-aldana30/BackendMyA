using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class ActivoDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("marca")]
        public string Marca { get; set; }

        [JsonPropertyName("modelo")]
        public string Modelo { get; set; }

        [JsonPropertyName("tipoActivo")]
        public string TipoActivo { get; set; }

        [JsonPropertyName("estadoActivo")]
        public string EstadoActivo { get; set; }
    }
}