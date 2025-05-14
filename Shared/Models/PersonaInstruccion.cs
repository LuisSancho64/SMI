using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaInstruccion
{
    public int id_Persona { get; set; }

    public int? id_NivelInstruccion { get; set; }

    public virtual NivelInstruccion? id_NivelInstruccionNavigation { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
