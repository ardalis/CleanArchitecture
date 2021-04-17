using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Api
{
    /// <summary>
    /// If your API controllers will use a consistent route convention and the [ApiController] attribute (they should)
    /// then it's a good idea to define and use a common base controller class like this one.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}
