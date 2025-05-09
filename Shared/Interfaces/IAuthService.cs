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
        Task<bool> Login(LoginDTO loginDto);
    }
}
