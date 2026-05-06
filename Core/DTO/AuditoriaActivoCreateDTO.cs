namespace MyAMIS.Core.DTO
{
    public class AuditoriaActivoCreateDTO
    {
        public string codigoActivo { get; set; }
        public int usuarioId { get; set; }
        public string codigoAccionAuditoria { get; set; }
        public string detalle { get; set; }
    }
}