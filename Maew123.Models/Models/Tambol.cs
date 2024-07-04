using System;
using System.Collections.Generic;

namespace Maew123.Models.Models;

public partial class Tambol
{
    public int Tcode { get; set; }

    public string? Tname { get; set; }

    public int? Acode { get; set; }

    public string? Aname { get; set; }

    public int? Pcode { get; set; }

    public string? Pname { get; set; }

    public virtual Amphoe? AcodeNavigation { get; set; }

    public virtual Province? PcodeNavigation { get; set; }
}
