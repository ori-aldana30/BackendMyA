using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class EstadoActivo
    {
        [Key]
        public int idEstadoActivo { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        // Relación: Un estado puede estar en muchos activos
        [JsonIgnore]
        public List<Activo> Activos { get; set; }
    }
}
