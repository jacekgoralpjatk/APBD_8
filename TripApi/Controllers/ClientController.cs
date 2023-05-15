using Microsoft.AspNetCore.Mvc;
using TripApi.Services;

namespace TripApi.Controllers {

    [ApiController]
    public class ClientController : Controller {

        private readonly IClientService clientService;

        public ClientController(IClientService clientService) {
            this.clientService = clientService;
        }


        [HttpDelete]
        [Route("api/clients/{idClient}")]
        public async Task<ActionResult> DeleteClient(int idClient) {
            Console.WriteLine("asdf");
            var res = await clientService.DeleteClient(idClient);
            Console.WriteLine("qwe");
            return res;
        }

    }

}
