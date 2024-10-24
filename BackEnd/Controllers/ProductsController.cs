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

            if(products == null || !products.Any()){
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
}