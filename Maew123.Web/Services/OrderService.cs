using Blazored.LocalStorage;
using Maew123.Models;
using Maew123.Models.Models;
using Maew123.Web.Pages;
using Maew123.Web.Services.Contracts;
using static System.Net.WebRequestMethods;

namespace Maew123.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        public OrderService(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }


        public List<CartsDto> Sale { get; set; } = new List<CartsDto>();
        //public SaleFilterResultDto AdminSale { get; set; } = new SaleFilterResultDto();
        public event Action SalesChanged;

        public async Task<ServiceResponse<int>> Checkout(CartDto cart)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/Checkout", cart);
            var newProduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<int>>())!;
            return newProduct;
        }

        public async Task GetSaleHistory()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetSaleHistory");

            if (result != null && result.Data != null)
                Sale = result.Data;


            if (Sale.Count == 0)
                return;

            SalesChanged.Invoke();

        }

        public async Task GetPaymentRequest()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetPaymentRequest");

            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            if (Sale?.Count < 1)
                return;

            try
            {
                SalesChanged.Invoke();
            }
            catch (Exception ex)
            {

            }


        }

        public async Task<bool> AnnotateByAdmin(CartsDto cart)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/AnnotateByAdmin", cart);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;
        }

        public async Task<bool> Payment(CartsDto cart)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/Payment", cart);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;

        }

        public async Task<bool> CancelOrder(CartsDto cart)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/CancelOrder", cart);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;
        }

        public async Task GetAlreadyPayment()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetAlreadyPayment");
            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            SalesChanged.Invoke();
        }

        public async Task GetWaitForSent()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetWaitForSent");
            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            SalesChanged.Invoke();
        }

        public async Task GetAlreadySent()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetAlreadySent");
            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            SalesChanged.Invoke();
        }

        public async Task GetAnnotatedOrder()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetAnnotatedOrder");
            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            SalesChanged.Invoke();
        }

        public async Task<int> GetAnnotatedCount()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<int>>("api/Sale/GetAnnotatedCount");
            return result.Data;
        }

        public async Task GetAllSaleForUser()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetAllSaleById");
            Sale = result.Data.OrderByDescending(s => s.OrderDate).ToList();

            SalesChanged.Invoke();
        }

        //AdminParts
        public async Task<SaleFilterResultDto> GetAllSalesByStatus(SaleFilterParam saleFilterParam)
        {
            var queryString = $"Currentpage={saleFilterParam.Currentpage}" +
                      $"&StatusIds={string.Join("&StatusIds=", saleFilterParam.StatusIds)}" +
                      $"&Year={saleFilterParam.Year}" +
                      $"&Month={saleFilterParam.Month}";

            if (!string.IsNullOrEmpty(saleFilterParam.OrderBy))
            {
                queryString += $"&OrderBy={saleFilterParam.OrderBy}";
            }

            if (!string.IsNullOrEmpty(saleFilterParam.SortDirection))
            {
                queryString += $"&SortDirection={saleFilterParam.SortDirection}";
            }

            var result = await _http.GetAsync($"api/Sale/GetAllSalesByStatus?{queryString}");

            result.EnsureSuccessStatusCode();

            var sale = (await result.Content.ReadFromJsonAsync<ServiceResponse<SaleFilterResultDto>>())!.Data;
            if (sale != null || sale.Carts.Count != 0)
            {
                return sale;

            }
            return null!;
        }

        public async Task<List<CartsDto>> GetAllSaleForReport()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<CartsDto>>>("api/Sale/GetAllSalesForReport");
            if (result != null || result.Data.Count != 0)
            {
                return result.Data!;
            }
            return null;
        }

        public async Task<bool> ConfirmByAdmin(CartsDto cartsDto)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/ConfirmByAdmin", cartsDto);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;
        }
        public async Task<bool> AlreadySentByAdmin(CartsDto cartsDto)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/AlreadySentByAdmin", cartsDto);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;
        }
        public async Task<bool> CancelByAdmin(CartsDto cartsDto)
        {
            var result = await _http.PostAsJsonAsync("api/Sale/CancelByAdmin", cartsDto);
            var IsSuccess = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
            return IsSuccess!;
        }

        public async Task<CartsDto> GetCartsBySaleId(int saleId)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<CartsDto>>($"api/Sale/GetCartsById/{saleId}");

            if(result != null && result.Success == true)
            {
                return result.Data;
            }
            return result.Data;
        }
    }

}
