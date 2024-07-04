using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Province
{
    public int Pcode { get; set; }

    public string? Pname { get; set; }

    public int? TypeSoilder { get; set; }

    [JsonIgnore]
    public virtual ICollection<Amphoe> Amphoes { get; set; } = new List<Amphoe>();

    [JsonIgnore]
    public virtual ICollection<Tambol> Tambols { get; set; } = new List<Tambol>();
}
