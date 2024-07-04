using Maew123.Api.Models;

using Maew123.Api.Services.MailService;
using Maew123.Api.Utilities;
using Maew123.Models;

using Maew123.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Maew123.Api.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IOtpEntityRepository _otpRepository;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IOtpEntityRepository otpRepository, IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _otpRepository = otpRepository;
            _mailService = mailService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public async Task<LoginResult> AuthenticateAsync(LoginRequest model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username)
                        ?? await _userRepository.GetUserByEmailAsync(model.Username); //ควรแก้ให้ชัดเจนนะ

            if(user!=null)
            {
                if (user.RoleId == 11)
                {
                    return new LoginResult { Result = false, Errors = new List<string> { "ไอดีนี้ได้ถูกปิดการใช้งานไปแล้ว" } };
                }

                if (user.RoleId == 16)
                {
                    return new LoginResult { Result = false, Errors = new List<string> { "กรุณายืนยันตัวตนก่อนเข้าใช้งาน" }, Token = "wasda" };
                }

                if (VerifyPassword(model.Password, user.Password, user.Salt))
                {
                    var jwtToken = GenerateJWT(user);
                    var claimsPrincipal = DecryptJWT(jwtToken);
                    await _userRepository.SaveTokenJWT(jwtToken, claimsPrincipal);
                    var expireClaimValue = claimsPrincipal.getExpiredClaim();

                    if (expireClaimValue.HasValue)
                    {
                        DateTime expireDateTime = expireClaimValue.Value;

                        var LoginResult = new LoginResult()
                        {
                            Token = jwtToken,
                            Expired = expireDateTime,
                            Result = true,
                        };
                        return LoginResult;
                    }
                    else
                    {
                        return new LoginResult { Result = false, Errors = new List<string> { "เกิดข้อผิดพลาด" } };
                    }
                }
            }
            return new LoginResult() { Result = false, Errors = new List<string> { "ชื่อหรือรหัสผ่านไม่ถูกต้อง" } };
        }

        public async Task<LoginResult> AuthenticateAsync(RegisterRequest model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user != null && VerifyPassword(model.Password, user.Password, user.Salt))
            {
                var jwtToken = GenerateJWT(user);
                var claimsPrincipal = DecryptJWT(jwtToken);
                await _userRepository.SaveTokenJWT(jwtToken, claimsPrincipal);
                var expireClaimValue = claimsPrincipal.getExpiredClaim();

                if (expireClaimValue.HasValue)
                {
                    DateTime expireDateTime = expireClaimValue.Value;

                    var LoginResult = new LoginResult()
                    {
                        Token = jwtToken,
                        Expired = expireDateTime,
                        Result = true,
                    };
                    return LoginResult;
                }
                else
                {
                    return new LoginResult { Result = false, Errors = new List<string> { "เกิดข้อผิดพลาด" } };
                }
            }
            return new LoginResult() { Result = false, Errors = new List<string> { "ชื่อหรือรหัสผ่านไม่ถูกต้อง" } };
        }

        public async Task<LoginResult> RegisterAsync(RegisterRequest model)
        {
            var existingU = await _userRepository.GetUserByUsernameAsync(model.Username);
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null || existingU != null)
            {
                return new LoginResult { Result = false, Errors = new List<string> { "มีผู้ใช้รายนี้แล้ว" } };
            }
            else
            {
                //string generatedUsername = UsernameGenerator.GenerateUsername(model.FirstName, model.LastName, model.Email);
                var newUser = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserTel = model.UserTel,
                    Gender = model.Gender,
                    InsertDate = DateTime.UtcNow,
                    InsertBy = "System",
                    UpdateDate = DateTime.UtcNow,
                    UpdateBy = "System",
                    RoleId = 16
                };

                string salt = GetSalt();
                newUser.Salt = salt;
                string hashedPassword = CalculateSHA256Hash(model.Password + salt);
                newUser.Password = hashedPassword;

                await _userRepository.SaveUserData(newUser);
                return new LoginResult { Result = true };
            }
        }

        public async Task<LoginResult> WaitForIdentify(string email)
        {
            //จริงๆแม่งไม่ควรใช้ซ้ำกันหรอก มันควรใส่ ServiceRes<string> มากกว่า แต่นั่นแหละ สังขารมาได้แค่นี้
            var user = await _userRepository.GetUserByEmailAsync(email)
                         ?? await _userRepository.GetUserByUsernameAsync(email); //ใส่หาโดยUsername กรณี login
            if (user == null)
            {
                return new LoginResult
                {
                    Result = false,
                    Errors = new List<string>() { "User not found." }
                };
            }

            string otpcode = GenerateRandomOtp();
            string jti = Guid.NewGuid().ToString();
            DateTime expiryTime = DateTime.Now.AddDays(5);
            var Otp = new OtpEntity { OtpCode = otpcode, Jti = jti, UserId = user.UserId, ExpiryTime = expiryTime };

            if (await _otpRepository.SaveOtp(Otp))
            {
                var MailData = new MailData
                {
                    EmailToId = user.Email!,
                    EmailSubject = "ยืนยันตัวตน - OTP",
                    EmailBody = $"รหัสสำหรับยืนยันตัวตนคือ: {otpcode}",
                    EmailToName = user.Username!

                };
                if (await _mailService.SendMailAsync(MailData))
                {

                    return new LoginResult
                    {
                        Token = GenerateOtptoken(jti),
                        Result = true,
                        Expired = DateTime.Now.AddDays(1-5),
                        Errors = new List<string>() { "ส่ง Otp ไปที่emailท่านสำเร็จ; อีกเพียงขั้นตอนเดียวก็จะเสร็จสมบูรณ์" }
                    };
                }

            }
            return new LoginResult { Result = false, Errors = new List<string>() { "ส่ง otp ไม่สำเร็จ" } };
        }

        public async Task<ServiceResponse<bool>> VerifyIdentity(string jti, string otpCode)
        {
            var otp = await _otpRepository.GetOtpByJti(jti);
            if (otp.OtpCode == otpCode)
            {
                var user = await _userRepository.GetUserById(otp.UserId);

                otp.ExpiryTime = DateTime.Now;
                if (await _otpRepository.DisableOtp(otp))
                {
                    user.RoleId = 4;

                    await _userRepository.UpdateUserData(user);
                    return new ServiceResponse<bool> { Data = true, Success = true, Message = $"ยืนยันตัวตนสำเร็จ!!!" };
                }
            }

            return new ServiceResponse<bool> { Data = false, Success = false, Message = "รหัสยืนยันตัวตนไม่ถูกต้อง" };
        }


        public async Task<ServiceResponse<bool>> CreateEmployee(RegisterRequest model)
        {
            var existingU = await _userRepository.GetUserByUsernameAsync(model.Username);
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null || existingU != null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "บัญชีนี้เคยสร้างไปแล้ว"
                };
            }
            else
            {
                var newEmployee = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserTel = model.UserTel,
                    Gender = model.Gender,
                    InsertDate = DateTime.UtcNow,
                    InsertBy = "Admin",
                    UpdateDate = DateTime.UtcNow,
                    UpdateBy = "Admin",
                    RoleId = 6
                };

                string salt = GetSalt();
                newEmployee.Salt = salt;
                string hashedPassword = CalculateSHA256Hash(model.Password + salt);
                newEmployee.Password = hashedPassword;

                await _userRepository.SaveUserData(newEmployee);
                return new ServiceResponse<bool>
                {
                    Data = true,
                    Success = true,
                    Message = "สร้างบัญชีพนักงานสำเร็จ"
                };
            }
        }


        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            if (VerifyPassword(oldPassword, user.Password!, user.Salt!))
            {
                string newSalt = GetSalt();
                string hashedNewPassword = CalculateSHA256Hash(newPassword + newSalt);

                user.Password = hashedNewPassword;
                user.Salt = newSalt;

                if (await _userRepository.UpdateUserData(user))
                {
                    return new ServiceResponse<bool> { Data = true, Success = true, Message = "Password has been changed." };
                }
            }

            return new ServiceResponse<bool> { Data = false, Message = "Password has not changed successfully" };

        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            string otpcode = GenerateRandomOtp();
            string jti = Guid.NewGuid().ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(5);
            var Otp = new OtpEntity { OtpCode = otpcode, Jti = jti, UserId = user.UserId, ExpiryTime = expiryTime };

            if (await _otpRepository.SaveOtp(Otp))
            {
                var MailData = new MailData
                {
                    EmailToId = email!,
                    EmailSubject = "Forgot Password - OTP",
                    EmailBody = $"Your OTP for password reset is: {otpcode}",
                    EmailToName = user.Username!

                };
                if (await _mailService.SendMailAsync(MailData))
                {
                    
                    return new ServiceResponse<string>
                    {
                        Data = GenerateOtptoken(jti),
                        Success = true,
                        Message = "ส่ง Otp ไปที่emailท่านสำเร็จ; Otp ของท่านจะหมดอายุใน 5 นาที"
                    };
                }

            }
            return new ServiceResponse<string> { Success = false, Message = "ส่ง otp ไม่สำเร็จ" };
        }

        public async Task<ServiceResponse<bool>> VerifyOtp(string jti, string otpCode)
        {
            var otp = await _otpRepository.GetOtpByJti(jti);
            if (otp.OtpCode == otpCode)
            {
                var user = await _userRepository.GetUserById(otp.UserId);

                otp.ExpiryTime = DateTime.Now;
                if(await _otpRepository.DisableOtp(otp))
                {
                    string salt = GetSalt();
                    string newPassword = GenerateRandomOtp();
                    string hashedPassword = CalculateSHA256Hash(newPassword + salt);

                    user.Salt = salt;
                    user.Password = hashedPassword;

                    await _userRepository.UpdateUserData(user);
                    return new ServiceResponse<bool> { Data = true, Success = true, Message = $"รหัสผ่านใหม่ของท่านคือ: {newPassword} กรุณาดำเนินการเปลี่ยนรหัสในภายหลัง" };
                }
            }

            return new ServiceResponse<bool> { Data = false, Success = false, Message="Otpของท่านไม่ถูกต้อง / Otpหมดอายุ" };
        }

        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var calculatedHash = CalculateSHA256Hash(password + salt);
            return calculatedHash == hashedPassword;
        }

        public string CalculateSHA256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("x2")); // Convert to hexadecimal representation
                }
                return sb.ToString();
            }
        }

        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public string GenerateJWT(LoginDb loginDb)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginDb.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, loginDb.Username!),
                new Claim(JwtRegisteredClaimNames.Email, loginDb.Email!),
                new Claim(ClaimTypes.Role, loginDb.RoleName),
                //new Claim(ClaimTypes.Role, "testRole"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        public ClaimsPrincipal DecryptJWT(string jwtToken)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:SecretKey")!);

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateIssuer = false,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = false,
                TokenDecryptionKey = new SymmetricSecurityKey(key),
            };

            try
            {
                var claimsPrincipal = jwtHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateRandomOtp()
        {
            // Generate a random 6-digit OTP for simplicity
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }

        private string GenerateOtptoken(string jti)
        {
            List<Claim> claims = new List<Claim>
            {          
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

            var OtpToken = new JwtSecurityTokenHandler().WriteToken(token);

            return OtpToken;
        }
    }
}
