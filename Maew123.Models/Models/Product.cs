using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string ProductStatus { get; set; } = null!;

    public int ProductCatagoryId { get; set; }

    public int ProductTypeId { get; set; }

    public int? PromotionId { get; set; }

    public decimal Price { get; set; }

    public string? Condition { get; set; }

    public string? Description { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? ImgPath { get; set; }

    public bool? Featured { get; set; }

    public bool? Deleted { get; set; }

    public bool? Visible { get; set; }

    public virtual ProductCatagory ProductCatagory { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ProductType ProductType { get; set; } = null!;

    [JsonIgnore]
    public virtual Promotion? Promotion { get; set; }

    [JsonIgnore]
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    [JsonIgnore]
    public virtual ICollection<WishListItem> WishListItems { get; set; } = new List<WishListItem>();
}
