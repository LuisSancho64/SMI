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
            // Solo valida existencia de usuario sin seguimiento
            var userExists = await _context.Usuario
                .AsNoTracking()
                .Include(u => u.Persona)
                .AnyAsync(u => u.Persona.Correo == loginDto.Correo &&
                               u.Clave == loginDto.Clave &&
                               u.Activo);

            return userExists;
        }
    }
}
