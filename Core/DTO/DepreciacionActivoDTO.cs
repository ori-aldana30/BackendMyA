namespace MyAMIS.Core.DTO
{
    public class DepreciacionActivoDTO
    {
        public string codigo { get; set; }
        public string codigoActivo { get; set; }
        public string metodo { get; set; }
        public DateTime fechaCalculo { get; set; }
        public decimal depreciacionAcumulada { get; set; }
        public decimal valorActual { get; set; }
    }
}