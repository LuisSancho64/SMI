using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaReligion
{
    public int id_Persona { get; set; }

    public int? id_Religion { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;

    public virtual Religion? id_ReligionNavigation { get; set; }
}
