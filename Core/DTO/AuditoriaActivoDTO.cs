namespace MyAMIS.Core.DTO
{
    public class AuditoriaActivoDTO
    {
        public string codigo { get; set; }
        public string activoCodigo { get; set; }
        public int usuarioId { get; set; }
        public string accion { get; set; }
        public DateTime fechaHora { get; set; }
        public string detalle { get; set; }
    }
}