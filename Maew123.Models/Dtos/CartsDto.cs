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
    public class CartsDto
    {
        public int SaleId { get; set; }
        public string? SaleCode { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? UserTel { get; set; }
        public int? SaleNum { get; set; }
        public int SaleDiscount { get; set; }
        public decimal? SaleTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusName {  get; set; }
        public int? AddressId {  get; set; }
        public string? ImgPath { get; set; } = string.Empty;
        //[Display(Name = "Choose Image")]
        //public IFormFile? ImagePath { get; set; }
        [NotMapped]
        public string? Base64ImageData { get; set; }
        public DateTime? PayDate {  get; set; }

        public string? Annotation { get; set; }

        public string? ParcelTypeNo { get; set; }
        public string? ParcelNumber { get; set; }
        public DateTime? SentDate { get; set; }

        public List<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();

        public string FormattedOrderDate => OrderDate.ToString("dd/MM/yyyy HH:mm");
        public string? FormattedPayDate => (PayDate??DateTime.Now).ToString("dd/MM/yyyy HH:mm");

        // Method to calculate aggregated values
        //public void CalculateAggregatedValues()
        //{
        //    decimal? sumSale = 0;
        //    int sumDiscount = 0;
        //    decimal? totalPrice = 0;
        //    decimal? totalVat = 0;
        //    decimal? totalPriceWithoutVat = 0;

        //    foreach (var cartDetail in CartDetails)
        //    {
        //        var discountedPrice = cartDetail.TotalPrice / cartDetail.Quantity;
        //        var discount = cartDetail.Discount ?? 0;
        //        var unitPrice = discountedPrice + discount;
        //        var sumPrice = (unitPrice * cartDetail.Quantity) + (cartDetail.Proprice ?? 0);
        //        var vatPrice = (cartDetail.TotalPrice * 7) / 100;
        //        var sumWithoutVat = cartDetail.TotalPrice - vatPrice;

        //        sumSale += sumPrice;
        //        sumDiscount += (discount * cartDetail.Quantity);
        //        totalPrice += cartDetail.TotalPrice;
        //        totalVat += vatPrice;
        //        totalPriceWithoutVat += sumWithoutVat;
        //    }

        //    SumSale = sumSale;
        //    SumDiscount = sumDiscount;
        //    TotalPrice = totalPrice;
        //    TotalVat = totalVat;
        //    TotalPriceWithoutVat = totalPriceWithoutVat;
        //}

        //// Additional properties to store aggregated values
        //public decimal? SumSale { get; private set; }
        //public int SumDiscount { get; private set; }
        //public decimal? TotalPrice { get; private set; }
        //public decimal? TotalVat { get; private set; }
        //public decimal? TotalPriceWithoutVat { get; private set; }
    }
}
