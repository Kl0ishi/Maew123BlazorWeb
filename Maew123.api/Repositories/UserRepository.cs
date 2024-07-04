using Maew123.Api.Models;
using Maew123.Api.Utilities;
using Microsoft.EntityFrameworkCore;
using Maew123.Api.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Maew123.Models.Models;

namespace Maew123.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ItshopMaew123Context _dbContext;

        public UserRepository(ItshopMaew123Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoginDb> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await (from a in _dbContext.Users
                                  join b in _dbContext.Roles on a.RoleId equals b.Id
                                  where a.Username == username
                                  select new LoginDb
                                  {
                                      UserId = a.UserId,
                                      Username = a.Username,
                                      Password = a.Password,
                                      Salt = a.Salt,
                                      FirstName = a.FirstName,
                                      LastName = a.LastName,
                                      Email = a.Email,
                                      RoleId = a.RoleId,
                                      RoleName = b.Name
                                  }).FirstOrDefaultAsync();

                return user!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<LoginDb> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await (from a in _dbContext.Users
                                  join b in _dbContext.Roles on a.RoleId equals b.Id
                                  where a.Email == email
                                  select new LoginDb
                                  {
                                      UserId = a.UserId,
                                      Username = a.Username,
                                      Password = a.Password,
                                      Salt = a.Salt,
                                      FirstName = a.FirstName,
                                      LastName = a.LastName,
                                      Email = a.Email,
                                      RoleId = a.RoleId,
                                      RoleName = b.Name
                                  }).FirstOrDefaultAsync();

                return user!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            return user!;
        }

        public async Task<string> GetUserPasswordHashAsync(string username)
        {
            var password = await _dbContext.Users
                .Where(u => u.Username == username)
                .Select(u => u.Password)
                .FirstOrDefaultAsync();
            return password;
        }

        public LoginDb GetUserFromClaimsAsync(IEnumerable<Claim> claims)
        {
            var user = new LoginDb
            {
                Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                Email = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                RoleName = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
            };
            return user;
        }

        public async Task SaveTokenJWT(String jwtToken, ClaimsPrincipal jwtClaimPrinciple)
        {
            try
            {
                var usernameClaim = jwtClaimPrinciple.getUsernameClaim();
                var user = await GetUserByUsernameAsync(usernameClaim);

                var expClaimValue = jwtClaimPrinciple.getExpiredClaim();
                var iatClaimValue = DateTime.Now;

                var newToken = new Token
                {
                    Tokenkey = jwtToken,
                    Status = true,
                    UserId = user.UserId,
                    Date = iatClaimValue,
                    Exp = expClaimValue,
                };

                _dbContext.Tokens.Add(newToken);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task SaveUserData(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                // Log or inspect innerException to see the details
                Console.WriteLine(innerException?.Message);
                throw;
            }
        }

        public async Task<bool> UpdateUserData(User user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Address>> GetAddresses(int userId)
        {
            var Addresses = await _dbContext.Addresses
                            .Include(a => a.User)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();

            return Addresses;
        }

        public async Task<Address> GetAddressByAddressId(int AddressId)
        {
            var Address = await _dbContext.Addresses
                            .FirstOrDefaultAsync(a => a.Id == AddressId);

            return Address;
        }

        public async Task<Address> SetAddressDefault(Address address, int userId)
        {
            //อัปอันนี้ให้เป็น default
            _dbContext.Addresses.Update(address);

            //เอาอัน default ออกจากอันอื่นของuserคนนี้
            var otherAddresses = await _dbContext.Addresses
                .Where(a => a.UserId == userId && a.Id != address.Id)
                .ToListAsync();

            foreach (var otherAddress in otherAddresses)
            {
                otherAddress.IsDefault = false;
            }

            await _dbContext.SaveChangesAsync();

            return address;
        }

        public async Task<List<Province>> GetProvinces()
        {
            var Provinces = await _dbContext.Provinces.ToListAsync();

            return Provinces;
        }

        public async Task<List<Amphoe>> GetAmphoes(int provinceCode)
        {
            var Amphoes = await _dbContext.Amphoes.Where(a => a.Pcode == provinceCode).ToListAsync();
            return Amphoes;
        }

        public async Task<List<Tambol>> GetTambols(int amphoeCode)
        {
            var Tambols = await _dbContext.Tambols.Where(t => t.Acode == amphoeCode).ToListAsync();
            return Tambols;
        }

        public async Task<bool> SaveAddress(Address address)
        {
            address.User = null;
            _dbContext.Add(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            address.User = null;
            var addressDb = await _dbContext.Addresses
                            .Where(a => a.Id == address.Id)
                            .FirstOrDefaultAsync();

            if (addressDb != null)
            {
                addressDb.FirstName = address.FirstName;
                addressDb.LastName = address.LastName;
                addressDb.Street = address.Street;
                addressDb.City = address.City;
                addressDb.State = address.State;
                addressDb.Zip = address.Zip;
                addressDb.Country = address.Country;
                addressDb.Phone = address.Phone;
                addressDb.addressName = address.addressName;

                _dbContext.Update(addressDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAddress(int id)
        {
            var addressDb = await _dbContext.Addresses
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();

            if (addressDb != null)
            {
                _dbContext.Remove(addressDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<AddressSaleSnapshot> GetAddressSnapshotById(int AddressSnapshotId)
        {
            var Address = await _dbContext.AddressSaleSnapshots
                            .FirstOrDefaultAsync(a => a.Id == AddressSnapshotId);

            return Address;
        }

        public async Task<List<AddressSaleSnapshot>> GetAddressSnapshots(int userId)
        {
            var Addresses = await _dbContext.AddressSaleSnapshots
                            .Include(a => a.User)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            return Addresses;
        }

        public async Task<int> InsertAddressSaleSnapshot(AddressSaleSnapshot addressSaleSnapshot)
        {
            _dbContext.AddressSaleSnapshots.Add(addressSaleSnapshot);

            await _dbContext.SaveChangesAsync();

            return addressSaleSnapshot.Id;
        }

        public async Task<List<UserLogDto>> GetUsersLog()
        {
            var userLogs = await _dbContext.Tokens
                .Where(t => t.Status == true)
                .GroupBy(t => t.UserId)
                .Select(g => new UserLogDto
                {
                    UserId = g.Key.GetValueOrDefault(),
                    UserName = g.First().User.Username,
                    LastLogin = g.Max(t => t.Date)
                })
                .ToListAsync();

            return userLogs;
        }

        public async Task<UserLogDetailsDto> GetUserLogDetails(int userId)
        {
            var userLogs = await _dbContext.Tokens
                .Include(t => t.User)
                .Where(t => t.UserId == userId && t.Status == true)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var userLogDetails = new UserLogDetailsDto
            {
                Tokens = userLogs,
                Username = userLogs.FirstOrDefault()?.User?.Username
            };
            foreach (var userLog in userLogDetails.Tokens)
            {
                userLog.User = null;
            }
            return userLogDetails;
        }

        public async Task<List<UserInfoDto>> GetAllUser()
        {
            var users = await _dbContext.Users
                .Select(u => new UserInfoDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserAddress = u.UserAddress,
                    UserTel = u.UserTel,
                    Gender = u.Gender,
                    InsertBy = u.InsertBy,
                    InsertDate = u.InsertDate,
                    UpdateDate = u.UpdateDate,
                    UpdateBy = u.UpdateBy,
                    RoleId = u.RoleId
                })
                .ToListAsync();
            return users;
        }

        public async Task<List<UserInfoDto>> GetUsersByrole(int roleId)
        {
            var users = await _dbContext.Users
                .Where(u => u.RoleId == roleId)
                .Select(u => new UserInfoDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserAddress = u.UserAddress,
                    UserTel = u.UserTel,
                    Gender = u.Gender,
                    InsertBy = u.InsertBy,
                    InsertDate = u.InsertDate,
                    UpdateDate = u.UpdateDate,
                    UpdateBy = u.UpdateBy,
                    RoleId = u.RoleId
                })
                .ToListAsync();
            return users;
        }
        public async Task DeactivateUserTokens(int userId)
        {
            var userTokens = await _dbContext.Tokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            foreach (var token in userTokens)
            {
                token.Status = false;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<WishList> GetWishListByUserId(int userId)
        {
            var wishlist = await _dbContext.WishLists.FirstOrDefaultAsync(w => w.UserId == userId);
            return wishlist;
        }

        public async Task<List<WishListItem>> GetWishListItems(int WishListId)
        {
            var wishlistItems = await _dbContext.WishListItems
                                .Include(w => w.Product)
                                .ThenInclude(p => p.Promotion)
                                .Where(w => w.WishListId == WishListId)
                                .ToListAsync();

            return wishlistItems;
        }

        public async Task<int> AddWishList(WishList wishList)
        {
            _dbContext.WishLists.Add(wishList);
            await _dbContext.SaveChangesAsync();
            return wishList.WishListId;
        }

        public async Task<bool> AddWishListItem(WishListItem wishListItem)
        {
            _dbContext.WishListItems.Add(wishListItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveWishlistItem(int WishlistId, int ProductId)
        {
            var wishlistitem = await _dbContext.WishListItems
                               .Where(w => w.WishListId == WishlistId && w.ProductId == ProductId)            
                                .FirstOrDefaultAsync();

            if(wishlistitem != null)
            {
                _dbContext.WishListItems.Remove(wishlistitem);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> wishlistItemExist(WishListItem wishListItem)
        {
            var wishitemExist = await _dbContext.WishListItems.FirstOrDefaultAsync(w => w.WishListId == wishListItem.WishListId && w.ProductId == wishListItem.ProductId);
            if (wishitemExist == null)
            {
                return false;
            }
            return true;

        }
    }
}
