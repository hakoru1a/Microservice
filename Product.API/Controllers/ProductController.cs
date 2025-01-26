using Constracts.Common.Interface;
using Microsoft.AspNetCore.Mvc;
using Product.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Repository.Interface;
using AutoMapper;
using Shared.DTOs.Product;
using Microsoft.AspNetCore.Authorization;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.FindAll().ToListAsync();
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.FindByCondition(p => p.Id == id)
                                                     .SingleOrDefaultAsync();
            if (product == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        // POST: api/products
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _repository.CreateAsync(product);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
        }

        // PUT: api/products/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _repository.FindByCondition(p => p.Id == id)
                                                    .SingleOrDefaultAsync();
            if (product == null)
                return NotFound();

            _mapper.Map(productDto, product);
            await _repository.UpdateProduct(product);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        // DELETE: api/products/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.FindByCondition(p => p.Id == id)
                                              .SingleOrDefaultAsync();
            if (product == null)
                return NotFound();

            await _repository.DeleteProduct(id);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}