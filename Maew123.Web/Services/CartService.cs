using Blazored.LocalStorage;
using Maew123.Models;
using Maew123.Web.Services.Contracts;
using System.Net.Http.Json;

namespace Maew123.Web.Services
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CartService(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public event Action OnChange;

        public async Task AddToCart(ItemQuantityDto cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");
            if (cart == null)
            {
                cart = new List<ItemQuantityDto>();
            }

            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId);
            if (sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _localStorage.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task<List<ItemQuantityDto>> GetCartItems()
        {
            try
            {
                var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");
                if (cart == null)
                {
                    cart = new List<ItemQuantityDto>();
                }

                return cart;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<List<CartDetailsDto>> GetCartProducts()
        {
            try
            {
                var cartItems = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");
                var cartDto = new CartDto();
                foreach (var item in cartItems)
                {
                    cartDto.Quans.Add(item);
                }
                var response = await _http.PostAsJsonAsync("api/sale/GetCart", cartDto);
                var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartDetailsDto>>>();
                return cartProducts.Data;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task RemoveProductFromCart(int productId)
        {
            var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }
        }

        public async Task RemoveCart()
        {
            await _localStorage.RemoveItemAsync("cart");
            OnChange.Invoke();
        }

        public async Task RemoveCheckedItemsFromCart(List<CartDetailsDto> checkedProducts)
        {
            var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");

            if (cart == null)
            {
                return;
            }

            cart = cart.Where(cartItem => !checkedProducts.Any(checkedProduct => checkedProduct.ProductId == cartItem.ProductId)).ToList();

            await _localStorage.SetItemAsync("cart", cart);

            OnChange.Invoke();
        }

        public async Task UpdateQuantity(CartDetailsDto product)
        {
            var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == product.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;
                await _localStorage.SetItemAsync("cart", cart);

            }
        }

        public async Task<(bool success, string message)> AddCompareProduct(ItemQuantityDto compareitem)
        {
            var compare = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("compareitem");
            if (compare == null)
            {
                compare = new List<ItemQuantityDto>();
            }

            if (compare.Count >= 5)
            {
                return (false, "You can't compare more than 5 products!");
            }

            var sameItem = compare.Find(x => x.ProductId == compareitem.ProductId);
            if (sameItem == null)
            {
                compare.Add(compareitem);
                await _localStorage.SetItemAsync("compareitem", compare);
                OnChange.Invoke();
                return (true, "Item added successfully");
            }
            else
            {
                return (false, "You already have this item!");
            }
        }
        public async Task<List<ItemQuantityDto>> GetCompareItems()
        {
            try
            {
                var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("compareitem");
                if (cart == null)
                {
                    cart = new List<ItemQuantityDto>();
                }

                return cart;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<CartDetailsDto>> GetCompareProducts()
        {
            try
            {
                var compareItems = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("compareitem");
                var cartDto = new CartDto();
                foreach (var item in compareItems)
                {
                    cartDto.Quans.Add(item);
                }
                var response = await _http.PostAsJsonAsync("api/sale/GetCart", cartDto);
                var compareProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartDetailsDto>>>();
                return compareProducts.Data;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task RemoveProductFromComparing(int productId)
        {
            var cart = await _localStorage.GetItemAsync<List<ItemQuantityDto>>("compareitem");
            if (cart == null)
            {
                return;
            }

            var compareItem = cart.Find(x => x.ProductId == productId);
            if (compareItem != null)
            {
                cart.Remove(compareItem);
                await _localStorage.SetItemAsync("compareitem", cart);
                OnChange.Invoke();
            }
        }
    }
}
