using MyAMIS.Core.DTO;

namespace MyAMIS.Core.Mappers
{
    public static class AreaMapper
    {
        public static AreaReadDTO ToReadDTO(AreaDTO dto)
        {
            return new AreaReadDTO
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion
            };
        }
    }
}