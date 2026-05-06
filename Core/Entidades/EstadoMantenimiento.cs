using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class EstadoMantenimiento
    {
        [Key]
        public int idEstadoMantenimiento { get; set; }

        public string codigo { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; } = "Activo";

        [JsonIgnore]
        public List<Mantenimiento> Mantenimientos { get; set; }
    }
}