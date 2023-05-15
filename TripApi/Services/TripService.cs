using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TripApi.Models;
using Microsoft.EntityFrameworkCore;
using TripApi.DTOs;
using TripApi.Exceptions;
using TripApi.Mapper;

namespace TripApi.Services {

    public interface ITripService {

        Task<List<ResponseGetTripDTO>> GetTripList(CancellationToken cancellationToken);

        Task<ActionResult> AddClientToTrip(RequestAddClientToTripDTO requestDTO);
    }

    public class TripService : BaseService, ITripService {


        public TripService(ApbdDbContext dbContext) : base(dbContext) {
        }

        public async Task<List<ResponseGetTripDTO>> GetTripList(CancellationToken cancellationToken) {
            return await _dbContext
                .Trip
                .Include(t => t.CountryTrip)
                .ThenInclude(ct => ct.IdCountryNavigation)
                .Include(t => t.ClientTrip)
                .ThenInclude(ct => ct.IdClientNavigation)
                .AsSplitQuery()
                .Select(trip => new ResponseGetTripDTO {
                    Name = trip.Name,
                    Description = trip.Description,
                    DateFrom = trip.DateFrom,
                    DateTo = trip.DateTo,
                    MaxPeople = trip.MaxPeople,
                    Countries = trip.CountryTrip
                        .Select(ct => new CountryDTO {
                            Name = ct.IdCountryNavigation.Name
                        }).ToList(),
                    Clients = trip.ClientTrip
                        .Select(ct => new ClientDTO {
                            FirstName = ct.IdClientNavigation.FirstName,
                            LastName = ct.IdClientNavigation.LastName
                        }).ToList()
                })
                .OrderByDescending(x => x.DateFrom)
                .ToListAsync(cancellationToken);
        }

        public async Task<ActionResult> AddClientToTrip(RequestAddClientToTripDTO requestDTO) {
            var client = EFDTOMapper<RequestAddClientToTripDTO, Client>.GetMapper()
                .Map<Client>(requestDTO) ?? throw new TripException(ExceptionCode.REQUEST_BODY_NOT_VALID);

            if (!DoesClientExistByPesel(requestDTO.Pesel)) {
                using var clientService = new ClientService(_dbContext);
                client = await clientService.AddClient(client);

                if (client == null) {
                    throw new TripException(ExceptionCode.REQUEST_BODY_NOT_VALID);
                }
            }
            else {
                client = _dbContext.Client.Single(c => c.Pesel == requestDTO.Pesel);
            }

            if (!DoesTripExist(requestDTO.IdTrip)) {
                throw new TripException(ExceptionCode.TABLE_NOT_CONTAINS_BY_ID, new string[] { "trip", requestDTO.IdTrip.ToString() });
            }

            var clientTrip = new ClientTrip {
                IdClient = client.IdClient,
                IdTrip = requestDTO.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = requestDTO.PaymentDate
            };

            _dbContext.ClientTrip.Add(clientTrip);

            try {
                await _dbContext.SaveChangesAsync();
            }
            catch {
                throw new TripException(ExceptionCode.TABLE_ALREADY_CONTAINS, new[] { "ClientTrip", "IdTrip: " + requestDTO.IdTrip + ", IdClient: " + client.IdClient });
            }

            return new OkObjectResult(requestDTO);
        }


        private bool DoesTripExist(int idTrip) =>
            _dbContext.Trip.Any(t => t.IdTrip == idTrip);

        private bool IsClientPartOfTrip(int idClient, int idTrip) =>
            _dbContext.ClientTrip.Any(ct => ct.IdClient == idClient && ct.IdTrip == idTrip);

        private bool DoesClientExistByPesel(string pesel) =>
            _dbContext.Client.Any(c => c.Pesel == pesel);

    }

}
