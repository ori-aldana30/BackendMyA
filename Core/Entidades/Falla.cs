using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class Falla
    {
        [Key]
        public int idFalla { get; set; }

        public string codigo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaReporte { get; set; } = DateTime.Now;

        public string estado { get; set; } = "Activo";

        public int activoId { get; set; }
        public int prioridadFallaId { get; set; }
        public int estadoFallaId { get; set; }

        [ForeignKey("activoId")]
        [JsonIgnore]
        public Activo Activo { get; set; }

        [ForeignKey("prioridadFallaId")]
        [JsonIgnore]
        public PrioridadFalla PrioridadFalla { get; set; }

        [ForeignKey("estadoFallaId")]
        [JsonIgnore]
        public EstadoFalla EstadoFalla { get; set; }
    }
}