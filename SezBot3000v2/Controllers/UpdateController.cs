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
        public IActionResult Message([FromBody]Update update)
        {
            _updateService.Update(update);
            return Ok();
        }

        //// uncomment for test purposes
        //public async Task<IActionResult> Test()
        //{
        //    await _updateService.Test();
        //    return Ok();
        //}
    }
}