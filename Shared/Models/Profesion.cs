using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Profesion
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaProfesion> PersonaProfesions { get; set; } = new List<PersonaProfesion>();
}
