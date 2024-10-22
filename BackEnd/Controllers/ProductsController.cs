using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProductsController(ApplicationDbContext context){
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(){
            var products = await _context.Product.ToListAsync();

            if(!products.Any()){
                return NotFound();
            }
            return Ok(products);
        }
    [HttpPost]
    public ActionResult<Product> PostProduct(Product product)
    {
        _context.Product!.Add(product);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }
    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteProduct([FromRoute] int id){
        var product = _context.Product.FirstOrDefault(x => x.Id == id);

        if (product == null){
            return NotFound();
        }

        _context.Product.Remove(product);

        _context.SaveChanges();

        return NoContent();
    }
}