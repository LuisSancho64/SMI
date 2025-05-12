using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public bool Activo { get; set; }
        public string Clave { get; set; }
        public PersonaDto Persona { get; set; }
    }

}
