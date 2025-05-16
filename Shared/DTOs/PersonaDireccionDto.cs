using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class PersonaDireccionDto
    {
        public int IdPersona { get; set; }
        public string? CallePrincipal { get; set; }
        public string? CalleSecundaria1 { get; set; }
        public string? CalleSecundaria2 { get; set; }
        public string? NumeroCasa { get; set; }
        public string? Referencia { get; set; }
    }
}
