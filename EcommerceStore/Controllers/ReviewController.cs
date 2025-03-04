using System.ComponentModel;
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
    public class ReviewController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet]
        [Description("Get all reviews in the database")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _eStoreDbContext.Reviews.ToListAsync();
                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Get all reviews for a product")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            try
            {
                var reviews = await _eStoreDbContext.Reviews
                    .Include(r => r.Product.Name)
                    .Where(r => r.ProductId == productId)
                    .ToListAsync();
                if (reviews.Count == 0) return NotFound("No reviews found for the product.");
                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Get review by userId")]
        public async Task<IActionResult> GetReviewsByUserId(int userId)
        {
            try
            {
                var reviews = await _eStoreDbContext.Reviews
                    .Include(r => r.Product.Name)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
                if (reviews.Count == 0) return NotFound("No reviews found from the customer.");
                return Ok(reviews);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Get review by id")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            try
            {
                var review = await _eStoreDbContext.Reviews
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (review == null) return NotFound("Review not found.");
                return Ok(review);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Description("Create a new review")]
        public async Task<IActionResult> CreateReview(ReviewDto reviewDto)
        {
            try
            {
                var newReview = new Review
                {
                    ProductId = reviewDto.ProductId,
                    UserId = reviewDto.UserId,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment
                };
                await _eStoreDbContext.Reviews.AddAsync(newReview);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Thank you for giving a product review.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        [Description("Update a review")]
        public async Task<IActionResult> UpdateReview(int id, ReviewDto reviewDto)
        {
            try
            {
                var review = await _eStoreDbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
                if (review == null) return NotFound("Review not found.");
                review.Rating = reviewDto.Rating;
                review.Comment = reviewDto.Comment;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Review updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Description("Delete a review")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _eStoreDbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
                if (review == null) return NotFound("Review not found.");
                _eStoreDbContext.Reviews.Remove(review);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Review has been successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
