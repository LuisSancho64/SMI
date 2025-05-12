using SMI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginDTO loginDto);
        Task Logout();
        Task<UsuarioDto> GetCurrentUser();
        Task<string> GetToken();
        Task<bool> IsAuthenticated();
    }

}
