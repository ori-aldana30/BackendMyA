using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionAuditoriaController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public AccionAuditoriaController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/AccionAuditorias/lista
        [HttpGet("lista")]
        public async Task<IActionResult> GetLista()
        {
            var lista = await _context.AccionAuditoria
                .Where(a => a.estado != "Inactivo")
                .Select(a => new
                {
                    a.codigo,
                    a.nombre,
                    a.estado
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/AccionAuditorias/buscar/AUDACC-001
        [HttpGet("buscar/{codigo}")]
        public async Task<IActionResult> Buscar(string codigo)
        {
            var accion = await _context.AccionAuditoria
                .Where(a => a.codigo == codigo && a.estado != "Inactivo")
                .Select(a => new
                {
                    a.codigo,
                    a.nombre,
                    a.estado
                })
                .FirstOrDefaultAsync();

            if (accion == null)
                return NotFound("No existe la acción de auditoría");

            return Ok(accion);
        }

        // POST: api/AccionAuditorias/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El nombre es obligatorio");

            var ultimoId = await _context.AccionAuditoria
                .OrderByDescending(a => a.idAccionAuditoria)
                .Select(a => a.idAccionAuditoria)
                .FirstOrDefaultAsync();

            var nuevo = new AccionAuditoria
            {
                codigo = $"AUDACC-{(ultimoId + 1):D3}",
                nombre = nombre,
                estado = "Activo"
            };

            _context.AccionAuditoria.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Acción creada", codigoGenerado = nuevo.codigo });
        }

        // PUT: api/AccionAuditorias/actualizar/AUDACC-001
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> Actualizar(string codigo, string nombre)
        {
            var accion = await _context.AccionAuditoria
                .FirstOrDefaultAsync(a => a.codigo == codigo && a.estado != "Inactivo");

            if (accion == null)
                return NotFound("No existe la acción");

            accion.nombre = nombre;
            await _context.SaveChangesAsync();

            return Ok("Acción actualizada correctamente");
        }

        // DELETE: api/AccionAuditorias/borrar/AUDACC-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Borrar(string codigo)
        {
            var accion = await _context.AccionAuditoria
                .FirstOrDefaultAsync(a => a.codigo == codigo);

            if (accion == null)
                return NotFound();

            accion.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Acción eliminada");
        }
    }
}
