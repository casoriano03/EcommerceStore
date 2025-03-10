using EcommerceStore.Data;
using EcommerceStore.Interface;
using EcommerceStore.Model;
using EcommerceStore.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet("GetAllProducts")]
        [Description("Get all products in the database")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _eStoreDbContext.Products
                    .Include(p => p.Category)
                    .ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetProductById{id}")]
        [Description("Get a product by id in the database")]

        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _eStoreDbContext.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return NotFound("No product found with the id provided.");
                
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("AddProduct")]
        [Description("Get a product by id in the database")]

        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Inventory = productDto.Inventory,
                    ImageUrl = productDto.ImageUrl,
                    CategoryId = productDto.CategoryId
                };
                await _eStoreDbContext.Products.AddAsync(product);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Product {product.Name} has been successfully added.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("UpdateProduct{id}")]
        [Description("Edit a product by id in the database")]

        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                var product = await _eStoreDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) return NotFound("No product found with the id provided.");
                
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.Inventory = productDto.Inventory;
                product.ImageUrl = productDto.ImageUrl;
                product.CategoryId = productDto.CategoryId;
             
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Product {product.Name} has been successfully updated.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("DeleteProduct{id}")]
        [Description("Delete a product by id in the database")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _eStoreDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) return NotFound("No product found with the id provided.");
                
                _eStoreDbContext.Products.Remove(product);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Product {product.Name} has been successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
