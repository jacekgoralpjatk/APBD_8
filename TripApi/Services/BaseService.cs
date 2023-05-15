using TripApi.Models;

namespace TripApi.Services {

    public abstract class BaseService {

        protected readonly ApbdDbContext _dbContext;

        public BaseService(ApbdDbContext dbContext) {
            _dbContext = dbContext;
        }

    }
}
