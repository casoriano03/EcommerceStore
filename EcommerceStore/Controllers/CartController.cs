using EcommerceStore.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using EcommerceStore.Model;
using EcommerceStore.Model.DTO;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet]
        [Description("Get all carts in the database")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            try
            {
                var cartItems = await _eStoreDbContext.Carts
                    .Include(c => c.Product)
                    .Include(c => c.User)
                    .ToListAsync();
                return Ok(cartItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Gets cart for registered user")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            try
            {
                var cartItems = await _eStoreDbContext.Carts
                    .Include(c => c.Product)
                    .Include(c => c.User)
                    .Where(c => c.UserId == id)
                    .ToListAsync();
                if (cartItems.Count == 0) return NotFound("No cart item found with the id provided.");
                return Ok(cartItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Description("Adds item to cart")]
        public async Task<IActionResult> AddCartItem(CartDto cartDto)
        {
            try
            {
                var newCart = new Cart
                {
                    UserId = cartDto.UserId,
                    ProductId = cartDto.ProductId,
                    Quantity = cartDto.Quantity
                };
                await _eStoreDbContext.Carts.AddAsync(newCart);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Successfully created a new cart.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        [Description("Changes quantity of item in cart")]
        public async Task<IActionResult> AddProductToCart(int id, int newQuantity)
        {
            try
            {
                var cart = await _eStoreDbContext.Carts
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (cart == null) return NotFound("No cart found with the id provided.");
                cart.Quantity = newQuantity;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Changed quantity of {cart.Product.Name} to {newQuantity}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Description("Deletes item from cart")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            try
            {
                var cart = await _eStoreDbContext.Carts
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (cart == null) return NotFound("No cart found with the id provided.");
                _eStoreDbContext.Carts.Remove(cart);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok($"Successfully removed {cart.Product.Name} from cart.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
