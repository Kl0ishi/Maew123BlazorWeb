namespace Maew123.Api.Repositories.Contracts
{
    public interface IStatusRepository
    {
        Task<Status> GetStatus(int StatusId);
    }
}
