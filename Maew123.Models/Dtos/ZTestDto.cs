using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class ZTestDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductStatus { get; set; } = null!;

        public int ProductCatagoryId { get; set; }

        public int? PromotionId { get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? ImgPath { get; set; } = string.Empty;

        public ProductCatagory ProductCatagory { get; set; } = null!;

        public ProductType ProductType { get; set; } = null!;

        public Promotion? Promotion { get; set; }

        //public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();


        public bool Featured { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;

        [NotMapped]
        public bool IsFound { get; set; }
    }
}
