
using Maew123.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Maew123.Api.Utilities;
using Maew123.Api.Services;
using Maew123.Models.Models;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccessController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public AdminAccessController(IUserRepository userRepository, AuthService authService)
        {
            this._userRepository = userRepository;
            this._authService = authService;
        }

        //Logging
        [HttpGet("[action]"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<List<UserLogDto>>>> GetUsersLog()
        {
            try
            {
                var usersLog = await _userRepository.GetUsersLog();
                return Ok(new ServiceResponse<List<UserLogDto>>
                {
                    Data = usersLog,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("[action]/{userId}"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<UserLogDetailsDto>>> GetUserLogDetails(int userId)
        {
            try
            {
                var userLogDetails = await _userRepository.GetUserLogDetails(userId);
                return Ok(new ServiceResponse<UserLogDetailsDto>
                {
                    Data = userLogDetails,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //Employee management
        [HttpGet("[action]/{roleId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<UserInfoDto>>>> GetAllUsers()
        {
            try
            {
                var usersbyRole = await _userRepository.GetAllUser();
                return Ok(new ServiceResponse<List<UserInfoDto>>
                {
                    Data = usersbyRole,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet("[action]/{roleId}"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<List<UserInfoDto>>>> GetUsersByRole(int roleId)
        {
            try
            {
                var usersbyRole = await _userRepository.GetUsersByrole(roleId);
                return Ok(new ServiceResponse<List<UserInfoDto>>
                {
                    Data = usersbyRole,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet("[action]/{userId}"), Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ServiceResponse<UserInfoDto>>> GetUserDetails(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId!);
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

        [HttpPost("[action]"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserInfoDto>>> CreateNewEmployee(RegisterRequest userDto)
        {
            try
            {
                var Response = await _authService.CreateEmployee(userDto);
                if (Response.Data)
                {
                    return Ok(Response);
                }
                else
                {
                    return BadRequest(Response);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //Getdetails ก่อนค่อยมานี่
        [HttpPut("[action]"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserInfoDto>>> UpdateEmployee(UserInfoDto userDto)
        {
            try
            {
                var user = await _userRepository.GetUserById(userDto.UserId!);
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                user.GetFromDto(userDto);
                user.UpdateBy = "Admin";
                user.UpdateDate = DateTime.Now;
                if (await _userRepository.UpdateUserData(user))
                {
                    return Ok(new ServiceResponse<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = "Update User Data Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Update ข้อมูลไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpDelete("[action]/{userId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserInfoDto>>> DeleteEmployee(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId!);
                if (user == null)
                {
                    return BadRequest(new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "User not found."
                    });
                }

                user.RoleId = 11;
                if (await _userRepository.UpdateUserData(user))
                {
                    await _userRepository.DeactivateUserTokens(userId);
                    return Ok(new ServiceResponse<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = "Delete User Successfully."
                    });
                }

                return BadRequest(new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Delete ข้อมูลไม่สำเร็จ"
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
