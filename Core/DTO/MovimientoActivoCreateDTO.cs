namespace MyAMIS.Core.DTO
{
    public class MovimientoActivoCreateDTO
    {
        public string codigoActivo { get; set; }
        public int areaOrigenId { get; set; }
        public int areaDestinoId { get; set; }
        public int responsableId { get; set; }
        public string motivo { get; set; }
    }
}