using Maew123.Models.Models;
using Maew123.Models;
using Maew123.Web.Services.Contracts;
using System.Net.Http.Json;
using Maew123.Models.Dtos;
using System.Net.Http;
using CurrieTechnologies.Razor.SweetAlert2;
using System;

namespace Maew123.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<NewProductDto> Products { get; set; } = new List<NewProductDto>();
        public List<NewProductDto> BestSellerProducts { get; set; } = new List<NewProductDto>();

        public string Message { get; set; } = "Loading products...";
        //public int CurrentPage { get; set; } = 1;
        //public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;
        public List<ProductDto> AdminProducts { get; set; }

        public event Action ProductsChanged;

        public async Task<ProductSearchResultDto> LoadAdminProductIndex(ProductSearchParam param)
        {
            var result = await _http.GetAsync($"api/Product/Home?" +
                                                  $"Currentpage={param.Currentpage}" +
                                                  $"&searchText={param.searchText}" +
                                                  $"&filterCata={param.filterCata}" +
                                                  $"&filterType={param.filterType}" +
                                                  $"&minPrice={param.minPrice}" +
                                                  $"&maxPrice={param.maxPrice}");

            result.EnsureSuccessStatusCode();

            var ProductSearch = (await result.Content.ReadFromJsonAsync<ServiceResponse<ProductSearchResultDto>>())!.Data;
            return ProductSearch!;
        }

        public async Task<ProductDto> CreateProduct(ProductDto product)
        {
            var result = await _http.PostAsJsonAsync("api/product/CreateProduct", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<ProductDto>>())!.Data;
            return newProduct!;
        }

        public async Task DeleteProduct(int productId)
        {
            var result = await _http.DeleteAsync($"api/product/DeleteProduct/{productId}");
        }

        public async Task GetAdminProducts()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<ProductDto>>>("api/product/admin");
            AdminProducts = result.Data;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<ServiceResponse<NewProductDto>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<NewProductDto>>($"api/product/GetProduct/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<NewProductDto>>>("api/product/GetFeaturedProducts") :
                await _http.GetFromJsonAsync<ServiceResponse<List<NewProductDto>>>($"api/product/category/{categoryUrl}");

            if (result != null && result.Data != null)
                Products = result.Data;

            //CurrentPage = 1;
            //PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found";

            ProductsChanged.Invoke();
        }

        public async Task GetBestSellerProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<NewProductDto>>>($"api/product/GetBestSellerProducts");

            if (result != null && result.Data != null)
                BestSellerProducts = result.Data;

            if (BestSellerProducts.Count == 0)
                Message = "No products found";

            ProductsChanged.Invoke();
        }

        public async Task GetProductsByTypeUnderCate(int typeId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<NewProductDto>>>($"api/product/type/{typeId}");

            if (result != null && result.Data != null)
                Products = result.Data;

            if (Products.Count == 0)
                Message = "No products found";

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/GetProductSearchSuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText)
        {
            LastSearchText = searchText;
            var param = new ProductSearchParam
            {
                searchText = searchText
            };
            var result = await _http.GetAsync($"api/Product/SearchProducts?" +
                                                 $"Currentpage={param.Currentpage}" +
                                                 $"&searchText={param.searchText}" +
                                                 $"&filterCata={param.filterCata}" +
                                                 $"&filterType={param.filterType}" +
                                                 $"&minPrice={param.minPrice}" +
                                                 $"&maxPrice={param.maxPrice}");

            result.EnsureSuccessStatusCode();

            var ProductsSearch = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<NewProductDto>>>())!.Data;

            if (result != null && ProductsSearch != null)
            {
                Products = ProductsSearch;
            }
            if (Products.Count == 0) Message = "No Products found.";
            ProductsChanged.Invoke();
        }

        //public async Task SearchProducts(string searchText, int page)
        //{
        //    LastSearchText = searchText;
        //    var result = await _http
        //         .GetFromJsonAsync<ServiceResponse<ProductSearchResultDto>>($"api/product/search/{searchText}/{page}");
        //    if (result != null && result.Data != null)
        //    {
        //        Products = result.Data.Products;
        //        CurrentPage = result.Data.CurrentPage;
        //        PageCount = result.Data.Pages;
        //    }
        //    if (Products.Count == 0) Message = "No products found.";
        //    ProductsChanged?.Invoke();
        //}

        public async Task<ProductDto> UpdateProduct(ProductDto product)
        {
            var result = await _http.PutAsJsonAsync($"api/product/updateproduct", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<ProductDto>>();
            return content.Data;
        }

        public async Task<int> UpdateStock(StocksDto stockDto)
        {
            var result = await _http.PostAsJsonAsync($"api/Product/UpdateStock", stockDto);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            return content.Data;
        }

        public async Task<List<DecreasedProductsDto>> GetDecreasedProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<DecreasedProductsDto>>>($"api/product/GetDecreasedProducts");
            if(result.Data.Count > 0)
            {
                return result.Data;
            }
            else 
            { 
                return new List<DecreasedProductsDto>();
            }
        }

        public async Task ResetPromotion()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/product/UpdatePromotion");
        }
    }
}
