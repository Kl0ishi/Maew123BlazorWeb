using Maew123.Models;
using Maew123.Models.Models;
using Maew123.Web.Services.Contracts;
using System.Net.Http;

namespace Maew123.Web.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;

        public AdminService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<ServiceResponse<List<UserLogDto>>> GetUsersLog()
        {
            try
            {
                var response = await _http.GetAsync($"api/AdminAccess/GetUsersLog");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserLogDto>>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<List<UserLogDto>> { Success = false, Message = "An error occurred while fetching users." };
            }
        }

        public async Task<ServiceResponse<UserLogDetailsDto>> GetUserLogDetails(int userId)
        {
            try
            {
                var response = await _http.GetAsync($"api/AdminAccess/GetUserLogDetails/{userId}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ServiceResponse<UserLogDetailsDto>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<UserLogDetailsDto> { Success = false, Message = "An error occurred while fetching user details." };
            }
        }

        public async Task<ServiceResponse<List<UserInfoDto>>> GetAllUser()
        {
            try
            {
                var response = await _http.GetAsync($"api/AdminAccess/GetAllUsers");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserInfoDto>>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<List<UserInfoDto>> { Success = false, Message = "An error occurred while fetching users." };
            }
        }

        public async Task<ServiceResponse<List<UserInfoDto>>> GetUsersByRole(int roleId)
        {
            try
            {
                var response = await _http.GetAsync($"api/AdminAccess/GetUsersByRole/{roleId}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserInfoDto>>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<List<UserInfoDto>> { Success = false, Message = "An error occurred while fetching users." };
            }
        }

        public async Task<ServiceResponse<UserInfoDto>> GetUserDetails(int userId)
        {
            try
            {
                var response = await _http.GetAsync($"api/AdminAccess/GetUserDetails/{userId}");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<ServiceResponse<UserInfoDto>>();
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<UserInfoDto> { Success = false, Message = "An error occurred while fetching user details." };
            }
        }

        public async Task<ServiceResponse<UserInfoDto>> CreateNewEmployee(RegisterRequest userDto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/AdminAccess/CreateNewEmployee", userDto);
                response.EnsureSuccessStatusCode();

                return (await response.Content.ReadFromJsonAsync<ServiceResponse<UserInfoDto>>())!;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<UserInfoDto> { Success = false, Message = "An error occurred while creating a new employee." };
            }
        }

        public async Task<ServiceResponse<UserInfoDto>> UpdateEmployee(UserInfoDto userDto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/AdminAccess/UpdateEmployee", userDto);
                response.EnsureSuccessStatusCode();

                return (await response.Content.ReadFromJsonAsync<ServiceResponse<UserInfoDto>>())!;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<UserInfoDto> { Success = false, Message = "An error occurred while updating the employee." };
            }
        }

        public async Task<ServiceResponse<UserInfoDto>> DeleteEmployee(int userId)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/AdminAccess/DeleteEmployee/{userId}");
                response.EnsureSuccessStatusCode();

                return (await response.Content.ReadFromJsonAsync<ServiceResponse<UserInfoDto>>())!;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error: {ex.Message}");
                return new ServiceResponse<UserInfoDto> { Success = false, Message = "An error occurred while deleting the employee." };
            }
        }

    }
}
