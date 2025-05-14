using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class ContactoEmergencium
{
    public int id { get; set; }

    public int id_Paciente { get; set; }

    public string? nombre { get; set; }

    public string? telefono { get; set; }

    public string? correo { get; set; }

    public virtual Paciente id_PacienteNavigation { get; set; } = null!;
}
