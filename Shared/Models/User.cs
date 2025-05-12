using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
    
        public int Id_Persona { get; set; } // Relación con la tabla Persona
        public string Clave { get; set; }
        public bool Activo { get; set; }

        public Persona Persona { get; set; }
    }
}

