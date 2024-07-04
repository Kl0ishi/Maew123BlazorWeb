using Maew123.Models;
using Maew123.Models.Models;
using Maew123.Web.Services.Contracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Maew123.Web.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _http;

        public AccountService(HttpClient http)
        {
            _http = http;
        }

        public event Action userInfoChanged;
        public UserInfoDto userInfo { get; set; } = new UserInfoDto();
        public List<Address> userAddresses { get; set; } = new List<Address>();
        public List<WishListItem> wishListItems { get; set; } = new List<WishListItem>();

        public async Task GetUserInfo()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserInfoDto>>($"api/Account/GetUserInfo");
            if (result != null && result.Data != null)
            {
                userInfo = result.Data;
                if (userInfoChanged != null)
                {
                    userInfoChanged.Invoke();
                }
            }
        }

        public async Task<bool> UpdateUserInfo(UserInfoDto userInfo)
        {
            var result = await _http.PostAsJsonAsync($"api/Account/UpdateUserInfo", userInfo);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
        }

        public async Task<Address> GetAddressByAddressId(int? AddressId)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<Address>>($"api/Account/GetAddressById/{AddressId}");
                if(result.Data != null && result.Success)
                {
                    return result.Data;
                }
                return new Address();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task GetUserAddresses()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<List<Address>>>($"api/Account/GetUserAddresses");
                if (result != null && result.Data?.Count != 0)
                {
                    userAddresses = result.Data;
                    if (userInfoChanged != null)
                    {
                        userInfoChanged.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> SetAddressDefault(int AddressId)
        {
            var result = await _http.PostAsJsonAsync($"api/Account/SetAddressDefault", AddressId);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Address>>();
            return content.Success;

        }

        public async Task<List<Province>> GetProvinces()
        {
            var Provinces = await _http.GetFromJsonAsync<ServiceResponse<List<Province>>>("api/Account/GetProvinces");
            return Provinces.Data;
        }

        public async Task<List<Amphoe>> GetAmphoes(int provinceCode)
        {
            var Amphoes = await _http.GetFromJsonAsync<ServiceResponse<List<Amphoe>>>($"api/Account/GetAmphoes/{provinceCode}");
            return Amphoes.Data;
        }

        public async Task<List<Tambol>> GetTambols(int amphoeCode)
        {
            var Tambols = await _http.GetFromJsonAsync<ServiceResponse<List<Tambol>>>($"api/Account/GetTambols/{amphoeCode}");
            return Tambols.Data;
        }

        public async Task<bool> SaveAddress(Address address)
        {
            var result = await _http.PostAsJsonAsync($"api/Account/SaveAddress", address);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            var result = await _http.PutAsJsonAsync($"api/Account/UpdateAddress", address);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>())!.Data;
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            try
            {
                var result = await _http.DeleteFromJsonAsync<ServiceResponse<bool>>($"api/Account/DeleteAddress?addressId={addressId}");
                return result.Data;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        //อันใหม่ใช้เน้นกับ SaleHistory
        public async Task<AddressSaleSnapshot> GetAddressSnapshotBysId(int? AddressId)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<AddressSaleSnapshot>>($"api/Account/GetAddressSnapshotById/{AddressId}");
                if (result.Data != null && result.Success)
                {
                    return result.Data;
                }
                return new AddressSaleSnapshot();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AddressSaleSnapshot>> GetAddressSnapshots()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<List<AddressSaleSnapshot>>>($"api/Account/GetAddressSnapshots");
                if (result != null && result.Data?.Count != 0)
                {
                    return result.Data;
                }
                return new List<AddressSaleSnapshot>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GetWishlishes
        public async Task GetWishListItems()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<List<WishListItem>>>($"api/Account/GetWishListItems");
                if (result != null && result.Data?.Count != 0)
                {
                    wishListItems = result.Data;

                    if (userInfoChanged != null)
                    {
                        userInfoChanged.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
        }

        public async Task<bool> IsInWishlist(int productId)
        {
            try
            {
                return await Task.FromResult(wishListItems.Any(item => item.ProductId == productId));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddWishlist(int ProductId)
        {
            var result = await _http.PostAsJsonAsync($"api/Account/AddWishlist", ProductId);
            var idk = (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>())!;
            if (idk.Success)
            {
                var itemToAdd = wishListItems.FirstOrDefault(item => item.Product!.ProductId == ProductId);
                if(itemToAdd != null)
                {
                    wishListItems.Add(itemToAdd!);
                    userInfoChanged.Invoke();
                }
            }
            return idk.Success;
        }


        public async Task<bool> RemoveWishlist(int ProductId)
        {
            var result = await _http.DeleteFromJsonAsync<ServiceResponse<bool>>($"api/Account/RemoveWishlist?ProductId={ProductId}");
            var itemToRemove = wishListItems.FirstOrDefault(item => item.Product!.ProductId == ProductId);
            if (itemToRemove != null)
            {
                wishListItems.Remove(itemToRemove);
                userInfoChanged?.Invoke();
            }

            return result!.Success;
        }
    }
}
