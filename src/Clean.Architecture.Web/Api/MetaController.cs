using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clean.Architecture.Web.Api
{
    public class MetaController : BaseApiController
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate}");
        }
    }
}
