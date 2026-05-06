using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class Diagnostico
    {
        [Key]
        public int idDiagnostico { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public string descripcion { get; set; }
        public DateTime fechaRegistro { get; set; } = DateTime.Now;

        public int mantenimientoId { get; set; }

        [ForeignKey("mantenimientoId")]
        [JsonIgnore]
        public Mantenimiento Mantenimiento { get; set; }
    }
}