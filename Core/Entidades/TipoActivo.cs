using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class TipoActivo
    {
        [Key]
        public int idTipoActivo { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string estado { get; set; } = "Activo";

        // Relación: Un tipo puede tener muchos activos
        [JsonIgnore]
        public List<Activo> Activos { get; set; }
    }
}
