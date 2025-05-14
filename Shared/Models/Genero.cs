using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Genero
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
