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
    public class DiagnosticosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public DiagnosticosController(MyAMISContext context)
        {
            _context = context;
        }

        [HttpGet("porMantenimiento/{codigoMantenimiento}")]
        public async Task<IActionResult> GetPorMantenimiento(string codigoMantenimiento)
        {
            var query = await (
                from d in _context.Diagnostico
                join m in _context.Mantenimiento on d.mantenimientoId equals m.idMantenimiento
                where d.estado != "Inactivo"
                && m.codigo == codigoMantenimiento
                select new DiagnosticoDTO
                {
                    codigo = d.codigo,
                    descripcion = d.descripcion,
                    fechaRegistro = d.fechaRegistro
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(DiagnosticoCreateDTO dto)
        {
            var mantenimiento = await (
                from m in _context.Mantenimiento
                where m.codigo == dto.codigoMantenimiento && m.estado != "Inactivo"
                select m
            ).FirstOrDefaultAsync();

            if (mantenimiento == null)
                return BadRequest("No existe el mantenimiento");

            var ultimoId = await _context.Diagnostico
                .OrderByDescending(x => x.idDiagnostico)
                .Select(x => x.idDiagnostico)
                .FirstOrDefaultAsync();

            var nuevo = new Diagnostico
            {
                codigo = $"DIA-{(ultimoId + 1):D3}",
                descripcion = dto.descripcion,
                mantenimientoId = mantenimiento.idMantenimiento,
                estado = "Activo"
            };

            _context.Diagnostico.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Diagnóstico creado", codigo = nuevo.codigo });
        }

        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var diag = await (
                from d in _context.Diagnostico
                where d.codigo == codigo
                select d
            ).FirstOrDefaultAsync();

            if (diag == null) return NotFound();

            diag.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Diagnóstico eliminado (soft delete)");
        }
    }
}
