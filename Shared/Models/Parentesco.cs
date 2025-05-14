using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Parentesco
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PacienteParentesco> PacienteParentescos { get; set; } = new List<PacienteParentesco>();
}
