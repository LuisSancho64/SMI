using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class TipoProfesionalSalud
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<ProfesionalSalud> ProfesionalSaluds { get; set; } = new List<ProfesionalSalud>();
}
