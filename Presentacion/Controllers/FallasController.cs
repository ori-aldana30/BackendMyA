using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FallasController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public FallasController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/Fallas/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var query = await (from f in _context.Falla
                               join a in _context.Activo on f.activoId equals a.idActivo
                               join p in _context.PrioridadFalla on f.prioridadFallaId equals p.idPrioridadFalla
                               join ef in _context.EstadoFalla on f.estadoFallaId equals ef.idEstadoFalla
                               where f.estado == "Activo"
                               select new
                               {
                                   f.codigo,
                                   f.descripcion,
                                   f.fechaReporte,
                                   Activo = a.codigo,
                                   Prioridad = p.nombre,
                                   EstadoFalla = ef.nombre
                               }).ToListAsync();

            return Ok(query);
        }

        // GET: api/Fallas/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> GetByActivo(string codigoActivo)
        {
            var query = await (from f in _context.Falla
                               join a in _context.Activo on f.activoId equals a.idActivo
                               join p in _context.PrioridadFalla on f.prioridadFallaId equals p.idPrioridadFalla
                               join ef in _context.EstadoFalla on f.estadoFallaId equals ef.idEstadoFalla
                               where a.codigo == codigoActivo
                                     && f.estado == "Activo"
                                     && a.estado == "Activo"
                               select new
                               {
                                   f.codigo,
                                   f.descripcion,
                                   f.fechaReporte,
                                   Prioridad = p.nombre,
                                   EstadoFalla = ef.nombre
                               }).ToListAsync();

            return Ok(query);
        }

        [HttpPost("reportar")]
        public async Task<IActionResult> Reportar(FallaCreateDTO dto)
        {
            var activo = await (from a in _context.Activo
                                where a.codigo == dto.codigoActivo && a.estado == "Activo"
                                select a).FirstOrDefaultAsync();

            var prioridad = await (from p in _context.PrioridadFalla
                                   where p.codigo == dto.codigoPrioridad && p.estado == "Activo"
                                   select p).FirstOrDefaultAsync();

            var estadoFalla = await (from ef in _context.EstadoFalla
                                     where ef.codigo == dto.codigoEstadoFalla && ef.estado == "Activo"
                                     select ef).FirstOrDefaultAsync();

            if (activo == null || prioridad == null || estadoFalla == null)
                return BadRequest("Activo/Prioridad/EstadoFalla inválido");

            int ultimoId = await (from f in _context.Falla
                                  orderby f.idFalla descending
                                  select f.idFalla).FirstOrDefaultAsync();

            var falla = new Falla()
            {
                codigo = $"FAL-{(ultimoId + 1):D3}",
                descripcion = dto.descripcion,
                fechaReporte = DateTime.UtcNow, // ✅ CAMBIO AQUÍ
                estado = "Activo",
                activoId = activo.idActivo,
                prioridadFallaId = prioridad.idPrioridadFalla,
                estadoFallaId = estadoFalla.idEstadoFalla
            };

            _context.Falla.Add(falla);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Falla reportada", codigoGenerado = falla.codigo });
        }

        // DELETE: api/Fallas/borrar/FAL-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var falla = await (from f in _context.Falla
                               where f.codigo == codigo
                               select f).FirstOrDefaultAsync();

            if (falla == null)
                return NotFound("Falla no encontrada");

            falla.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Falla desactivada (Soft Delete)");
        }
    }
}