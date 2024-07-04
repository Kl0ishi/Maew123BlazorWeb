using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class DecreasedProductsDto
    {
        public int DecreasedProductId { get; set; }

        public int? ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? Condition { get; set; }

        public string? ImgPath { get; set; } = string.Empty;

        public DateTime? DecreaseDate { get; set; }

        public int? DecreaseQuantity { get; set; }

        public string? DecreaseReason { get; set; }

        public string? DecreaseBy { get; set; }

    }
}
