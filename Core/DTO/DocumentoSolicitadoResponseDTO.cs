using System.Text.Json.Serialization;

namespace MyAMIS.Core.DTO
{
    public class DocumentoSolicitadoResponseDTO
    {
        [JsonPropertyName("data")]
        public List<DocumentoSolicitadoDTO> Data { get; set; }
    }
}