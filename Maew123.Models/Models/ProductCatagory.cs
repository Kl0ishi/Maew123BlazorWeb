using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class ProductCatagory
{
    public int ProductCatagoryId { get; set; }

    public string? ProductCatagoryName { get; set; }

    public string? Url { get; set; }

    public bool? Visible { get; set; }

    public bool? Deleted { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
