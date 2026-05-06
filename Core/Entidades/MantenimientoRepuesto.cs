using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class MantenimientoRepuesto
    {
        [Key]
        public int idMantenimientoRepuesto { get; set; }

        public string estado { get; set; } = "Activo";

        public int mantenimientoId { get; set; }
        public int repuestoId { get; set; } // externo inventarios
        public int cantidad { get; set; }

        [ForeignKey("mantenimientoId")]
        [JsonIgnore]
        public Mantenimiento Mantenimiento { get; set; }
    }
}