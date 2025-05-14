using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int? Id_Persona { get; set; }

    public string? Clave { get; set; }

    public bool? Activo { get; set; }

    public virtual Persona? Id_PersonaNavigation { get; set; }
}
