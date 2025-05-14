using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaSeguroMedico
{
    public int id { get; set; }

    public int? id_Persona { get; set; }

    public int? id_SeguroMedico { get; set; }

    public virtual Persona? id_PersonaNavigation { get; set; }

    public virtual SeguroMedico? id_SeguroMedicoNavigation { get; set; }
}
