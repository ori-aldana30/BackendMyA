using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public MovimientoActivosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/MovimientoActivos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var query = await (from m in _context.MovimientoActivo
                               join a in _context.Activo on m.activoId equals a.idActivo
                               where m.estado == "Activo"
                               select new
                               {
                                   m.codigo,
                                   Activo = a.codigo,
                                   m.areaOrigenId,
                                   m.areaDestinoId,
                                   m.responsableId,
                                   m.fechaMovimiento,
                                   m.motivo
                               }).ToListAsync();

            return Ok(query);
        }

        // GET: api/MovimientoActivos/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> GetByActivo(string codigoActivo)
        {
            var query = await (from m in _context.MovimientoActivo
                               join a in _context.Activo on m.activoId equals a.idActivo
                               where a.codigo == codigoActivo
                                     && m.estado == "Activo"
                                     && a.estado == "Activo"
                               select new
                               {
                                   m.codigo,
                                   m.areaOrigenId,
                                   m.areaDestinoId,
                                   m.responsableId,
                                   m.fechaMovimiento,
                                   m.motivo
                               }).ToListAsync();

            return Ok(query);
        }

        // POST: api/MovimientoActivos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(MovimientoActivoCreateDTO dto)
        {
            var activo = await (from a in _context.Activo
                                where a.codigo == dto.codigoActivo && a.estado == "Activo"
                                select a).FirstOrDefaultAsync();

            if (activo == null)
                return BadRequest("Activo no encontrado");

            int ultimoId = await (from m in _context.MovimientoActivo
                                  orderby m.idMovimiento descending
                                  select m.idMovimiento).FirstOrDefaultAsync();

            var movimiento = new MovimientoActivo()
            {
                codigo = $"MOV-{(ultimoId + 1):D3}",
                estado = "Activo",
                activoId = activo.idActivo,
                areaOrigenId = dto.areaOrigenId,
                areaDestinoId = dto.areaDestinoId,
                responsableId = dto.responsableId,
                motivo = dto.motivo,
                fechaMovimiento = DateTime.Now
            };

            _context.MovimientoActivo.Add(movimiento);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Movimiento registrado", codigoGenerado = movimiento.codigo });
        }

        // DELETE: api/MovimientoActivos/borrar/MOV-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var movimiento = await (from m in _context.MovimientoActivo
                                    where m.codigo == codigo
                                    select m).FirstOrDefaultAsync();

            if (movimiento == null)
                return NotFound("Movimiento no encontrado");

            movimiento.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Movimiento desactivado");
        }
    }
}