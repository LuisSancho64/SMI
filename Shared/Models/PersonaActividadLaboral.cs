using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaActividadLaboral
{
    public int id_Persona { get; set; }

    public int? id_ActividadLaboral { get; set; }

    public virtual ActividadLaboral? id_ActividadLaboralNavigation { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
