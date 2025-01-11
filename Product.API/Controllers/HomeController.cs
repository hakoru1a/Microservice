using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("")]  // This maps to the root URL
    public class HomeController : ControllerBase
    {
        [HttpGet]  // Handles GET requests
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}