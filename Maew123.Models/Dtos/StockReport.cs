using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class StockReport
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductStatus { get; set; } = null!;
        public string ProductCatagoryName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public int? numStock { get; set; }
        public string PromotionName { get; set; } = string.Empty;
        public int? Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? Condition { get; set; }
        //public string? Description { get; set; } = string.Empty;
        public string? InsertBy { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
