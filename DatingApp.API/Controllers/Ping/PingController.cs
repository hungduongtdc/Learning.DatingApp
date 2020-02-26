using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers.Ping
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PingController : ControllerBaseCommon
    {
        public PingController()
        {
        }
        [HttpGet("dummy-anonymous")]
        [AllowAnonymous]
        public async Task<bool> GetDummyAnonymous()
        {
            await Task.Delay(1);
            return true;
        }
        [HttpGet("dummy-authorize")]
        public async Task<bool> GetDummyAuthorize()
        {
            await Task.Delay(1);
            return true;
        }
    }
}