using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Maew123.Api.Utilities
{
    public static class ClaimsPrincipalUtils
    {
        public static string getUsernameClaim(this ClaimsPrincipal claimsPrincipal)
        {
            var usernameClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
            return usernameClaim!;
        }

        public static string getEmailClaim(this ClaimsPrincipal claimsPrincipal)
        {
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            return emailClaim!;
        }

        public static string getJtiClaim(this ClaimsPrincipal claimsPrincipal)
        {
            var jtiClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            return jtiClaim!;
        }

        public static DateTime? getIatClaim(this ClaimsPrincipal claimsPrincipal)
        {
            long iatClaimValue = long.Parse(claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Iat)?.Value!);
            DateTime issueDateUtc = DateTimeUtils.UnixTimestampToDateTime(iatClaimValue);
            DateTime issueDateGMT7 = issueDateUtc.DateUtcToGMT7();
            return issueDateGMT7!;
        }

        public static string getRoleNameClaim(this ClaimsPrincipal claimsPrincipal)
        {
            var roleNameClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            return roleNameClaim!;
        }

        public static DateTime? getExpiredClaim(this SecurityToken validatedToken)
        {
            var expireClaimValue = validatedToken?.ValidTo;
            return expireClaimValue!;
        }

        public static DateTime? getExpiredClaim(this ClaimsPrincipal claimsPrincipal)
        {
            long expireClaimValue = long.Parse(claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value!); //ดึงexpมาเป็น string
            DateTime expireDateUtc = DateTimeUtils.UnixTimestampToDateTime(expireClaimValue);
            DateTime expireDateGMT7 = expireDateUtc.DateUtcToGMT7();
            return expireDateGMT7;
        }

        public static string getExpiredClaimAsUnix(this ClaimsPrincipal claimsPrincipal)
        {
            var expireClaimValue = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value; //ดึงexpมาเป็น string

            return expireClaimValue;
        }
        
    }
}
