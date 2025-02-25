using EcommerceStore.Interface;
using EcommerceStore.Model;
using EcommerceStore.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _eStoreDbContext.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _eStoreDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null) return NotFound("No category found with the id provided.");
                return Ok(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                Category newCategory = new Category
                {
                    Name = categoryDto.Name,
                    ImageLink = categoryDto.ImageLink
                };
                await _eStoreDbContext.Categories.AddAsync(newCategory);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Category {newCategory.Name} added successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            try
            {
                var category = await _eStoreDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null) return NotFound("No category found with the id provided.");
                category.Name = categoryDto.Name;
                category.ImageLink = categoryDto.ImageLink;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Category {category.Name} updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _eStoreDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null) return NotFound("No category found with the id provided.");
                _eStoreDbContext.Categories.Remove(category);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Category {category.Name} deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
