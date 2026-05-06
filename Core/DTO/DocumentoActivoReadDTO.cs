namespace MyAMIS.Core.DTO
{
    public class DocumentoActivoReadDTO
    {
        public string codigo { get; set; }
        public string codigoActivo { get; set; }
        public string tipoDocumento { get; set; }
        public string referenciaDocumento { get; set; }
        public DateTime fechaRegistro { get; set; }

        // Datos del microservicio Documentación
        public string archivoUrl { get; set; }
        public string nombreArchivo { get; set; }
        public string tipoArchivo { get; set; }
        public string tamañoArchivo { get; set; }
        public DateTime? fechaEmision { get; set; }
    }
}