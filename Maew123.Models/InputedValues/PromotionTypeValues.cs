using Maew123.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maew123.Models.InputedValues
{
    public static class PromotionTypeValues
    {

        public static List<Promotion> promotionTypes = GetPromotionType();
        private static List<Promotion> GetPromotionType()
        {
            var promotionTypes = new List<Promotion>
            {
                new Promotion
                {
                    PromotionName = "ลดราคาตามจำนวนกรอก",
                    PromotionType = "Fixed",
                    DiscountPer = 0
                },
                new Promotion
                {
                    PromotionName = "ลดราคาเป็นเปอร์เซ็น",
                    PromotionType = "Percent",
                    DiscountPer = 0
                },
                new Promotion
                {
                    PromotionName = "ซื้อครบกำหนด แถม+",
                    PromotionType = "OrderAmount"
                }
                //new Promotion
                //{
                //    PromotionName = "ส่งฟรี",
                //    PromotionType = "ส่งฟรี"
                //},
            };
            return promotionTypes;
        }
    }
}
