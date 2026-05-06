using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class DepreciacionActivo
    {
        [Key]
        public int idDepreciacion { get; set; }

        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        public int activoId { get; set; }
        public int metodoDepreciacionId { get; set; }

        public DateTime fechaCalculo { get; set; } = DateTime.Now;
        public decimal depreciacionAcumulada { get; set; }
        public decimal valorActual { get; set; }

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }

        [ForeignKey("metodoDepreciacionId")]
        [JsonIgnore]
        public MetodoDepreciacion MetodoDepreciacion { get; set; }
    }
}