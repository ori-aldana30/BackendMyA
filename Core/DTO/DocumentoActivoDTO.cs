using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class DocumentoActivoDTO
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("codigoActivo")]
        public string CodigoActivo { get; set; }

        [JsonPropertyName("tipoDocumento")]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("referenciaDocumento")]
        public string ReferenciaDocumento { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public DateTime fechaRegistro { get; set; } = DateTime.UtcNow;
    }
}