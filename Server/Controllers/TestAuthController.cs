using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SMI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthController : ControllerBase
    {
        // Endpoint protegido por token JWT
        [HttpGet("protegido")]
        [Authorize]
        public IActionResult GetProtegido()
        {
            var nombre = User.Identity?.Name;
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst("email")?.Value;

            return Ok(new
            {
                Mensaje = "🔐 Acceso protegido concedido",
                Usuario = nombre,
                Id = userId,
                Correo = email
            });
        }

        // Endpoint público (sin token)
        [HttpGet("publico")]
        public IActionResult GetPublico()
        {
            return Ok(new { Mensaje = "🌐 Este endpoint es público, sin token." });
        }
    }
}
