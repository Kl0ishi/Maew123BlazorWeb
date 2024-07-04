using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class SaleReport
    {
        public string? SaleCode { get; set; }
        public int? SaleNum { get; set; }
        public int? SaleDiscount { get; set; }
        public decimal? SaleTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string? StatusName { get; set; }
        public string? ParcelNumber { get; set; }
        public DateTime? SentDate { get; set; }
        //public List<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
    }
}
