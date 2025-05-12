using SMI.Shared.DTOs;

namespace SMI.Server.Services
{
    public interface IJwtService
    {
        string GenerateToken(UsuarioDto usuario);
    }
}