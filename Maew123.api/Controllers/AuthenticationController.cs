using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Maew123.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Maew123.Api.Models;
using System.Security.Cryptography;
using Maew123.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Maew123.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthenticationController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                LoginResult loginResult  = await _authService.AuthenticateAsync(model);
                if (loginResult.Result == false && loginResult.Token != null) 
                {
                    LoginResult resultForIdentify = await _authService.WaitForIdentify(model.Username);
                    return Ok(resultForIdentify);
                }
                return Ok(loginResult);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                LoginResult registerResult = await _authService.RegisterAsync(registerRequest);
                if(registerResult.Result == true)
                {
                    //จริงๆไม่ควรใช้ login result เพราะแม่งซ้ำ และชื่อไม่สอดคล้อง
                    LoginResult resultForIdentify = await _authService.WaitForIdentify(registerRequest.Email);

                    return Ok(resultForIdentify);
                }
                return Ok(registerResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> Identifying([FromBody] string otpCode)
        {
            try
            {
                var OtpJti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
                var response = await _authService.VerifyIdentity(OtpJti!, otpCode);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message = "เหลี่ยม" });
            }
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest cp)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _authService.ChangePassword(int.Parse(userId), cp.oldPassword, cp.newPassword);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword([FromBody]string email)
        {
            var response = await _authService.ForgotPassword(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse<bool>>> VerifyOtp([FromBody]string otpCode)
        {
            try
            {
                var OtpJti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
                var response = await _authService.VerifyOtp(OtpJti!, otpCode);
                if (!response.Success)
                { 
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new ServiceResponse<bool> { Success = false, Message="เหลี่ยม"});
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> TestItem(string token)
        {
            string message = "เทสเฉยๆนั่นแหละนะ";

            if (!string.IsNullOrEmpty(token))
            {
                var idk = ParseClaimsFromJwt(token);
                var parseToken =  new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                return Ok($"{message}: {parseToken}");
            }
            await Task.Run(() => message = "");
            return BadRequest("ทำไมไม่กรอก TOKEN มา!!!!");
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split(".")[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));

            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
