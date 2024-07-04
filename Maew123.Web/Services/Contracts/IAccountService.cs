using Maew123.Models;
using Microsoft.AspNetCore.Authorization;

namespace Maew123.Web.Services.Contracts
{
    public interface IAccountService
    {
        event Action userInfoChanged;
        UserInfoDto userInfo { get; set; }
        List<Address> userAddresses { get; set; }
        List<WishListItem> wishListItems { get; set; }

        Task GetUserInfo();
        Task<bool> UpdateUserInfo(UserInfoDto userInfo);
        Task<Address> GetAddressByAddressId(int? AddressId);
        Task GetUserAddresses();
        Task<bool> SetAddressDefault(int AddressId);
        Task<List<Province>> GetProvinces();
        Task<List<Amphoe>> GetAmphoes(int provinceCode);
        Task<List<Tambol>> GetTambols(int amphoeCode);
        Task<bool> SaveAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(int addressId);

        Task<AddressSaleSnapshot> GetAddressSnapshotBysId(int? AddressId);
        Task<List<AddressSaleSnapshot>> GetAddressSnapshots();
        //GetWishlishes
        Task GetWishListItems();
        Task<bool> IsInWishlist(int productId);
        Task<bool> AddWishlist(int ProductId);
        Task<bool> RemoveWishlist(int ProductId);
    }
}
