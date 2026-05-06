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
    public class AuditoriaActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public AuditoriaActivosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/AuditoriaActivos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var query = await (
                from au in _context.AuditoriaActivo
                join ac in _context.Activo on au.activoId equals ac.idActivo
                join aa in _context.AccionAuditoria on au.accionAuditoriaId equals aa.idAccionAuditoria
                where au.estado != "Inactivo"
                select new
                {
                    au.codigo,
                    Activo = ac.codigo,
                    au.usuarioId,
                    Accion = aa.nombre,
                    au.fechaHora,
                    au.detalle
                }
            ).ToListAsync();

            return Ok(query);
        }

        // GET: api/AuditoriaActivos/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> PorActivo(string codigoActivo)
        {
            var query = await (
                from au in _context.AuditoriaActivo
                join ac in _context.Activo on au.activoId equals ac.idActivo
                join aa in _context.AccionAuditoria on au.accionAuditoriaId equals aa.idAccionAuditoria
                where au.estado != "Inactivo"
                && ac.codigo == codigoActivo
                select new
                {
                    au.codigo,
                    au.fechaHora,
                    au.usuarioId,
                    Accion = aa.nombre,
                    au.detalle
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST: api/AuditoriaActivos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(string codigoActivo, string codigoAccion, int usuarioId, string detalle)
        {
            var activo = await _context.Activo
                .FirstOrDefaultAsync(a => a.codigo == codigoActivo && a.estado != "Inactivo");

            if (activo == null)
                return BadRequest("Activo no encontrado");

            var accion = await _context.AccionAuditoria
                .FirstOrDefaultAsync(a => a.codigo == codigoAccion && a.estado != "Inactivo");

            if (accion == null)
                return BadRequest("Acción auditoría no encontrada");

            var ultimoId = await _context.AuditoriaActivo
                .OrderByDescending(a => a.idAuditoria)
                .Select(a => a.idAuditoria)
                .FirstOrDefaultAsync();

            var nuevo = new AuditoriaActivo
            {
                codigo = $"AUD-{(ultimoId + 1):D3}",
                activoId = activo.idActivo,
                accionAuditoriaId = accion.idAccionAuditoria,
                usuarioId = usuarioId,
                detalle = detalle,
                fechaHora = DateTime.Now,
                estado = "Activo"
            };

            _context.AuditoriaActivo.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Auditoría registrada", codigoGenerado = nuevo.codigo });
        }

        // PUT: api/AuditoriaActivos/actualizar/AUD-001
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> Actualizar(string codigo, string detalle)
        {
            var auditoria = await _context.AuditoriaActivo
                .FirstOrDefaultAsync(a => a.codigo == codigo && a.estado != "Inactivo");

            if (auditoria == null)
                return NotFound("No existe la auditoría");

            auditoria.detalle = detalle;
            await _context.SaveChangesAsync();

            return Ok("Auditoría actualizada correctamente");
        }

        // DELETE: api/AuditoriaActivos/borrar/AUD-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Borrar(string codigo)
        {
            var auditoria = await _context.AuditoriaActivo
                .FirstOrDefaultAsync(a => a.codigo == codigo);

            if (auditoria == null)
                return NotFound();

            auditoria.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Auditoría eliminada");
        }
    }
}
