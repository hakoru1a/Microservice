using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("")]  
    public class HomeController : ControllerBase
    {
        [HttpGet]  // Handles GET requests
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}