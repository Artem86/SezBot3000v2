using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SezBot3000v2.Services;
using Telegram.Bot.Types;

namespace SezBot3000v2.Controllers
{
    ///[Produces("application/json")]
    ///[Route("api/Update")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService _updateService;

        public UpdateController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Message([FromBody]Update update)
        {
            await _updateService.EchoAsync(update);
            return Ok();
        }
    }
}