using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class DocumentoActivo
    {
        [Key]
        public int idDocumentoActivo { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public string referenciaDocumento { get; set; } // FK externa Documentación
        public DateTime fechaRegistro { get; set; } = DateTime.Now;

        public int activoId { get; set; }
        public int tipoDocumentoId { get; set; }

        public int? mantenimientoId { get; set; }

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }

        [ForeignKey("tipoDocumentoId")]
        [JsonIgnore]
        public TipoDocumento TipoDocumento { get; set; }

        [ForeignKey("mantenimientoId")]
        [JsonIgnore]
        public Mantenimiento Mantenimiento { get; set; }
    }
}