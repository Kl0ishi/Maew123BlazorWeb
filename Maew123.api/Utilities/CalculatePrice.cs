namespace Maew123.Api.Utilities
{
    public static class CalculatePrice
    {
        public static decimal? CalPromoPrice(decimal? OriginalPrice, string? PromotionType, int? DiscountPer, int? thresholdAmount, int? orderAmountDiscount, int quantity)
        {
            if (PromotionType == "Fixed")
            {
                decimal discount = Convert.ToDecimal(DiscountPer);
                return OriginalPrice - discount;
            }
            else if (PromotionType == "Percent")
            {
                decimal discount = Convert.ToDecimal(DiscountPer);
                return (OriginalPrice * discount) / 100;
            }
            else if (PromotionType == "OrderAmount")
            {
                // Check if the order quantity meets the threshold amount
                if (quantity >= thresholdAmount)
                {
                    decimal discount = Convert.ToDecimal(orderAmountDiscount!.Value);
                    // Calculate the total discount based on the order quantity
                    decimal totalDiscount = discount * (quantity / thresholdAmount.Value);
                    return OriginalPrice - totalDiscount;
                }
            }
            return 0;

        }
    }
}
