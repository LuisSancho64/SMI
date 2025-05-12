using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class UsuarioUpdateDto
    {
        public string Clave { get; set; } // Opcional, solo si se quiere cambiar
        public bool Activo { get; set; }
    }
}
