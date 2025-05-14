using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class PersonaLugarResidenciaDto
    {
        public int IdPersona { get; set; }
        public int IdCiudad { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreProvincia { get; set; }
    }
}
