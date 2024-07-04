using Maew123.Models;
using Maew123.Models.Models;
using Maew123.Web.Services.Contracts;

namespace Maew123.Web.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _http;

        public PromotionService(HttpClient http)
        {
            this._http = http;
        }

        public List<PromotionDto> PromotionsAdmin { get; set; } = new List<PromotionDto>();

        public async Task GetPromotions()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<List<PromotionDto>>>($"api/Promotion/GetPromotionsAdmin"); //แสดงอันที่ end date ไปแล้ว
                if (result != null && result.Data != null)
                {
                    PromotionsAdmin = result.Data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task GetPromotionsAdmin()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<List<PromotionDto>>>($"api/Promotion/GetPromotions"); //ไม่แสดงอันที่ enddate
                if (result != null && result.Data != null)
                {
                    PromotionsAdmin = result.Data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ServiceResponse<PromotionDto>> GetPromotion(int promotionid)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<PromotionDto>>($"api/Promotion/GetPromotion/{promotionid}");
            return result;
        }

        public async Task<PromotionDto> CreatePromotion(PromotionDto promotion)
        {
            var result = await _http.PostAsJsonAsync("api/Promotion/CreatePromotion", promotion);
            var newPromotion = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<PromotionDto>>())!.Data;
            return newPromotion!;
        }

        public async Task<ServiceResponse<PromotionDto>> UpdatePromotion(PromotionDto promotion)
        {
            var result = await _http.PutAsJsonAsync($"api/Promotion/UpdatePromotion", promotion);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<PromotionDto>>();
            return content;
        }
        public async Task DeletePromotion(int promotionid)
        {
            var result = await _http.DeleteAsync($"api/Promotion/DeletePromotion?id={promotionid}");
        }
    }
}
