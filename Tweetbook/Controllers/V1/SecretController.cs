using Microsoft.AspNetCore.Mvc;
using Tweetbook.Filters;

namespace Tweetbook.Controllers.V1.PostsController
{
    [ApiKeyAuth]
    public class SecretController : ControllerBase
    {
        [HttpGet("seret")]
        public IActionResult GetSecret()
        {
            return Ok("I have no secrets");
        }
    }
}