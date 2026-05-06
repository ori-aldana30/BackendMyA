using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public ActivosController(MyAMISContext context)
        {
            _context = context;
        }

        // GET: api/Activos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> GetLista()
        {
            var query = await (
                from a in _context.Activo
                where a.estado == "Activo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo,
                    a.codigoArea
                }
            ).ToListAsync();

            return Ok(query);
        }

        // GET: api/Activos/buscar/ACT-001
        [HttpGet("buscar/{codigo}")]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            var activo = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.codigo == codigo && a.estado == "Activo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo,
                    a.costoCompra,
                    a.codigoArea,
                    TipoActivo = t.nombre,
                    EstadoActivo = e.nombre
                }
            ).FirstOrDefaultAsync();

            if (activo == null)
                return NotFound("Activo no encontrado");

            return Ok(activo);
        }

        // POST: api/Activos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(ActivoCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                return BadRequest("Nombre obligatorio");

            if (string.IsNullOrWhiteSpace(dto.codigoTipoActivo))
                return BadRequest("Código de tipo activo obligatorio");

            if (string.IsNullOrWhiteSpace(dto.codigoEstadoActivo))
                return BadRequest("Código de estado activo obligatorio");

            if (string.IsNullOrWhiteSpace(dto.codigoArea))
                return BadRequest("Código de área obligatorio");

            var tipo = await (
                from t in _context.TipoActivo
                where t.codigo == dto.codigoTipoActivo && t.estado == "Activo"
                select t
            ).FirstOrDefaultAsync();

            var estado = await (
                from e in _context.EstadoActivo
                where e.codigo == dto.codigoEstadoActivo && e.estado == "Activo"
                select e
            ).FirstOrDefaultAsync();

            if (tipo == null || estado == null)
                return BadRequest("TipoActivo o EstadoActivo inválido");

            int ultimoId = await (
                from a in _context.Activo
                orderby a.idActivo descending
                select a.idActivo
            ).FirstOrDefaultAsync();

            var nuevo = new Activo()
            {
                codigo = $"ACT-{(ultimoId + 1):D3}",
                nombre = dto.nombre,
                marca = dto.marca,
                modelo = dto.modelo,
                costoCompra = dto.costoCompra,
                codigoArea = dto.codigoArea,
                estado = "Activo",
                tipoActivold = tipo.idTipoActivo,
                estadoActivold = estado.idEstadoActivo
            };

            _context.Activo.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Activo creado correctamente",
                codigoGenerado = nuevo.codigo
            });
        }

        // PUT: api/Activos/actualizar/ACT-001
        [HttpPut("actualizar/{codigo}")]
        public async Task<IActionResult> Actualizar(string codigo, ActivoUpdateDTO dto)
        {
            var activo = await (
                from a in _context.Activo
                where a.codigo == codigo && a.estado == "Activo"
                select a
            ).FirstOrDefaultAsync();

            if (activo == null)
                return NotFound("Activo no encontrado");

            var tipo = await (
                from t in _context.TipoActivo
                where t.codigo == dto.codigoTipoActivo && t.estado == "Activo"
                select t
            ).FirstOrDefaultAsync();

            var estadoActivo = await (
                from e in _context.EstadoActivo
                where e.codigo == dto.codigoEstadoActivo && e.estado == "Activo"
                select e
            ).FirstOrDefaultAsync();

            if (tipo == null || estadoActivo == null)
                return BadRequest("TipoActivo o EstadoActivo inválido");

            activo.nombre = dto.nombre;
            activo.marca = dto.marca;
            activo.modelo = dto.modelo;
            activo.costoCompra = dto.costoCompra;
            activo.codigoArea = dto.codigoArea;

            activo.tipoActivold = tipo.idTipoActivo;
            activo.estadoActivold = estadoActivo.idEstadoActivo;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Activo actualizado correctamente" });
        }

        // DELETE: api/Activos/borrar/ACT-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var activo = await (
                from a in _context.Activo
                where a.codigo == codigo
                select a
            ).FirstOrDefaultAsync();

            if (activo == null)
                return NotFound("No existe el activo");

            activo.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Activo desactivado (Soft Delete)");
        }

        // GET: api/Activos/query
        [HttpGet("query")]
        public async Task<IActionResult> QueryActivos()
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.estado == "Activo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo,
                    a.codigoArea,
                    Tipo = t.nombre,
                    Estado = e.nombre
                }
            ).ToListAsync();

            return Ok(query);
        }
    }
}