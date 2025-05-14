using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PacienteParentesco
{
    public int id_Paciente { get; set; }

    public int? id_Parentesco { get; set; }

    public virtual Paciente id_PacienteNavigation { get; set; } = null!;

    public virtual Parentesco? id_ParentescoNavigation { get; set; }
}
