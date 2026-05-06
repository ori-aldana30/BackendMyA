using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public MantenimientosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/Mantenimientos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var query = await (from m in _context.Mantenimiento
                               join a in _context.Activo on m.activoId equals a.idActivo
                               join tm in _context.TipoMantenimiento on m.tipoMantenimientoId equals tm.idTipoMantenimiento
                               join em in _context.EstadoMantenimiento on m.estadoMantenimientoId equals em.idEstadoMantenimiento
                               where m.estado == "Activo"
                               select new
                               {
                                   m.codigo,
                                   Activo = a.codigo,
                                   Tipo = tm.nombre,
                                   EstadoMantenimiento = em.nombre,
                                   m.fechaInicio,
                                   m.fechaFin
                               }).ToListAsync();

            return Ok(query);
        }

        // GET: api/Mantenimientos/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> GetByActivo(string codigoActivo)
        {
            var query = await (from m in _context.Mantenimiento
                               join a in _context.Activo on m.activoId equals a.idActivo
                               join tm in _context.TipoMantenimiento on m.tipoMantenimientoId equals tm.idTipoMantenimiento
                               join em in _context.EstadoMantenimiento on m.estadoMantenimientoId equals em.idEstadoMantenimiento
                               where a.codigo == codigoActivo
                                     && m.estado == "Activo"
                                     && a.estado == "Activo"
                               select new
                               {
                                   m.codigo,
                                   Tipo = tm.nombre,
                                   EstadoMantenimiento = em.nombre,
                                   m.fechaInicio,
                                   m.fechaFin
                               }).ToListAsync();

            return Ok(query);
        }

        // POST: api/Mantenimientos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(MantenimientoCreateDTO dto)
        {
            var activo = await (from a in _context.Activo
                                where a.codigo == dto.codigoActivo && a.estado == "Activo"
                                select a).FirstOrDefaultAsync();

            var tipo = await (from t in _context.TipoMantenimiento
                              where t.codigo == dto.codigoTipoMantenimiento && t.estado == "Activo"
                              select t).FirstOrDefaultAsync();

            var estado = await (from e in _context.EstadoMantenimiento
                                where e.codigo == dto.codigoEstadoMantenimiento && e.estado == "Activo"
                                select e).FirstOrDefaultAsync();

            if (activo == null || tipo == null || estado == null)
                return BadRequest("Activo/Tipo/Estado inválido");

            int? fallaId = null;

            if (!string.IsNullOrWhiteSpace(dto.codigoFalla))
            {
                var falla = await (from f in _context.Falla
                                   where f.codigo == dto.codigoFalla && f.estado == "Activo"
                                   select f).FirstOrDefaultAsync();

                if (falla != null)
                    fallaId = falla.idFalla;
            }

            int ultimoId = await (from m in _context.Mantenimiento
                                  orderby m.idMantenimiento descending
                                  select m.idMantenimiento).FirstOrDefaultAsync();

            var mantenimiento = new Mantenimiento()
            {
                codigo = $"MAN-{(ultimoId + 1):D3}",
                estado = "Activo",
                fechaInicio = DateTime.UtcNow,
                activoId = activo.idActivo,
                fallaId = fallaId,
                tipoMantenimientoId = tipo.idTipoMantenimiento,
                estadoMantenimientoId = estado.idEstadoMantenimiento
            };

            _context.Mantenimiento.Add(mantenimiento);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Mantenimiento creado", codigoGenerado = mantenimiento.codigo });
        }

        // DELETE: api/Mantenimientos/borrar/MAN-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var mantenimiento = await (from m in _context.Mantenimiento
                                       where m.codigo == codigo
                                       select m).FirstOrDefaultAsync();

            if (mantenimiento == null)
                return NotFound("Mantenimiento no encontrado");

            mantenimiento.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Mantenimiento desactivado (Soft Delete)");
        }
    }
}