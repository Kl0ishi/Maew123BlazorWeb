using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface IPromotionService
    {
        List<PromotionDto> PromotionsAdmin { get; set; }
        Task GetPromotions();
        Task GetPromotionsAdmin();
        Task<ServiceResponse<PromotionDto>> GetPromotion(int promotionid);

        Task<ServiceResponse<PromotionDto>> UpdatePromotion(PromotionDto promotion);

        Task<PromotionDto> CreatePromotion(PromotionDto promotion);
        Task DeletePromotion(int promotionid);
    }
}
