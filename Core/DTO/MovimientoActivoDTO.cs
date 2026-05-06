using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class MovimientoActivoDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("codigoActivo")]
        public string CodigoActivo { get; set; }

        [JsonPropertyName("areaOrigenId")]
        public int AreaOrigenId { get; set; }

        [JsonPropertyName("areaDestinoId")]
        public int AreaDestinoId { get; set; }

        [JsonPropertyName("responsableId")]
        public int ResponsableId { get; set; }

        [JsonPropertyName("fechaMovimiento")]
        public DateTime FechaMovimiento { get; set; }

        [JsonPropertyName("motivo")]
        public string Motivo { get; set; }
    }
}