using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMI.Shared.Models
{
    public class TipoDocumento
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public ICollection<PersonaDocumento> PersonaDocumentos { get; set; }
    }

}