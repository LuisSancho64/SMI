using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

namespace SMI.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly SGISDbContext _context;

        public AuthService(SGISDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Login(LoginDTO loginDto)
        {
            // Buscamos el usuario con el correo y la contraseña proporcionados
            var user = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Correo == loginDto.Correo &&
                                          u.Clave == loginDto.Clave &&
                                          u.Activo); // Verifica si el usuario está activo

            // Si el usuario existe y está activo, devolvemos verdadero
            return user != null;
        }
    }
}
