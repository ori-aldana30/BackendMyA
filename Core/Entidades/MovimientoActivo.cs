using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class MovimientoActivo
    {
        [Key]
        public int idMovimiento { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public DateTime fechaMovimiento { get; set; } = DateTime.Now;
        public string motivo { get; set; }

        public int activoId { get; set; }

        // FK externos
        public int areaOrigenId { get; set; }   // Gestión Hospitalaria
        public int areaDestinoId { get; set; }  // Gestión Hospitalaria
        public int responsableId { get; set; }  // RRHH

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }
    }
}