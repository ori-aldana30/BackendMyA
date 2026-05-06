using Microsoft.AspNetCore.Mvc;
using MyAMIS.Core.DTO;
using System.Text.Json;

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

            try
            {
                var result = await _http.GetFromJsonAsync<DocumentoSolicitadoResponseDTO>(url);

                if (result == null || result.Data == null)
                    return BadRequest("No se pudo obtener documentos");

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }
    }
}