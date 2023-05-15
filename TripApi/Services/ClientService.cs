using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripApi.Exceptions;
using TripApi.Models;

namespace TripApi.Services {

    public interface IClientService {
        Task<ActionResult> DeleteClient(int idClient);
    }

    public class ClientService : BaseService, IClientService, IDisposable
    {

        public ClientService(ApbdDbContext dbContext) : base(dbContext) {
        }

        public async Task<Client> AddClient(Client client) {
            var maxId = await _dbContext.Client.MaxAsync(c => c.IdClient);
            client.IdClient = maxId + 1;

            try {
                await _dbContext.Client.AddAsync(client);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                throw new TripException(ExceptionCode.REQUEST_BODY_NOT_VALID);
            }
            catch {
                throw new TripException(ExceptionCode.DB_UPDATE_FAILED);
            }

            return client;
        }

        public async Task<ActionResult> DeleteClient(int idClient) {
            if (await _dbContext.ClientTrip.AnyAsync(ct => ct.IdClient == idClient))
                throw new TripException(ExceptionCode.CLIENT_PART_OF_TRIP);
            var client = await _dbContext.Client.FindAsync(idClient);
            if (client != null) {
                _dbContext.Client.Remove(client);
                await _dbContext.SaveChangesAsync();
            }

            return new OkResult();
        }

        public void Dispose()
        {
        }
    }

}
