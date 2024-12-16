using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Update(int id, [FromBody] Product product){
            bool result = await _context.UpdateProduct(id, product);
            return result ? NoContent() : NotFound();
        }
    }
}