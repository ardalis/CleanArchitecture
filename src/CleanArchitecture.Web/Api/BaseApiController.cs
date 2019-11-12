using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}
