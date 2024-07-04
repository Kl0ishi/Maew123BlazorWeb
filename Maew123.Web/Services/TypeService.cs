using Maew123.Models;
using Maew123.Models.Models;
using Maew123.Web.Services.Contracts;
using static System.Net.WebRequestMethods;

namespace Maew123.Web.Services
{
    public class TypeService : ITypeService
    {
        private readonly HttpClient _http;

        public TypeService(HttpClient http)
        {
            this._http = http;
        }
        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public async Task getProductTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>($"api/ProductType/GetProductTypes");
            if (result != null && result.Data != null)
            {
                ProductTypes = result.Data;
            }
        }
        public async Task<ServiceResponse<ProductType>> GetProductType(int producttypeid)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductType>>($"api/ProductType/GetProductType/{producttypeid}");
            return result;
        }

        public async Task<ProductType> CreateProductType(ProductType producttype)
        {
            try
            {
                var result = await _http.PostAsJsonAsync("api/ProductType/CreateProductType", producttype);
                var newType = (await result.Content
                    .ReadFromJsonAsync<ServiceResponse<ProductType>>())!.Data;
                return newType!;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ServiceResponse<ProductType>> UpdateProductType(ProductType producttype)
        {
            var result = await _http.PutAsJsonAsync($"api/ProductType/UpdateProductType", producttype);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductType>>();
            return content;
        }
        public async Task DeleteProductType(int producttypeid)
        {
            var result = await _http.DeleteAsync($"api/ProductType/DeleteProductType?id={producttypeid}");
        }
    }
}
