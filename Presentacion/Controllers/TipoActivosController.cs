using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public TipoActivosController(MyAMISContext context)
        {
            _context = context;
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Get()
        {
            // Solo tipos activos para que el frontend los muestre en los combos/dropdowns
            return Ok(await _context.TipoActivo.Where(t => t.estado == "Activo").ToListAsync());
        }

        // PUT: api/TipoActivos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> PutTipoActivo(string codigo, string nombre)
        {
            var tipoActivo = await (from t in _context.TipoActivo
                                    where t.codigo == codigo && t.estado != "Inactivo"
                                    select t).FirstOrDefaultAsync();

            if (tipoActivo == null)
                return NotFound("Tipo no encontrado");

            tipoActivo.nombre = nombre;

            _context.TipoActivo.Update(tipoActivo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Actualizado correctamente" });
        }

        // POST: api/TipoActivos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("crear")]
        public async Task<IActionResult> Post(string nombre)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre es obligatorio");

            // Generar código automático
            var ultimoId = await _context.TipoActivo
                .OrderByDescending(t => t.idTipoActivo)
                .Select(t => t.idTipoActivo)
                .FirstOrDefaultAsync();

            var nuevo = new TipoActivo
            {
                nombre = nombre,
                codigo = $"TIPO-{(ultimoId + 1):D3}",
                estado = "Activo"
            };

            _context.TipoActivo.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Tipo creado correctamente",
                codigoGenerado = nuevo.codigo
            });
        }

        // DELETE: api/TipoActivos/5
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var tipo = await _context.TipoActivo.FirstOrDefaultAsync(t => t.codigo == codigo);
            if (tipo == null) return NotFound();

            tipo.estado = "Inactivo"; // Soft Delete
            await _context.SaveChangesAsync();
            return Ok("Tipo de activo desactivado");
        }

        private bool TipoActivoExists(int id)
        {
            return _context.TipoActivo.Any(e => e.idTipoActivo == id);
        }
    }
}
