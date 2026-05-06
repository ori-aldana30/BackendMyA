using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMantenimientosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public TipoMantenimientosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/TipoMantenimientos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await (from t in _context.TipoMantenimiento
                             where t.estado == "Activo"
                             select new
                             {
                                 t.codigo,
                                 t.nombre
                             }).ToListAsync());
        }

        // POST: api/TipoMantenimientos/crear?nombre=Preventivo
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Nombre obligatorio");

            int ultimoId = await (from t in _context.TipoMantenimiento
                                  orderby t.idTipoMantenimiento descending
                                  select t.idTipoMantenimiento).FirstOrDefaultAsync();

            var nuevo = new TipoMantenimiento()
            {
                codigo = $"TM-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.TipoMantenimiento.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tipo mantenimiento creado", codigoGenerado = nuevo.codigo });
        }

        // DELETE: api/TipoMantenimientos/borrar/TM-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var tipo = await (from t in _context.TipoMantenimiento
                              where t.codigo == codigo
                              select t).FirstOrDefaultAsync();

            if (tipo == null)
                return NotFound("No existe tipo mantenimiento");

            tipo.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Tipo mantenimiento desactivado");
        }
    }
}