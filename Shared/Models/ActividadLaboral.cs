using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class ActividadLaboral
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaActividadLaboral> PersonaActividadLaborals { get; set; } = new List<PersonaActividadLaboral>();
}
