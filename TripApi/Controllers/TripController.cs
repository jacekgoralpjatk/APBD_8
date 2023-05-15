using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripApi.DTOs;
using TripApi.Models;
using TripApi.Services;

namespace TripApi.Controllers {

    [ApiController]
    public class TripController : Controller {

        private readonly ITripService tripService;

        public TripController(ITripService tripService) {
            this.tripService = tripService;
        }


        [HttpGet]
        [Route("api/trips")]
        public async Task<ActionResult<List<ResponseGetTripDTO>>> GetTripList(CancellationToken cancellationToken) {
            return await tripService.GetTripList(cancellationToken);
        }

        [HttpPut]
        [Route("api/trips/{idTrip}/clients")]
        public async Task<ActionResult> AddClientToTrip([FromBody]RequestAddClientToTripDTO requestDto) {
            return await tripService.AddClientToTrip(requestDto);
        }

    }

}
