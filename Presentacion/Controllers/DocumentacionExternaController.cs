using Microsoft.AspNetCore.Mvc;
using MyAMIS.Core.DTO;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentacionExternaController : ControllerBase
    {
        private readonly HttpClient _http;

        public DocumentacionExternaController(HttpClient http)
        {
            _http = http;
        }

        [HttpGet("documentos")]
        public async Task<IActionResult> ListarDocumentos()
        {
            var url = "https://gestiondocumental-1.onrender.com/api/DocumentoSolicitadoes/ListarTodos";

            var docs = await _http.GetFromJsonAsync<List<DocumentoSolicitadoDTO>>(url);

            if (docs == null)
                return BadRequest("No se pudo obtener documentos");

            return Ok(docs);
        }
    }
}