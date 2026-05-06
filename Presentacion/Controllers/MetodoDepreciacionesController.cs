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
    public class MetodoDepreciacionesController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public MetodoDepreciacionesController(MyAMISContext context)
        {
            _context = context;
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            return Ok(await _context.MetodoDepreciacion
                .Where(m => m.estado != "Inactivo")
                .Select(m => new
                {
                    m.codigo,
                    m.nombre,
                    m.estado
                })
                .ToListAsync());
        }

        [HttpGet("buscar/{codigo}")]
        public async Task<IActionResult> Buscar(string codigo)
        {
            var metodo = await _context.MetodoDepreciacion
                .Where(m => m.codigo == codigo && m.estado != "Inactivo")
                .Select(m => new
                {
                    m.codigo,
                    m.nombre,
                    m.estado
                })
                .FirstOrDefaultAsync();

            if (metodo == null)
                return NotFound("Método no encontrado");

            return Ok(metodo);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Nombre obligatorio");

            var ultimoId = await _context.MetodoDepreciacion
                .OrderByDescending(m => m.idMetodoDepreciacion)
                .Select(m => m.idMetodoDepreciacion)
                .FirstOrDefaultAsync();

            var nuevo = new MetodoDepreciacion
            {
                codigo = $"DEP-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.MetodoDepreciacion.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Método creado", codigoGenerado = nuevo.codigo });
        }

        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> Actualizar(string codigo, string nombre)
        {
            var metodo = await _context.MetodoDepreciacion
                .FirstOrDefaultAsync(m => m.codigo == codigo && m.estado != "Inactivo");

            if (metodo == null)
                return NotFound("Método no encontrado");

            metodo.nombre = nombre;
            await _context.SaveChangesAsync();

            return Ok("Método actualizado correctamente");
        }

        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Borrar(string codigo)
        {
            var metodo = await _context.MetodoDepreciacion
                .FirstOrDefaultAsync(m => m.codigo == codigo);

            if (metodo == null)
                return NotFound();

            metodo.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Método eliminado (Soft Delete)");
        }
    }
}
