using Azure;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using GoogleCaptchaComponent.Models;
using Maew123.Models;
using Maew123.Models.Dtos;
using Maew123.Web.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Maew123.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISessionStorageService sessionStorageService;
        private string jwtToken;

        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider AuthStateProvider, ISessionStorageService sessionStorageService)
        {
            this._http = httpClient;
            this.localStorageService = localStorageService;
            this._authStateProvider = AuthStateProvider;
            this.sessionStorageService = sessionStorageService;
        }


        public async Task<LoginResult> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _http.PostAsJsonAsync<LoginRequest>("api/Authentication/Login", loginRequest);

                if (!response.IsSuccessStatusCode)
                {
                    throw new UnauthorizedAccessException("Login failed.");
                }

                var content = await response.Content.ReadFromJsonAsync<LoginResult>() ?? throw new InvalidDataException();
                if(content.Expired < DateTime.Now) 
                {
                    await localStorageService.SetItemAsync<string>("OtpIdentity", content!.Token!);
                }
                else
                {
                    await localStorageService.SetItemAsync<string>("JwtToken", content!.Token!);
                }
                
                return content;
            }
            catch (Exception ex)
            {

                return new LoginResult { Errors = new List<string> { $"มีข้อผิดพลาด {ex.Message}" } };
            }

        }

        public async Task LogoutAsync()
        {
            await localStorageService.RemoveItemAsync("JwtToken");

            if (await localStorageService.ContainKeyAsync("cart"))
            {
                await localStorageService.RemoveItemAsync("cart");
            }

            if (await localStorageService.ContainKeyAsync("compareitem"))
            {
                await localStorageService.RemoveItemAsync("compareitem");
            }
        }

        public async Task<LoginResult> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var response = await _http.PostAsJsonAsync<RegisterRequest>("api/Authentication/Register", registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    var content = await response.Content.ReadFromJsonAsync<LoginResult>() ?? throw new InvalidDataException();
                    if (content.Token != null)
                    {
                        await localStorageService.SetItemAsync<string>("OtpIdentity", content!.Token!);
                    }

                    return content;
                }
                else
                {
                    return await response.Content.ReadFromJsonAsync<LoginResult>() ?? throw new InvalidDataException();
                }
            }
            catch (Exception ex)
            {
                return new LoginResult { Errors = new List<string> { $"มีข้อผิดพลาด {ex.Message}" } };
            }
        }

        public async Task<ServiceResponse<bool>> VerifyOtpIdentity(string otpCode)
        {
            var otpToken = await localStorageService.GetItemAsync<string>("OtpIdentity");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otpToken);

            var result = await _http.PostAsJsonAsync("api/Authentication/Identifying", otpCode);
            var data = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();


            return data;
        }

        public async Task<bool> HasOTPIdentity()
        {
            return await localStorageService.ContainKeyAsync("OtpIdentity");
        }


        public async Task<ServiceResponse<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _http.PostAsJsonAsync("api/Authentication/ChangePassword", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            try
            {
                await sessionStorageService.RemoveItemAsync("OTP");
                var result = await _http.PostAsJsonAsync("api/Authentication/ForgotPassword", email);

                var data = await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
                if (data.Success)
                {
                    await sessionStorageService.SetItemAsync("OTP", data.Data);
                }
                return data;

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new ServiceResponse<string> {Data=message};
            }

        }

        public async Task<bool> HasOTP()
        {
            return await sessionStorageService.ContainKeyAsync("OTP");
        }

        public async Task<ServiceResponse<bool>> VerifyOtp(string otpCode, int count)
        {
            if(count > 3)
            {
                await sessionStorageService.RemoveItemAsync("OTP");
                return new ServiceResponse<bool> { Message = "คุณกรอกOTPผิดเกินรอบกำหนด" };
            }
            var otpToken = await sessionStorageService.GetItemAsync<string>("OTP");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otpToken);

            var result = await _http.PostAsJsonAsync("api/Authentication/VerifyOtp", otpCode);
             var data = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            
            return data;
            //ส่งรหัสใหม่กลับไป พร้อมบอกว่าสำเร็จแล้ว
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        //public async Task<string> Test()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync("api/Authentication/SetRefreshToken");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            //กรณีเข้าถึง api ได้ แต่ไม่มีข้อมูลกลับมา
        //            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        //            {
        //                return default!;
        //            }

        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        else
        //        {
        //            var message = await response.Content.ReadAsStringAsync();
        //            throw new Exception(message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }

        //}

        public async Task<ServerSideCaptchaValidationResultModel> ServerSideValidationHandler(ServerSideCaptchaValidationRequestModel requestModel)
        {
            // Make API call to verify the reCAPTCHA response
            var response = await _http.GetAsync($"api/Recaptcha/VerifyRecaptcha?token={requestModel.CaptchaResponse}");

            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<GoogleCaptchaCheckResponseResult>();
                return new ServerSideCaptchaValidationResultModel(apiResult.Success, string.Join("\n", apiResult.ErrorCodes ?? new List<string>() { "No Error" }));
            }
            else
            {
                // Handle API call failure
                return new ServerSideCaptchaValidationResultModel(false, "Failed to validate captcha");
            }
        }
    }
}
