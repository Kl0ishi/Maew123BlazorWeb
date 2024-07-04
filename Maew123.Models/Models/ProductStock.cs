using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class ProductStock
{
    public int ProductStockId { get; set; }

    public int? NumStock { get; set; }

    public int ProductId { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}
