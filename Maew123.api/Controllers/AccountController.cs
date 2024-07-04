using Maew123.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Maew123.Api.Utilities;
using Azure;
using Microsoft.EntityFrameworkCore;


namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<UserInfoDto>>> GetUserInfo()
        {
            try
            {
                var user = await getUser();
                var userDto = user.ToUserDto();
                return Ok(new
                {
                    Data = userDto,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserInfo([FromBody] UserInfoDto userInfo)
        {
            try
            {
                var user = await getUser();
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false, Success = false, Message = "User not found."
                    });
                }

                user.GetFromDto(userInfo);
                user.UpdateDate = DateTime.Now;
                user.UpdateBy = "System";
                if(await _userRepository.UpdateUserData(user))
                {
                    return Ok(new ServiceResponse<bool>
                    {
                        Data= true,
                        Success = true,
                        Message = "Update User Data Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false, Success = false, Message = "Update ข้อมูลไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]/{AddressId}"), Authorize]
        public async Task<ActionResult<ServiceResponse<Address>>> GetAddressById(int AddressId)
        {
            try
            {
                var user = await getUser();
                var Address = await _userRepository.GetAddressByAddressId(AddressId);
                if (Address == null)
                {
                    return Ok(new ServiceResponse<Address>
                    {
                        Data = { },
                        Success = false,
                        Message = "ท่านยังไม่เคยระบุที่อยู่"
                    });
                }

                return Ok(new
                {
                    Data = Address,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<Address>>>> GetUserAddresses()
        {
            try
            {
                var user = await getUser();
                var Addresses = await _userRepository.GetAddresses(user.UserId);
                if(Addresses == null || Addresses.Count == 0 )
                {
                    return Ok(new ServiceResponse<List<Address>>
                    {
                        Data =  {},
                        Success = false,
                        Message = "ท่านยังไม่เคยระบุที่อยู่"
                    });
                }

                return Ok(new
                {
                    Data = Addresses,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<Address>>> SetAddressDefault([FromBody] int AddressId)
        {
            var user = await getUser();
            var result = new ServiceResponse<Address>
            {
                Success = false,
            };

            try
            {
                var AddressToSet = await _userRepository.GetAddressByAddressId(AddressId);
                AddressToSet.IsDefault = true;
                var addressAfterUpdate = await _userRepository.SetAddressDefault(AddressToSet, user.UserId);
                if (addressAfterUpdate != null)
                {
                    result.Data = addressAfterUpdate;
                    result.Success = true;
                    return Ok(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<Province>>>> GetProvinces()
        {
            try
            {
                var user = await getUser();
                var Provinces = await _userRepository.GetProvinces();
                if(Provinces == null || Provinces.Count  == 0 )
                {
                    return Ok(new ServiceResponse<List<Province>>
                    {
                        Data = { },
                        Success = false,
                        Message = "หาจังหวัดไม่พบ"
                    });
                }

                return Ok(new
                {
                    Data = Provinces,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]/{provinceCode}"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<Amphoe>>>> GetAmphoes(int provinceCode)
        {
            try
            {
                var user = await getUser();
                var Amphoes = await _userRepository.GetAmphoes(provinceCode);
                if (Amphoes == null || Amphoes.Count == 0)
                {
                    return Ok(new ServiceResponse<List<Amphoe>>
                    {
                        Data = { },
                        Success = false,
                        Message = "หาอำเภอไม่พบ"
                    });
                }

                return Ok(new
                {
                    Data = Amphoes,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]/{amphoeCode}"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<Tambol>>>> GetTambols(int amphoeCode)
        {
            try
            {
                var user = await getUser();
                var Tambols = await _userRepository.GetTambols(amphoeCode);
                if (Tambols == null || Tambols.Count == 0)
                {
                    return Ok(new ServiceResponse<List<Tambol>>
                    {
                        Data = { },
                        Success = false,
                        Message = "หาตำบลไม่พบ"
                    });
                }

                return Ok(new
                {
                    Data = Tambols,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> SaveAddress(Address address)
        {
            try
            {
                var user = await getUser();
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                address.UserId = user.UserId;
                if (await _userRepository.SaveAddress(address))
                {
                    return Ok(new ServiceResponse<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = "Save Address Data Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Save Address ไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateAddress(Address address)
        {
            try
            {
                var user = await getUser();
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                address.UserId = user.UserId;
                if (await _userRepository.UpdateAddress(address))
                {
                    return Ok(new ServiceResponse<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = "Update Address Data Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Update Address ไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteAddress(int addressId)
        {
            try
            {
                var user = await getUser();
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                if (await _userRepository.DeleteAddress(addressId))
                {
                    return Ok(new ServiceResponse<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = "Delete Address Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Delete Address ไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]/{AddressId}"), Authorize]
        public async Task<ActionResult<ServiceResponse<AddressSaleSnapshot>>> GetAddressSnapshotById(int AddressId)
        {
            try
            {
                var user = await getUser();
                var Address = await _userRepository.GetAddressSnapshotById(AddressId);
                if (Address == null)
                {
                    return Ok(new ServiceResponse<AddressSaleSnapshot>
                    {
                        Data = { },
                        Success = false,
                        Message = "ท่านยังไม่เคยระบุที่อยู่"
                    });
                }

                return Ok(new
                {
                    Data = Address,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<AddressSaleSnapshot>>>> GetAddressSnapshots()
        {
            try
            {
                var user = await getUser();
                var Addresses = await _userRepository.GetAddressSnapshots(user.UserId);
                if (Addresses == null || Addresses.Count == 0)
                {
                    return Ok(new ServiceResponse<List<AddressSaleSnapshot>>
                    {
                        Data = { },
                        Success = false,
                        Message = "ท่านยังไม่เคยระบุที่อยู่"
                    });
                }

                return Ok(new
                {
                    Data = Addresses,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //GetWishlishes
        [HttpGet("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<WishListItem>>>> GetWishListItems()
        {
            var User = await getUser();
            var wishlist = await _userRepository.GetWishListByUserId(User.UserId);
            if(wishlist == null)
            {
                return Ok(new ServiceResponse<List<WishListItem>>());
            }
            try
            {
                var wishlistItems = await _userRepository.GetWishListItems(wishlist.WishListId);
                foreach (var wish in wishlistItems)
                {
                    if(wish.Product.Promotion != null)
                    {
                        wish.ProPrice = CalculatePrice.CalPromoPrice(wish.Product.Price, wish.Product.Promotion.PromotionType, wish.Product.Promotion.DiscountPer, 0, 0, 0);
                    }
                    wish.Product.Promotion = null;
                }
                
                return Ok(new ServiceResponse<List<WishListItem>>
                {
                    Data = wishlistItems,
                    Success = true
                });
            }
            catch (Exception ex)
            {

                return Ok(new ServiceResponse<List<WishListItem>>());
            }
            
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> AddWishlist([FromBody]int ProductId)
        {
            try
            {
                var user = await getUser();
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                var result = new ServiceResponse<bool>
                {
                    Data = true,
                    Success = true
                };

                //check if wishlist exist
                var wishlist = await _userRepository.GetWishListByUserId(user.UserId);

                //update wishlist if exist
                if(wishlist != null)
                {
                    var wishlistItem = new WishListItem
                    {
                        WishListId = wishlist.WishListId,
                        ProductId = ProductId,
                        AddWhen = DateTime.Now
                    };
                    if(!await _userRepository.wishlistItemExist(wishlistItem))
                    {
                        await _userRepository.AddWishListItem(wishlistItem);
                        return Ok(result);
                    }
                    
                }

                //create new wishlist if not exist
                if (wishlist == null)
                {
                    var NewWishlist = new WishList
                    {
                        UserId = user.UserId,
                    };

                    var NewWishListId = await _userRepository.AddWishList(NewWishlist);
                    
                        var wishlistItem = new WishListItem
                        {
                            WishListId = NewWishListId,
                            ProductId = ProductId,
                            AddWhen = DateTime.Now
                        };
                        await _userRepository.AddWishListItem(wishlistItem);
                        return Ok(result);
                    

                }

                result.Data = false; result.Success = false;
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //RemoveWishlist
        [HttpDelete("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveWishlist(int ProductId)
        {
            var User = await getUser();
            var result = new ServiceResponse<bool> { Success = true };

            var wishlist = await _userRepository.GetWishListByUserId(User.UserId);
            if (await _userRepository.RemoveWishlistItem(wishlist.WishListId, ProductId))
                return Ok(result);
            result.Success = false;
            return BadRequest(result);

        }

        private async Task<User> getUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userRepository.GetUserById(int.Parse(userId!));
        }

    }
}
