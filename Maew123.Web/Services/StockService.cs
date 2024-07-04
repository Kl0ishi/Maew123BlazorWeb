using Maew123.Models;
using Maew123.Web.Services.Contracts;

namespace Maew123.Web.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _http;

        public StockService(HttpClient http)
        {
            this._http = http;
        }
        public async Task<ServiceResponse<List<ProductType>>> GetStock()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>($"api/ProductType/GetProductTypes");
            return result;
        }
    }
}
