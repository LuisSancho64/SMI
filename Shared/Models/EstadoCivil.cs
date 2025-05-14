using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class EstadoCivil
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaEstadoCivil> PersonaEstadoCivils { get; set; } = new List<PersonaEstadoCivil>();
}
