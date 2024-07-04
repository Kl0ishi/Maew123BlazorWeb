using Maew123.Models;
using Maew123.Web.Services.Contracts;
using static System.Net.WebRequestMethods;

namespace Maew123.Web.Services
{
    public class CatagoryService : ICatagoryService
    {
        private readonly HttpClient _http;

        public CatagoryService(HttpClient http)
        {
            this._http = http;
        }

        public List<ProductCatagory> Catagories { get; set; } = new List<ProductCatagory>();

        public async Task GetCatagories()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductCatagory>>>($"api/ProductCatagory/GetCatagories");
            if (result != null && result.Data != null)
            {
                Catagories = result.Data;
            }
        }
        public async Task<ServiceResponse<ProductCatagory>> GetCatagory (int catagoryid)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductCatagory>>($"api/ProductCatagory/GetCatagory/{catagoryid}");
            return result;
        }
        public async Task<ProductCatagory> CreateCatagory(ProductCatagory catagory)
        {
            var result = await _http.PostAsJsonAsync("api/ProductCatagory/CreateCatagory", catagory);
            var newCatagory = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<ProductCatagory>>())!.Data;
            return newCatagory!;
        }

        public async Task<ServiceResponse<ProductCatagory>> UpdateCatagory(ProductCatagory catagory)
        {
            var result = await _http.PutAsJsonAsync($"api/ProductCatagory/UpdateCatagory", catagory);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductCatagory>>();
            return content;
        }
        public async Task DeleteCatagory(int catagoryid)
        {
            var result = await _http.DeleteAsync($"api/ProductCatagory/DeleteCatagory?id={catagoryid}");
        }
    }
}
