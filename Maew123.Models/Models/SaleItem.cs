using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maew123.Models.Models;

public partial class SaleItem
{
    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int? Seq { get; set; }

    public int? Qty { get; set; }

    public decimal? TotalPrice { get; set; }

    public int? PromotionId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;

    public Promotion? Promotion { get; set; }

}
