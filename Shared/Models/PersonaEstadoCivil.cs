using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaEstadoCivil
{
    public int id_Persona { get; set; }

    public int? id_EstadoCivil { get; set; }

    public virtual EstadoCivil? id_EstadoCivilNavigation { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
