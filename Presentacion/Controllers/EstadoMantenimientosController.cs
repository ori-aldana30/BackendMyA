using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoMantenimientosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public EstadoMantenimientosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/EstadoMantenimientos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await (from e in _context.EstadoMantenimiento
                             where e.estado == "Activo"
                             select new
                             {
                                 e.codigo,
                                 e.nombre
                             }).ToListAsync());
        }

        // POST: api/EstadoMantenimientos/crear?nombre=En Proceso
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Nombre obligatorio");

            int ultimoId = await (from e in _context.EstadoMantenimiento
                                  orderby e.idEstadoMantenimiento descending
                                  select e.idEstadoMantenimiento).FirstOrDefaultAsync();

            var nuevo = new EstadoMantenimiento()
            {
                codigo = $"EM-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.EstadoMantenimiento.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estado mantenimiento creado", codigoGenerado = nuevo.codigo });
        }

        // DELETE: api/EstadoMantenimientos/borrar/EM-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var estado = await (from e in _context.EstadoMantenimiento
                                where e.codigo == codigo
                                select e).FirstOrDefaultAsync();

            if (estado == null)
                return NotFound("No existe estado mantenimiento");

            estado.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Estado mantenimiento desactivado");
        }
    }
}