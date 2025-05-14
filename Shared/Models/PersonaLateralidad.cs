using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaLateralidad
{
    public int id_Persona { get; set; }

    public int? id_Lateralidad { get; set; }

    public virtual Lateralidad? id_LateralidadNavigation { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
