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
    public class EstadoActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public EstadoActivosController(MyAMISContext context)
        {
            _context = context;
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.EstadoActivo.Where(e => e.estado == "Activo").ToListAsync());
        }

        // PUT: api/EstadoActivos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> PutEstadoActivo(string codigo, string nombre)
        {
            var estadoActivo = await (from e in _context.EstadoActivo
                                      where e.codigo == codigo && e.estado != "Inactivo"
                                      select e).FirstOrDefaultAsync();

            if (estadoActivo == null)
                return NotFound("Estado no encontrado");

            estadoActivo.nombre = nombre;

            _context.EstadoActivo.Update(estadoActivo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Actualizado correctamente" });
        }

        // POST: api/EstadoActivos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("crear")]
        public async Task<IActionResult> Post(string nombre)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre es obligatorio");

            // Generar código automático: EST-001, EST-002...
            var ultimoId = await _context.EstadoActivo
                .OrderByDescending(e => e.idEstadoActivo)
                .Select(e => e.idEstadoActivo)
                .FirstOrDefaultAsync();

            var nuevo = new EstadoActivo
            {
                nombre = nombre,
                codigo = $"EST-{(ultimoId + 1):D3}",
                estado = "Activo"
            };

            _context.EstadoActivo.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Estado creado correctamente",
                codigoGenerado = nuevo.codigo
            });
        }

        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            var estado = await _context.EstadoActivo.FirstOrDefaultAsync(e => e.codigo == codigo);
            if (estado == null) return NotFound();

            estado.estado = "Inactivo"; // Soft Delete
            await _context.SaveChangesAsync();
            return Ok("Estado de activo desactivado");
        }

        private bool EstadoActivoExists(int id)
        {
            return _context.EstadoActivo.Any(e => e.idEstadoActivo == id);
        }
    }
}
