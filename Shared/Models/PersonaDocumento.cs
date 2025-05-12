using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SMI.Shared.Models
{
    public class PersonaDocumento
    {
        public int id_Persona { get; set; }

        public int id_TipoDocumento { get; set; }

        public string numeroDocumento { get; set; }

        // Propiedades de navegación
        public Persona Persona { get; set; }

        public TipoDocumento TipoDocumento { get; set; }
    }
}