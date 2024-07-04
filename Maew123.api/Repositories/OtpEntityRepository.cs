using Maew123.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace Maew123.Api.Repositories
{
    public class OtpEntityRepository : IOtpEntityRepository
    {
        private readonly ItshopMaew123Context _dbContext;

        public OtpEntityRepository(ItshopMaew123Context DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<bool> SaveOtp(OtpEntity otpEntity)
        {
            try
            {
                _dbContext.OtpEntities.Add(otpEntity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                
            }
        }

        public async Task<OtpEntity> GetOtpByJti(string OtpJti)
        {
            var currentTime = DateTime.Now;

            var Otp = await _dbContext.OtpEntities
                .Where(o => o.Jti == OtpJti && o.ExpiryTime > currentTime)
                .FirstOrDefaultAsync();

            return Otp;
        }

        public async Task<bool> DisableOtp(OtpEntity otpEntity)
        {
            _dbContext.OtpEntities.Update(otpEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
