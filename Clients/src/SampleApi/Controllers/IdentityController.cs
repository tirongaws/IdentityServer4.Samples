using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SampleApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            var user = this.User;
            var result = User.Claims.Select(
                c => new { c.Type, c.Value });
            return new JsonResult(result);
        }
    }
}