using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class TipoDocumento
    {
        [Key]
        public int idTipoDocumento { get; set; }

        public string codigo { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; } = "Activo";

        [JsonIgnore]
        public List<DocumentoActivo> DocumentoActivos { get; set; }
    }
}