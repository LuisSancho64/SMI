using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaEstadoCivil
{
    public int id_Persona { get; set; }

    public int? id_EstadoCivil { get; set; }

    public  EstadoCivil? EstadoCivil { get; set; }

    public  Persona Persona { get; set; } = null!;
}
