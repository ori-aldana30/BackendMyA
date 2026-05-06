namespace MyAMIS.Core.DTO
{
    public class DepreciacionActivoCreateDTO
    {
        public string codigoActivo { get; set; }
        public string codigoMetodo { get; set; }
        public decimal depreciacionAcumulada { get; set; }
        public decimal valorActual { get; set; }
    }
}