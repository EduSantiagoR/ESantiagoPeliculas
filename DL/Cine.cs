using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace DL;

public partial class Cine
{
    public int IdCine { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int IdZona { get; set; }

    public decimal Ventas { get; set; }

    public virtual Zona IdZonaNavigation { get; set; } = null!;

    public string NombreZona { get; set; }
}
