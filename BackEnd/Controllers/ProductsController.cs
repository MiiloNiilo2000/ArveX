using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts(){
            var products = await _context.Product.ToListAsync();

            if(!products.Any()){
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById(int id){
            var products = await _context.Product.FindAsync(id);

            if(products == null){
            return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var product = _context.Product.FirstOrDefault(x => x.ProductId == id);

            if (product == null){
                return NotFound();
            }
            _context.Product.Remove(product);

            _context.SaveChanges();

            return NoContent();
        }
         [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.ProductId)
            {
                return BadRequest("Product ID mismatch");
            }

            var existingProduct = await _context.Product.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update the properties of the existing product
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.CompanyId = updatedProduct.CompanyId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Product.Any(e => e.ProductId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
    }
}