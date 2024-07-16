using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAoniken.Models;
using TestAoniken.Servicios;

namespace TestAoniken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IPublicacionService _publicacionService;

        // Constructor que recibe una instancia de IPublicacionService
        public PublicacionController(IPublicacionService publicacionService)
        {
            _publicacionService = publicacionService;
        }

        // Endpoint para obtener publicaciones pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            // Llama al método del servicio para obtener publicaciones pendientes
            var publicacionesPendientes = await _publicacionService.ObtenerPublicacionesPendientesAsync();

            if (publicacionesPendientes == null || publicacionesPendientes.Count == 0)
            {
                return NotFound("No se encontraron publicaciones pendientes.");
            }

            return Ok(publicacionesPendientes); // Retorna las publicaciones pendientes
        }

        // Endpoint para aprobar una publicación
        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> AprobarPublicacion(int id)
        {
            var resultado = await _publicacionService.AprobarPublicacionAsync(id);
            if (resultado)
            {
                return Ok(true);
            }
            return NotFound();
        }

        // Endpoint para rechazar una publicación (Eliminar)
        [HttpDelete("rechazar/{id}")]
        public async Task<IActionResult> RechazarPublicacion(int id)
        {
            var resultado = await _publicacionService.RechazarPublicacionAsync(id);
            if (resultado)
            {
                return Ok(true);
            }
            return NotFound();
        }

        // Endpoint para actualizar una publicación
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            // Llama al método del servicio para actualizar una publicación con el ID
            var result = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
            if (!result)
            {
                return NotFound(); // 404
            }
            return Ok(publicacionActualizada); // Retorna la publicación actualizada
        }

        // Endpoint para eliminar una publicación
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            // Llama al método del servicio (Eliminar) para eliminar una publicación con el ID dado
            var result = await _publicacionService.EliminarPublicacionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
