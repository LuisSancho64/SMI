using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaProfesion
{
    public int id_Persona { get; set; }

    public int? id_Profesion { get; set; }

    public virtual Persona? Persona { get; set; }

    public virtual Profesion? id_ProfesionNavigation { get; set; }
}
