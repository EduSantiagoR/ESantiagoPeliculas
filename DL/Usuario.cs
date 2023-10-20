using System;
using System.Collections.Generic;
namespace DL;

public partial class Usuario
{
    public string Email { get; set; } = null!;

    public byte[] Password { get; set; } = null!;
}
