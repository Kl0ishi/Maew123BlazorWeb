using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Maew123.Models.Models
{
    public partial class AddressSaleSnapshot
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? AddressName { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }
}
