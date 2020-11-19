

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AidaCarParts.Controllers.api
{
    public class SpaController : Controller
    {
        // Enumerate all SPA urls so we boot the Vue app correctly
        // Alternatively, use this as the 404 page so any unhandled url will boot the vue app
        [HttpGet("/")]
        [HttpGet("/about")]
        public async Task<IActionResult> Index() => File("/index.html", "text/html");
    }
}
