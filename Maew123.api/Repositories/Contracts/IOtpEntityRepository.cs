namespace Maew123.Api.Repositories.Contracts
{
    public interface IOtpEntityRepository
    {
        Task<bool> SaveOtp(OtpEntity otpEntity);
        Task<OtpEntity> GetOtpByJti(string OtpJti);
        Task<bool> DisableOtp(OtpEntity otpEntity);
    }
}
