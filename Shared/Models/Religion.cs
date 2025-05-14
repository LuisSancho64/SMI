using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Religion
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaReligion> PersonaReligions { get; set; } = new List<PersonaReligion>();
}
