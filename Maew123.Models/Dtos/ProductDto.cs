using Maew123.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ProductStatus { get; set; }

        public int ProductCatagoryId { get; set; }

        public int ProductTypeId {  get; set; }
        
        public int? PromotionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {  get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? ImgPath { get; set; } = string.Empty;

        public ProductCatagory ProductCatagory { get; set; } = new ProductCatagory
        {
            ProductCatagoryId = 0
        };

        public ProductType ProductType { get; set; } = new ProductType
        {
            ProductTypeId = 0,
            ProductCategory = new ProductCatagory()
        };

        public Promotion? Promotion { get; set; }

        public int stockId {  get; set; }
        public int stockNum {  get; set; }


        public bool Featured { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
        //[NotMapped] 
        //[Display(Name = "Choose Image")]
        //public IFormFile? ImagePath { get; set; }
        [NotMapped]
        public string? Base64ImageData { get; set; }


        [NotMapped]
        public bool IsFound { get; set; }

        [NotMapped]
        public int? numStock { get; set; }
    }
}
