using Maew123.Api.Models;
using Maew123.Api.Utilities;

using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly ItshopMaew123Context _dbContext;
        private DateOnly DisableDate = DateOnly.FromDateTime(new DateTime(666, 1, 1, 1, 1, 1));
        public PromotionRepository(ItshopMaew123Context dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Promotion> GetPromotion(int id)
        {
            var promotion = await _dbContext.Promotions
                    .FirstOrDefaultAsync(p => p.PromotionId == id && 
                    //p.EndDate >= DateOnly.FromDateTime(DateTime.Now) && 
                    p.EndDate != DisableDate);

            return promotion!;
        }

        public async Task<PromotionDto> GetPromotionDto(int id)
        {
            var promotion = await _dbContext.Promotions
                    .FirstOrDefaultAsync(p => p.PromotionId == id &&
                    //p.EndDate >= DateOnly.FromDateTime(DateTime.Now) &&
                    p.EndDate != DisableDate);
            var promotionDto = promotion!.ToPromotionDto();
            return promotionDto;
        }

        public async Task<List<PromotionDto>> GetPromotions()
        {
            var promotions = await _dbContext.Promotions
                    .Where(p => p.EndDate >= DateOnly.FromDateTime(DateTime.Now))
                    .ToListAsync();
            var promotionsDto = ToPromotionDtosList(promotions!);
            return promotionsDto;
        }

        public async Task<List<PromotionDto>> GetPromotionsAdmin()
        {
            var promotions = await _dbContext.Promotions
                    .Where(p => p.EndDate != DisableDate)
                    .ToListAsync();
            var promotionsDto = ToPromotionDtosList(promotions!);
            return promotionsDto;
        }

        public async Task<PromotionDto> CreatePromotion(Promotion promotion)
        {
            promotion.InsertDate = DateTime.Now;
            promotion.UpdateDate = DateTime.Now;

            _dbContext.Promotions.Add(promotion);
            await _dbContext.SaveChangesAsync();
            var promotionDto = promotion.ToPromotionDto();
            return promotionDto;
        }

        public async Task<PromotionDto> UpdatePromotion(Promotion promotion)
        {
            promotion.UpdateDate = DateTime.Now;

            _dbContext.Promotions.Update(promotion);
            await _dbContext.SaveChangesAsync();
            var promotionDto = promotion.ToPromotionDto();
            return promotionDto;
        }

        public async Task<bool> DisablePromotion(int id, string updateBy)
        {
            var promotion = await GetPromotion(id);
            if(promotion != null)
            {
                promotion.UpdateBy = updateBy;
                promotion.UpdateDate = DateTime.Now;
                promotion.EndDate = DisableDate;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<PromotionDto> ToPromotionDtosList(List<Promotion> promotions)
        {
            var dtoList = new List<PromotionDto>();
            foreach (var promotion in promotions)
            {
                dtoList.Add(promotion.ToPromotionDto());
            }

            return dtoList;
        }

    }
}
