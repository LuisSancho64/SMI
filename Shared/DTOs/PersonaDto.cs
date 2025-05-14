using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMI.Shared.DTOs
{
    public class PersonaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Id_Genero { get; set; }

        public string? GeneroNombre { get; set; }
        public string Correo { get; set; }

        public int Edad => FechaNacimiento.HasValue ? DateTime.Today.Year - FechaNacimiento.Value.Year : 0;
        public DateTime? FechaNacimiento { get; set; }

        // Nueva propiedad para documentos asociados
        public List<PersonaDocumentoDto> Documentos { get; set; } = new List<PersonaDocumentoDto>();
        public List<CiudadDto> CiudadesResidencia { get; set; } = new List<CiudadDto>();

        public List<int> CiudadesSeleccionadas { get; set; } = new List<int>();

        public int IdEstadoCivil { get; set; }
        public string? EstadoCivilNombre { get; set; }
    }
}

