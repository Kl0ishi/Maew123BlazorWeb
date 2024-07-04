using Maew123.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Maew123.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<LoginDb> GetUserByUsernameAsync(string username);
        Task<LoginDb> GetUserByEmailAsync(string email);
        Task<string> GetUserPasswordHashAsync(string username);
        Task<User> GetUserById(int id);
        LoginDb GetUserFromClaimsAsync(IEnumerable<Claim> claims);
        Task SaveTokenJWT(String JweToken, ClaimsPrincipal JweClaim);

        Task SaveUserData(User user);
        Task<bool> UpdateUserData(User user);

        Task<List<Address>> GetAddresses(int userId);
        Task<Address> GetAddressByAddressId(int AddressId);
        Task<Address> SetAddressDefault(Address address, int userId);
        Task<List<Province>> GetProvinces();
        Task<List<Amphoe>> GetAmphoes(int provinceCode);
        Task<List<Tambol>> GetTambols(int amphoeCode);
        Task<bool> SaveAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(int id);

        Task<AddressSaleSnapshot> GetAddressSnapshotById(int AddressSnapshotId);
        Task<List<AddressSaleSnapshot>> GetAddressSnapshots(int userId);
        Task<int> InsertAddressSaleSnapshot(AddressSaleSnapshot addressSaleSnapshot);

        Task<WishList> GetWishListByUserId(int userid);
        Task<List<WishListItem>> GetWishListItems(int WishListId);
        Task<int> AddWishList(WishList wishList);
        Task<bool> AddWishListItem(WishListItem wishListItem);
        Task<bool> RemoveWishlistItem(int UserId, int ProductId);
        Task<bool> wishlistItemExist(WishListItem wishListItem);

        Task<List<UserLogDto>> GetUsersLog();
        Task<UserLogDetailsDto> GetUserLogDetails(int userId);
        Task<List<UserInfoDto>> GetAllUser();
        Task<List<UserInfoDto>> GetUsersByrole(int roleId);
        Task DeactivateUserTokens(int userId);
    }
}
