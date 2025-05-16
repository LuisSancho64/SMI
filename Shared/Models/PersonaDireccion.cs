using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class PersonaDireccion
{
    public int id_Persona { get; set; }

    public string? callePrincipal { get; set; }

    public string? calleSecundaria1 { get; set; }

    public string? calleSecundaria2 { get; set; }

    public string? numeroCasa { get; set; }

    public string? referencia { get; set; }

    public virtual Persona Persona { get; set; }
}
