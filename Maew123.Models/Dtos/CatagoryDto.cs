using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class CatagoryDto
    {
        public int ProductCatagoryId { get; set; }

        public string? ProductCatagoryName { get; set; }

        public string? Url { get; set; }

        public bool? Visible { get; set; }

        public bool? Deleted { get; set; }

        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    }
}
