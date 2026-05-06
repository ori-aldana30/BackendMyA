using Microsoft.AspNetCore.Mvc;
using MyAMIS.Core.DTO;
using MyAMIS.Core.Mappers;
using MyAMIS.Soporte;

namespace MyAMIS.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasExternasController : ControllerBase
    {
        private readonly HttpClient _http;

        public AreasExternasController(HttpClient http)
        {
            _http = http;
        }

        // GET: api/AreasExternas/lista
        [HttpGet("lista")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await _http.GetFromJsonAsync<List<AreaDTO>>(
                $"{Constantes.URL_LOGISTICA}api/Departamentos"
            );

            if (areas == null)
                return BadRequest("No se pudo obtener áreas desde Logística");

            var resultado = areas.Select(a => AreaMapper.ToReadDTO(a)).ToList();
            return Ok(resultado);
        }

    }
}