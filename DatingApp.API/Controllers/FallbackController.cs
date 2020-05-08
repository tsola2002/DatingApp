using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    // get controller that has view support
    public class FallbackController : Controller
    {
        // we call it index because it will match wat we called it in out configuration
        public IActionResult Index()
        {
            // we return a physical file and use combine to build the path
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}