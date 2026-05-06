using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class FallaDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("prioridad")]
        public string Prioridad { get; set; }

        [JsonPropertyName("estadoFalla")]
        public string EstadoFalla { get; set; }

        [JsonPropertyName("fechaReporte")]
        public DateTime FechaReporte { get; set; }

        [JsonPropertyName("codigoActivo")]
        public string CodigoActivo { get; set; }
    }
}