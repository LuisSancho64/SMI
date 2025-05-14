using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class GrupoSanguineo
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<PersonaGrupoSanguineo> PersonaGrupoSanguineos { get; set; } = new List<PersonaGrupoSanguineo>();
}
