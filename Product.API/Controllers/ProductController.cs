using Constracts.Common.Interface;
using Microsoft.AspNetCore.Mvc;
using Product.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Repository.Interface;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _repository.FindAll().ToListAsync();
            return Ok(result);
        }
    }
}