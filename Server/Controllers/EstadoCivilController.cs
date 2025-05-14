using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;

namespace SMI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoCivilController : ControllerBase
    {
        private readonly SGISDbContext _context;

        public EstadoCivilController(SGISDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadoCivilDto>>> GetAll()
        {
            try
            {
                var estados = await _context.EstadosCiviles
                    .Select(ec => new EstadoCivilDto
                    {
                        id = ec.id,
                        nombre = ec.nombre
                    })
                    .ToListAsync();

                return Ok(estados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EstadoCivilController: {ex}");
                return StatusCode(500, "Error interno al obtener estados civiles");
            }
        }
    }
}
