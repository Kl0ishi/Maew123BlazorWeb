using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string? PromotionName { get; set; }

    public string? PromotionType { get; set; }

    public int? DiscountPer { get; set; }

    public int? thresholdAmount {  get; set; }

    public int? orderAmountDiscount {  get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? InsertBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [JsonIgnore]
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
