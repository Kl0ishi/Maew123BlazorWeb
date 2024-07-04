using GoogleCaptchaComponent.Models;
using Maew123.Models;
using Maew123.Models.Dtos;

namespace Maew123.Web.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<LoginResult> LoginAsync(LoginRequest loginRequest);
        //ใช้อันเดียวกัน
        Task<LoginResult> RegisterAsync(RegisterRequest registerRequest);
        Task<ServiceResponse<bool>> VerifyOtpIdentity(string otpCode);
        Task<bool> HasOTPIdentity();
        Task LogoutAsync();
        Task<ServiceResponse<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<bool> HasOTP();
        Task<ServiceResponse<bool>> VerifyOtp(string otpCode, int count);
        Task<bool> IsUserAuthenticated();
        //Task<string> Test();

        Task<ServerSideCaptchaValidationResultModel> ServerSideValidationHandler(ServerSideCaptchaValidationRequestModel requestModel);
    }
}
