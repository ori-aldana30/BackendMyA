using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class AuditoriaActivo
    {
        [Key]
        public int idAuditoria { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public int activoId { get; set; }
        public int usuarioId { get; set; } // externo (seguridad o RRHH)
        public int accionAuditoriaId { get; set; }

        public DateTime fechaHora { get; set; } = DateTime.Now;
        public string detalle { get; set; }

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }

        [ForeignKey("accionAuditoriaId")]
        [JsonIgnore]
        public AccionAuditoria AccionAuditoria { get; set; }
    }
}