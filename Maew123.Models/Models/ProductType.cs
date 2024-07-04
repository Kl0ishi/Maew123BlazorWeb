using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string? ProductTypeName { get; set; }

    public bool ProductTypeStatus { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int ProductCategoryId { get; set; }

    public virtual ProductCatagory ProductCategory { get; set; } = new ProductCatagory();

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
