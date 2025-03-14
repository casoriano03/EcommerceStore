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
    public class WishlistController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet("GetAllWishlists")]
        [Description("Get all wishlists in the database")]
        public async Task<IActionResult> GetAllWishlists()
        {
            try
            {
                var wishlists = await _eStoreDbContext.Wishlists
                    .Include(w => w.Product)
                    .ToListAsync();
                return Ok(wishlists);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetWishlistById{id}")]
        [Description("Get wishlist by id")]
        public async Task<IActionResult> GetWishlistById(int id)
        {
            try
            {
                var wishlist = await _eStoreDbContext.Wishlists
                    .Include(w=> w.Product)
                    .FirstOrDefaultAsync(w => w.Id == id);
                if (wishlist == null) return NotFound("Wishlist not found.");
                return Ok(wishlist);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetWishlistByUserId{id}")]
        [Description("Get wishlist by userId")]
        public async Task<IActionResult> GetWishlistByUserId(int userId)
        {
            try
            {
                var wishlist = await _eStoreDbContext.Wishlists
                    .Include(w => w.Product)
                    .Where(w => w.UserId == userId)
                    .ToListAsync();
                if (wishlist == null) return NotFound("Wishlist not found.");
                return Ok(wishlist);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("AddProductToWishlist")]
        [Description("Add product to wishlist")]
        public async Task<IActionResult> AddProductToWishlist(WishlistDto wishlistDto)
        {
            try
            {
                var wishlist = new Wishlist
                {
                    UserId = wishlistDto.UserId,
                    ProductId = wishlistDto.ProductId
                };
                await _eStoreDbContext.Wishlists.AddAsync(wishlist);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok(wishlist);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("RemoveProductFromWishlist{id}")]
        [Description("Remove product from wishlist")]
        public async Task<IActionResult> RemoveProductFromWishlist(int id)
        {
            try
            {
                var wishlist = await _eStoreDbContext.Wishlists
                    .FirstOrDefaultAsync(w => w.Id == id);
                if (wishlist == null) return NotFound("Wishlist not found.");
                _eStoreDbContext.Wishlists.Remove(wishlist);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Product removed from wishlist.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
