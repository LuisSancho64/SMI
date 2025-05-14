using System;
using System.Collections.Generic;

namespace SMI.Shared.Models;

public partial class Provincia
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<Ciudad> Ciudades { get; set; }
}
