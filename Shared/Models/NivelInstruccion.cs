using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class NivelInstruccion
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaInstruccion> PersonaInstruccions { get; set; } = new List<PersonaInstruccion>();
}
