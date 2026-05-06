using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public TipoDocumentosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/TipoDocumentos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await (from t in _context.TipoDocumento
                             where t.estado == "Activo"
                             select new
                             {
                                 t.codigo,
                                 t.nombre
                             }).ToListAsync());
        }

        // POST: api/TipoDocumentos/crear?nombre=Informe Tecnico
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Nombre obligatorio");

            int ultimoId = await (from t in _context.TipoDocumento
                                  orderby t.idTipoDocumento descending
                                  select t.idTipoDocumento).FirstOrDefaultAsync();

            var nuevo = new TipoDocumento()
            {
                codigo = $"TD-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.TipoDocumento.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tipo documento creado", codigoGenerado = nuevo.codigo });
        }

        // DELETE: api/TipoDocumentos/borrar/TD-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var tipo = await (from t in _context.TipoDocumento
                              where t.codigo == codigo
                              select t).FirstOrDefaultAsync();

            if (tipo == null)
                return NotFound("No existe tipo documento");

            tipo.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Tipo documento desactivado");
        }
    }
}