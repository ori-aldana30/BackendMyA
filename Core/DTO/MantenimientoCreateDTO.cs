namespace MyAMIS.Core.DTO
{
    public class MantenimientoCreateDTO
    {
        public string codigoActivo { get; set; }
        public string? codigoFalla { get; set; }

        public string codigoTipoMantenimiento { get; set; }
        public string codigoEstadoMantenimiento { get; set; }
    }
}