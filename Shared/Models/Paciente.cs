using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Paciente
{
    public int id_Persona { get; set; }

    public virtual ICollection<ContactoEmergencium> ContactoEmergencia { get; set; } = new List<ContactoEmergencium>();

    public virtual PacienteParentesco? PacienteParentesco { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
