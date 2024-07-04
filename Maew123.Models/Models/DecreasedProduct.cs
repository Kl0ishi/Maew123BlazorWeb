using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maew123.Models.Models
{
    public class DecreasedProduct
    {
        public int DecreasedProductId { get; set; }

        public int? ProductId { get; set; }

        public DateTime? DecreaseDate { get; set; }

        public int? DecreaseQuantity { get; set; }

        public string? DecreaseReason { get; set; }

        public string? DecreaseBy { get; set; }

        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
