

namespace Maew123.Api.Repositories.Contracts
{
    public interface IPromotionRepository
    {
        Task<Promotion> GetPromotion(int id);
        Task<PromotionDto> GetPromotionDto(int id);
        Task<List<PromotionDto>> GetPromotions();

        Task<List<PromotionDto>> GetPromotionsAdmin();
        
        Task<PromotionDto> CreatePromotion(Promotion promotion);
        Task<PromotionDto> UpdatePromotion(Promotion promotion);
        Task<bool> DisablePromotion(int id, string updateBy);

        List<PromotionDto> ToPromotionDtosList(List<Promotion> promotions);

    }
}
