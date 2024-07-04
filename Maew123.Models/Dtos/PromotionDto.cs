using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.Dtos
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }

        public string? PromotionName { get; set; }

        public string? PromotionType { get; set; }

        public int? DiscountPer { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string? UpdateDate {  get; set; }
        public DateTime? UpdateBy { get; set; }

        public int expiredInDay { get; set; } = 0;
    }
}
