using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Lateralidad
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaLateralidad> PersonaLateralidads { get; set; } = new List<PersonaLateralidad>();
}
