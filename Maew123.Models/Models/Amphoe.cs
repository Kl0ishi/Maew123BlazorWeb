using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Amphoe
{
    public int Acode { get; set; }

    public string? Aname { get; set; }

    public int? Pcode { get; set; }

    public string? Pname { get; set; }

    public virtual Province? PcodeNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Tambol> Tambols { get; set; } = new List<Tambol>();
}
