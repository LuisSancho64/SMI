using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaTelefono
{
    public int id_Persona { get; set; }

    public string? celular { get; set; }

    public string? convencional { get; set; }

    public virtual Persona id_PersonaNavigation { get; set; } = null!;
}
