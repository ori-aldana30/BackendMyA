using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasMISController : ControllerBase
    {
        private readonly MyAMISContext _context;

        public ConsultasMISController(MyAMISContext context)
        {
            _context = context;
        }

        // ===============================
        // CONSULTAS GENÉRICAS OBLIGATORIAS
        // ===============================

        // 1) LISTADO GENERAL CON JOIN
        [HttpGet("listadoGeneralJoin")]
        public async Task<IActionResult> ListadoGeneralJoin()
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.estado != "Inactivo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo,
                    TipoActivo = t.nombre,
                    EstadoActivo = e.nombre
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 2) GROUP BY + COUNT
        [HttpGet("activosPorTipoCount")]
        public async Task<IActionResult> ActivosPorTipoCount()
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                where a.estado != "Inactivo" && t.estado != "Inactivo"
                group a by t.nombre into g
                select new
                {
                    Tipo = g.Key,
                    Cantidad = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 3) GROUP BY + SUM (requiere costoCompra, si no existe no se puede)
        // Si no tienes costoCompra, agrega el campo en Activo.
        [HttpGet("activosPorTipoSumatoria")]
        public async Task<IActionResult> ActivosPorTipoSumatoria()
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                where a.estado != "Inactivo" && t.estado != "Inactivo"
                group a by t.nombre into g
                select new
                {
                    Tipo = g.Key,
                    TotalActivos = g.Count(),
                    TotalCosto = g.Sum(x => x.costoCompra)
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 4) BÚSQUEDA FILTRADA POR CÓDIGO
        [HttpGet("buscarActivo/{codigo}")]
        public async Task<IActionResult> BuscarActivo(string codigo)
        {
            var query = await (
                from a in _context.Activo
                where a.codigo == codigo && a.estado != "Inactivo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo
                }
            ).FirstOrDefaultAsync();

            if (query == null)
                return NotFound("Activo no encontrado");

            return Ok(query);
        }

        // 5) NOT EXISTS (Activos sin tipo activo válido)
        [HttpGet("activosSinTipoValido")]
        public async Task<IActionResult> ActivosSinTipoValido()
        {
            var query = await (
                from a in _context.Activo
                where a.estado != "Inactivo"
                && !(from t in _context.TipoActivo
                     where t.estado != "Inactivo"
                     select t.idTipoActivo)
                     .Contains(a.tipoActivold)
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo
                }
            ).ToListAsync();

            return Ok(query);
        }

        // ===============================
        // 10 CONSULTAS DEL DIAGRAMA CASOS DE USO
        // ===============================

        // 6) Activos operativos por tipo
        [HttpGet("activosOperativosPorTipo/{codigoTipo}")]
        public async Task<IActionResult> ActivosOperativosPorTipo(string codigoTipo)
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.estado != "Inactivo"
                && t.codigo == codigoTipo
                && e.nombre == "Operativo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    Tipo = t.nombre,
                    Estado = e.nombre
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 7) Activos dados de baja
        [HttpGet("activosInactivos")]
        public async Task<IActionResult> ActivosInactivos()
        {
            var query = await (
                from a in _context.Activo
                where a.estado == "Inactivo"
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 8) Conteo de activos por estado (GROUP BY)
        [HttpGet("conteoActivosPorEstado")]
        public async Task<IActionResult> ConteoActivosPorEstado()
        {
            var query = await (
                from a in _context.Activo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.estado != "Inactivo"
                group a by e.nombre into g
                select new
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 9) Conteo de activos por marca
        [HttpGet("conteoActivosPorMarca")]
        public async Task<IActionResult> ConteoActivosPorMarca()
        {
            var query = await (
                from a in _context.Activo
                where a.estado != "Inactivo"
                group a by a.marca into g
                select new
                {
                    Marca = g.Key,
                    Cantidad = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 10) Buscar activos por marca
        [HttpGet("buscarPorMarca/{marca}")]
        public async Task<IActionResult> BuscarPorMarca(string marca)
        {
            var query = await (
                from a in _context.Activo
                where a.estado != "Inactivo" && a.marca == marca
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.modelo,
                    a.marca
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 11) Buscar activos por modelo
        [HttpGet("buscarPorModelo/{modelo}")]
        public async Task<IActionResult> BuscarPorModelo(string modelo)
        {
            var query = await (
                from a in _context.Activo
                where a.estado != "Inactivo" && a.modelo == modelo
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 12) Activos por tipo (detalle)
        [HttpGet("activosPorTipo/{codigoTipo}")]
        public async Task<IActionResult> ActivosPorTipo(string codigoTipo)
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                where a.estado != "Inactivo"
                && t.codigo == codigoTipo
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo,
                    Tipo = t.nombre
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 13) Activos sin estado válido (NOT EXISTS)
        [HttpGet("activosSinEstadoValido")]
        public async Task<IActionResult> ActivosSinEstadoValido()
        {
            var query = await (
                from a in _context.Activo
                where a.estado != "Inactivo"
                && !(from e in _context.EstadoActivo
                     where e.estado != "Inactivo"
                     select e.idEstadoActivo)
                     .Contains(a.estadoActivold)
                select new
                {
                    a.codigo,
                    a.nombre,
                    a.marca,
                    a.modelo
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 14) Resumen general (total activos, activos, inactivos)
        [HttpGet("resumenGeneralActivos")]
        public async Task<IActionResult> ResumenGeneralActivos()
        {
            var total = await _context.Activo.CountAsync();
            var activos = await _context.Activo.CountAsync(a => a.estado != "Inactivo");
            var inactivos = await _context.Activo.CountAsync(a => a.estado == "Inactivo");

            return Ok(new
            {
                TotalRegistros = total,
                TotalActivos = activos,
                TotalInactivos = inactivos
            });
        }

        // 15) Activos por tipo y estado (reporte mixto)
        [HttpGet("reporteTipoEstado")]
        public async Task<IActionResult> ReporteTipoEstado()
        {
            var query = await (
                from a in _context.Activo
                join t in _context.TipoActivo on a.tipoActivold equals t.idTipoActivo
                join e in _context.EstadoActivo on a.estadoActivold equals e.idEstadoActivo
                where a.estado != "Inactivo"
                group a by new { Tipo = t.nombre, Estado = e.nombre } into g
                select new
                {
                    g.Key.Tipo,
                    g.Key.Estado,
                    Cantidad = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }
    }
}
