using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoFallasController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public EstadoFallasController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/EstadoFallas/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await (from e in _context.EstadoFalla
                             where e.estado == "Activo"
                             select new
                             {
                                 e.codigo,
                                 e.nombre
                             }).ToListAsync());
        }

        // POST: api/EstadoFallas/crear?nombre=Pendiente
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre es obligatorio");

            int ultimoId = await (from e in _context.EstadoFalla
                                  orderby e.idEstadoFalla descending
                                  select e.idEstadoFalla).FirstOrDefaultAsync();

            var nuevo = new EstadoFalla()
            {
                codigo = $"EF-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.EstadoFalla.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Estado de falla creado",
                codigoGenerado = nuevo.codigo
            });
        }

        // DELETE: api/EstadoFallas/borrar/EF-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var estado = await (from e in _context.EstadoFalla
                                where e.codigo == codigo
                                select e).FirstOrDefaultAsync();

            if (estado == null)
                return NotFound("No existe el estado de falla");

            estado.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Estado de falla desactivado");
        }
    }
}