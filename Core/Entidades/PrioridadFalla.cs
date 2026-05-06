using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class PrioridadFalla
    {
        [Key]
        public int idPrioridadFalla { get; set; }

        public string codigo { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; } = "Activo";

        [JsonIgnore]
        public List<Falla> Fallas { get; set; }
    }
}