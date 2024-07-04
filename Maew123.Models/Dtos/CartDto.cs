using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class CartDto
    {
        public int SaleId { get; set; }
        public int UserId { get; set; }
        public string? SaleCode {  get; set; }
        public int? SaleNum { get; set; }
        public int? SaleDiscount { get; set; }
        public decimal? SaleTotal { get; set; }
        public int? AddressId {  get; set; }
        public List<ItemQuantityDto> Quans { get; set; } = new List<ItemQuantityDto>();
    }
}
