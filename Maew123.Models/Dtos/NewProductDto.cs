using Maew123.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class NewProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductStatus { get; set; } = null!;

        public int ProductCatagoryId { get; set; }
        public string ProductCatagoryName {  get; set; } = string.Empty;

        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;

        public int stockId { get; set; }
        public int? numStock { get; set; }

        public int? PromotionId { get; set; }
        public string? PromotionName { get; set; }
        public string? PromotionType {  get; set; }
        public int? Discount { get; set; }
        public int? thresholdAmount { get; set; }
        public int? orderAmountDiscount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public decimal? ProPrice { get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? ImgPath { get; set; } = string.Empty;

        public int PurchasedCount {  get; set; }



        public bool Featured { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        //ใช้ if ตรวจเอาทีหลัง
        [NotMapped]
        public bool IsNew { get; set; } = false;
        [NotMapped]
        public string? Base64ImageData { get; set; }

        [NotMapped]
        public bool? IsFound { get; set; }

    }
}
