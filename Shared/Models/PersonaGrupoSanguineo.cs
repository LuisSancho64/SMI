using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaGrupoSanguineo
{
    public int id_Persona { get; set; }

    public int? id_GrupoSanguineo { get; set; }

    public virtual GrupoSanguineo? id_GrupoSanguineoNavigation { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
