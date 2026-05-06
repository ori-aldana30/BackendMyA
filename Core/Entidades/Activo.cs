using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAMIS.Core.Entidades
{
    public class Activo
    {
        [Key]
        public int idActivo { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public decimal costoCompra { get; set; }
        public string codigoArea { get; set; }
        public string estado { get; set; } = "Activo";

        public int tipoActivold { get; set; }
        public int estadoActivold { get; set; }

        [ForeignKey("tipoActivold")]
        [JsonIgnore]
        public TipoActivo TipoActivo { get; set; }

        [ForeignKey("estadoActivold")]
        [JsonIgnore]
        public EstadoActivo EstadoActivo { get; set; }
    }
}
