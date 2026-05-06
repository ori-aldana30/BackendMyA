using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioridadFallasController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public PrioridadFallasController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/PrioridadFallas/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await (from p in _context.PrioridadFalla
                             where p.estado == "Activo"
                             select new
                             {
                                 p.codigo,
                                 p.nombre
                             }).ToListAsync());
        }

        // POST: api/PrioridadFallas/crear?nombre=Alta
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre es obligatorio");

            int ultimoId = await (from p in _context.PrioridadFalla
                                  orderby p.idPrioridadFalla descending
                                  select p.idPrioridadFalla).FirstOrDefaultAsync();

            var nuevo = new PrioridadFalla()
            {
                codigo = $"PRI-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.PrioridadFalla.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Prioridad creada",
                codigoGenerado = nuevo.codigo
            });
        }

        // DELETE: api/PrioridadFallas/borrar/PRI-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var prioridad = await (from p in _context.PrioridadFalla
                                   where p.codigo == codigo
                                   select p).FirstOrDefaultAsync();

            if (prioridad == null)
                return NotFound("No existe la prioridad");

            prioridad.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Prioridad desactivada");
        }
    }
}