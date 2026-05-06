namespace MyAMIS.Core.DTO
{
    public class DocumentoActivoCreateDTO
    {
        public string codigoActivo { get; set; }
        public string codigoTipoDocumento { get; set; }

        public string referenciaDocumento { get; set; } // id externo del microservicio Documentación
        public string? codigoMantenimiento { get; set; }
    }
}