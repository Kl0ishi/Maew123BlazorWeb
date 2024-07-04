using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maew123.Models.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public string? SaleCode { get; set; }

    public int UserId { get; set; }

    public int? SaleNum { get; set; }

    public int? SaleDiscount { get; set; }

    public decimal? SaleTotal { get; set; }

    public DateTime OrderDate { get; set; }

    public int StatusId { get; set; }

    public string? ImgPath { get; set; }

    public DateTime? PayDate { get; set; }

    public string? Annotation { get; set; }

    public string? ParcelTypeNo { get; set; }

    public string? ParcelNumber { get; set; }

    public DateTime? SentDate { get; set; }

    public int AddressSnapshotId { get; set; }

    [JsonIgnore]
    public virtual AddressSaleSnapshot AddressSnapshot { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual Status Status { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
