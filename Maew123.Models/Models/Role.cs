using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Status { get; set; }

    public string InsertBy { get; set; } = null!;

    public DateTime InsertDate { get; set; }

    public string UpdateBy { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
