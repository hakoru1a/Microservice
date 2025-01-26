using Constracts.Indentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase   
    {
        private ITokenService _tokenService;
        public TokenController(ITokenService tokenService) 
        { 
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetToken()
        {
            return Ok(_tokenService.GetToken(new Shared.DTOs.Identity.TokenRequest()));
        }
    }
}
