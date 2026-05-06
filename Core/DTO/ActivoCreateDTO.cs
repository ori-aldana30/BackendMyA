namespace MyAMIS.Core.DTO
{
    public class ActivoCreateDTO
    {
        public string nombre { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public decimal costoCompra { get; set; }

        public string codigoTipoActivo { get; set; }
        public string codigoEstadoActivo { get; set; }

        public string codigoArea { get; set; }
    }
}