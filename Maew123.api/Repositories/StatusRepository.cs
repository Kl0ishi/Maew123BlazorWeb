using Maew123.Api.Models;

namespace Maew123.Api.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ItshopMaew123Context _dbContext;

        public StatusRepository(ItshopMaew123Context dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Status> GetStatus(int StatusId)
        {
            var Status = await _dbContext.Statuses.FindAsync(StatusId);
            return Status!;
        }
    }
}
