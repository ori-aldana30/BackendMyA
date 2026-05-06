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
    public class DepreciacionActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public DepreciacionActivosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/DepreciacionActivos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var query = await (
                from d in _context.DepreciacionActivo
                join a in _context.Activo on d.activoId equals a.idActivo
                join m in _context.MetodoDepreciacion on d.metodoDepreciacionId equals m.idMetodoDepreciacion
                where d.estado != "Inactivo"
                select new
                {
                    d.codigo,
                    Activo = a.codigo,
                    Metodo = m.nombre,
                    d.fechaCalculo,
                    d.depreciacionAcumulada,
                    d.valorActual
                }
            ).ToListAsync();

            return Ok(query);
        }

        // GET: api/DepreciacionActivos/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> PorActivo(string codigoActivo)
        {
            var query = await (
                from d in _context.DepreciacionActivo
                join a in _context.Activo on d.activoId equals a.idActivo
                join m in _context.MetodoDepreciacion on d.metodoDepreciacionId equals m.idMetodoDepreciacion
                where d.estado != "Inactivo"
                && a.codigo == codigoActivo
                select new
                {
                    d.codigo,
                    Metodo = m.nombre,
                    d.fechaCalculo,
                    d.depreciacionAcumulada,
                    d.valorActual
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST: api/DepreciacionActivos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(
            string codigoActivo,
            string codigoMetodo,
            decimal depreciacionAcumulada,
            decimal valorActual)
        {
            var activo = await _context.Activo
                .FirstOrDefaultAsync(a => a.codigo == codigoActivo && a.estado != "Inactivo");

            if (activo == null)
                return BadRequest("Activo no encontrado");

            var metodo = await _context.MetodoDepreciacion
                .FirstOrDefaultAsync(m => m.codigo == codigoMetodo && m.estado != "Inactivo");

            if (metodo == null)
                return BadRequest("Método no encontrado");

            var ultimoId = await _context.DepreciacionActivo
                .OrderByDescending(d => d.idDepreciacion)
                .Select(d => d.idDepreciacion)
                .FirstOrDefaultAsync();

            var nuevo = new DepreciacionActivo
            {
                codigo = $"DEPA-{(ultimoId + 1):D3}",
                activoId = activo.idActivo,
                metodoDepreciacionId = metodo.idMetodoDepreciacion,
                fechaCalculo = DateTime.Now,
                depreciacionAcumulada = depreciacionAcumulada,
                valorActual = valorActual,
                estado = "Activo"
            };

            _context.DepreciacionActivo.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Depreciación registrada", codigoGenerado = nuevo.codigo });
        }

        // PUT: api/DepreciacionActivos/actualizar/DEPA-001
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> Actualizar(
            string codigo,
            decimal depreciacionAcumulada,
            decimal valorActual)
        {
            var dep = await _context.DepreciacionActivo
                .FirstOrDefaultAsync(d => d.codigo == codigo && d.estado != "Inactivo");

            if (dep == null)
                return NotFound("Depreciación no encontrada");

            dep.depreciacionAcumulada = depreciacionAcumulada;
            dep.valorActual = valorActual;
            dep.fechaCalculo = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok("Depreciación actualizada correctamente");
        }

        // DELETE: api/DepreciacionActivos/borrar/DEPA-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> Borrar(string codigo)
        {
            var dep = await _context.DepreciacionActivo
                .FirstOrDefaultAsync(d => d.codigo == codigo);

            if (dep == null)
                return NotFound();

            dep.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Depreciación eliminada (Soft Delete)");
        }
    }
}
