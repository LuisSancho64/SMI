using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class UsuarioCreateDto
    {
        public int Id_Persona { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; } = true;
    }
}
