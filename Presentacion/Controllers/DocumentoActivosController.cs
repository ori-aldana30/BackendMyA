using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Entidades;
using MyAMIS.Soporte;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoActivosController : ControllerBase
    {
        private readonly MyAMISContext _context;
        private readonly HttpClient _http;

        public DocumentoActivosController(MyAMISContext context, HttpClient http)
        {
            _context = context;
            _http = http;
        }

        // GET: api/DocumentoActivos/lista
        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var documentosBD = await (
                from d in _context.DocumentoActivo
                join a in _context.Activo on d.activoId equals a.idActivo
                join t in _context.TipoDocumento on d.tipoDocumentoId equals t.idTipoDocumento
                where d.estado == "Activo"
                select new
                {
                    d.codigo,
                    codigoActivo = a.codigo,
                    tipoDocumento = t.nombre,
                    d.referenciaDocumento,
                    d.fechaRegistro
                }
            ).ToListAsync();

            var docsExternos = await _http.GetFromJsonAsync<List<DocumentoSolicitadoDTO>>(
                $"{Constantes.URL_DOCUMENTACION}api/DocumentoSolicitadoes/ListarTodos"
            );

            if (docsExternos == null)
                docsExternos = new List<DocumentoSolicitadoDTO>();

            var resultado = (
                from d in documentosBD
                join ext in docsExternos
                    on d.referenciaDocumento equals ext.CodigoDocSolicitado
                    into joinExt
                from ext in joinExt.DefaultIfEmpty()
                select new DocumentoActivoReadDTO
                {
                    codigo = d.codigo,
                    codigoActivo = d.codigoActivo,
                    tipoDocumento = d.tipoDocumento,
                    referenciaDocumento = d.referenciaDocumento,
                    fechaRegistro = d.fechaRegistro,

                    archivoUrl = ext != null ? ext.ArchivoUrl : null,
                    nombreArchivo = ext != null ? ext.NombreArchivo : null,
                    tipoArchivo = ext != null ? ext.TipoArchivo : null,
                    tamañoArchivo = ext != null ? ext.TamañoArchivo : null,
                    fechaEmision = ext != null ? ext.FechaEmision : null
                }
            ).ToList();

            return Ok(resultado);
        }

        // GET: api/DocumentoActivos/porActivo/ACT-001
        [HttpGet("porActivo/{codigoActivo}")]
        public async Task<IActionResult> GetByActivo(string codigoActivo)
        {
            var documentosBD = await (
                from d in _context.DocumentoActivo
                join a in _context.Activo on d.activoId equals a.idActivo
                join t in _context.TipoDocumento on d.tipoDocumentoId equals t.idTipoDocumento
                where d.estado == "Activo"
                      && a.estado == "Activo"
                      && a.codigo == codigoActivo
                select new
                {
                    d.codigo,
                    codigoActivo = a.codigo,
                    tipoDocumento = t.nombre,
                    d.referenciaDocumento,
                    d.fechaRegistro
                }
            ).ToListAsync();

            var docsExternos = await _http.GetFromJsonAsync<List<DocumentoSolicitadoDTO>>(
                $"{Constantes.URL_DOCUMENTACION}api/DocumentoSolicitadoes/ListarTodos"
            );

            if (docsExternos == null)
                docsExternos = new List<DocumentoSolicitadoDTO>();

            var resultado = (
                from d in documentosBD
                join ext in docsExternos
                    on d.referenciaDocumento equals ext.CodigoDocSolicitado
                    into joinExt
                from ext in joinExt.DefaultIfEmpty()
                select new DocumentoActivoReadDTO
                {
                    codigo = d.codigo,
                    codigoActivo = d.codigoActivo,
                    tipoDocumento = d.tipoDocumento,
                    referenciaDocumento = d.referenciaDocumento,
                    fechaRegistro = d.fechaRegistro,

                    archivoUrl = ext != null ? ext.ArchivoUrl : null,
                    nombreArchivo = ext != null ? ext.NombreArchivo : null,
                    tipoArchivo = ext != null ? ext.TipoArchivo : null,
                    tamañoArchivo = ext != null ? ext.TamañoArchivo : null,
                    fechaEmision = ext != null ? ext.FechaEmision : null
                }
            ).ToList();

            return Ok(resultado);
        }

        // POST: api/DocumentoActivos/crear
        [HttpPost("crear")]
        public async Task<IActionResult> Crear(DocumentoActivoCreateDTO dto)
        {
            var activo = await (
                from a in _context.Activo
                where a.codigo == dto.codigoActivo && a.estado == "Activo"
                select a
            ).FirstOrDefaultAsync();

            var tipo = await (
                from t in _context.TipoDocumento
                where t.codigo == dto.codigoTipoDocumento && t.estado == "Activo"
                select t
            ).FirstOrDefaultAsync();

            if (activo == null || tipo == null)
                return BadRequest("Activo o TipoDocumento inválido");

            int? mantenimientoId = null;

            if (!string.IsNullOrWhiteSpace(dto.codigoMantenimiento))
            {
                var mantenimiento = await (
                    from m in _context.Mantenimiento
                    where m.codigo == dto.codigoMantenimiento && m.estado == "Activo"
                    select m
                ).FirstOrDefaultAsync();

                if (mantenimiento != null)
                    mantenimientoId = mantenimiento.idMantenimiento;
            }

            int ultimoId = await (
                from d in _context.DocumentoActivo
                orderby d.idDocumentoActivo descending
                select d.idDocumentoActivo
            ).FirstOrDefaultAsync();

            var doc = new DocumentoActivo()
            {
                codigo = $"DOC-{(ultimoId + 1):D3}",
                estado = "Activo",
                activoId = activo.idActivo,
                tipoDocumentoId = tipo.idTipoDocumento,
                referenciaDocumento = dto.referenciaDocumento,
                mantenimientoId = mantenimientoId,
                fechaRegistro = DateTime.UtcNow
            };

            _context.DocumentoActivo.Add(doc);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Documento registrado", codigoGenerado = doc.codigo });
        }

        // DELETE: api/DocumentoActivos/borrar/DOC-001
        [HttpDelete("borrar/{codigo}")]
        public async Task<IActionResult> SoftDelete(string codigo)
        {
            var doc = await (
                from d in _context.DocumentoActivo
                where d.codigo == codigo
                select d
            ).FirstOrDefaultAsync();

            if (doc == null)
                return NotFound("Documento no encontrado");

            doc.estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok("Documento desactivado");
        }
    }
}