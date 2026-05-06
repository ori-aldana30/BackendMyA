using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class MantenimientoDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("codigoActivo")]
        public string CodigoActivo { get; set; }

        [JsonPropertyName("tipoMantenimiento")]
        public string TipoMantenimiento { get; set; }

        [JsonPropertyName("estadoMantenimiento")]
        public string EstadoMantenimiento { get; set; }

        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonPropertyName("fechaFin")]
        public DateTime? FechaFin { get; set; }
    }
}