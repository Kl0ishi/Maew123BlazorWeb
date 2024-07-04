using System;
using System.Collections.Generic;

namespace Maew123.Models.Models;

public partial class Token
{
    public int TokenId { get; set; }

    public string? Tokenkey { get; set; }

    public bool? Status { get; set; }

    public int? UserId { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Exp { get; set; }

    public virtual User? User { get; set; }
}
