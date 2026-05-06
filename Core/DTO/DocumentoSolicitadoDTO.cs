using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class DocumentoSolicitadoDTO
    {
        [JsonPropertyName("docSolicitadoId")]
        public int DocSolicitadoId { get; set; }

        [JsonPropertyName("codigoDocSolicitado")]
        public string CodigoDocSolicitado { get; set; }

        [JsonPropertyName("fechaEmision")]
        public DateTime FechaEmision { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("archivoUrl")]
        public string ArchivoUrl { get; set; }

        [JsonPropertyName("codigoSolicitud")]
        public string CodigoSolicitud { get; set; }

        [JsonPropertyName("codigoTipoDoc")]
        public string CodigoTipoDoc { get; set; }

        [JsonPropertyName("nombreArchivo")]
        public string NombreArchivo { get; set; }

        [JsonPropertyName("tipoArchivo")]
        public string TipoArchivo { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("tamañoArchivo")]
        public string TamañoArchivo { get; set; }
    }
}