using Maew123.Models;

namespace Maew123.Web.Services.Contracts
{
    public interface IAdminService
    {
        Task<ServiceResponse<List<UserLogDto>>> GetUsersLog();
        Task<ServiceResponse<UserLogDetailsDto>> GetUserLogDetails(int userId);
        Task<ServiceResponse<List<UserInfoDto>>> GetUsersByRole(int roleId);
        Task<ServiceResponse<List<UserInfoDto>>> GetAllUser();
        Task<ServiceResponse<UserInfoDto>> GetUserDetails(int userId);
        Task<ServiceResponse<UserInfoDto>> CreateNewEmployee(RegisterRequest userDto);
        Task<ServiceResponse<UserInfoDto>> UpdateEmployee(UserInfoDto userDto);
        Task<ServiceResponse<UserInfoDto>> DeleteEmployee(int userId);
    }
}
