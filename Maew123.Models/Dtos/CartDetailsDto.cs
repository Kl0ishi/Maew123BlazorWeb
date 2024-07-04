using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class CartDetailsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        //public int ProductCatagoryId { get; set; }
        //public string CatagoryName { get; set; } = string.Empty;

        //public string ProductType { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? Proprice { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }

        public int? PromotionId { get; set; }
        public string? PromotionName { get; set; } = string.Empty;
        public string? PromotionType { get; set; } = string.Empty;
        public int Discount { get; set; }
        public int? thresholdAmount { get; set; }
        public int? orderAmountDiscount { get; set; }

        // Calculated properties
        public decimal? DiscountedPrice //ราคาต่อชิ้น(รวมลดราคา)
        {
            get
            {
                if (Quantity != 0)
                {
                    return (TotalPrice ?? 0) / Quantity;
                }
                else
                {
                    return null; // Or handle as desired when Discount or Quantity is zero
                }
            }
        }

        public decimal? UnitPrice //ราคาต่อชิ้น(ไม่รวมส่วนลด)
        {
            get
            {
                if (Quantity != 0)
                {
                    return DiscountedPrice + Discount;
                }
                else
                {
                    return null; // Or handle as desired when Discount or Quantity is zero
                }
            }
        }

        public decimal? SumPrice //ราคารวม(ไม่รวมส่วนลด)  note:ไม่รู้Propriceเอามาทำชิทไร ไม่ชัว อาจใช้คำนวณตอนตะกร้า
        {
            get
            {
                if (Quantity != 0)
                {
                    return (UnitPrice * Quantity) + (Proprice ?? 0);
                }
                else
                {
                    return null; // Or handle as desired when Discount or Quantity is zero
                }
            }
        }

        public decimal? VatPrice
        {
            get
            {
                if (Quantity != 0)
                {
                    return (TotalPrice ?? 0) * 0.07m;
                }
                else
                {
                    return null; // Or handle as desired when Discount or Quantity is zero
                }
            }
        }

        public decimal? SumWithoutVat
        {
            get
            {
                if (Quantity != 0)
                {
                    return (TotalPrice ?? 0) - VatPrice;
                }
                else
                {
                    return null; // Or handle as desired when Discount or Quantity is zero
                }
            }
        }
    }
}
