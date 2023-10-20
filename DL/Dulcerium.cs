using System;
using System.Collections.Generic;

namespace DL;

public partial class Dulcerium
{
    public int IdDulce { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Imagen { get; set; }
}
