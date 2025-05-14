using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class SeguroMedico
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaSeguroMedico> PersonaSeguroMedicos { get; set; } = new List<PersonaSeguroMedico>();
}
