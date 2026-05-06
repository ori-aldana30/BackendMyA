using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class Mantenimiento
    {
        [Key]
        public int idMantenimiento { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public DateTime fechaInicio { get; set; } = DateTime.Now;
        public DateTime? fechaFin { get; set; }

        public int activoId { get; set; }
        public int? fallaId { get; set; }

        public int tipoMantenimientoId { get; set; }
        public int estadoMantenimientoId { get; set; }

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }

        [ForeignKey("fallaId")]
        [JsonIgnore]
        public Falla Falla { get; set; }

        [ForeignKey("tipoMantenimientoId")]
        [JsonIgnore]
        public TipoMantenimiento TipoMantenimiento { get; set; }

        [ForeignKey("estadoMantenimientoId")]
        [JsonIgnore]
        public EstadoMantenimiento EstadoMantenimiento { get; set; }

        // FK externos (otros microservicios)
        public int? tecnicoId { get; set; }  // RRHH
    }
}