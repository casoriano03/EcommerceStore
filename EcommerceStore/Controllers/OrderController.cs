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
    public class OrderController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet("GetAllOrders")]
        [Description("Get all orders in the database")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _eStoreDbContext.Orders
                    .Include(o => o.OrderItems)
                    .ToListAsync();
                return Ok(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetOrdersByUserId{id}")]
        [Description("Get all orders for a user")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _eStoreDbContext.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
                if (orders.Count == 0) return NotFound("No orders found from the customer.");
                return Ok(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetOrderById{id}")]
        [Description("Get order by id")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _eStoreDbContext.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("No order found with the id provided.");
                return Ok(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("CreateOrder")]
        [Description("Create a new order")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            try
            {
                var cartItems = await _eStoreDbContext.Carts
                    .Include(c => c.Product)
                    .Where(c => c.UserId == orderDto.UserId)
                    .ToListAsync();
                if (cartItems.Count == 0) return NotFound("No cart items found for the user.");

                var order = new Order
                {
                    UserId = orderDto.UserId,
                    OrderItems = cartItems,
                    Status = "Pending",
                    Total = orderDto.Total
                };
                await _eStoreDbContext.Orders.AddAsync(order);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Successfully created a new order.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("UpdateOrderStatus{id}")]
        [Description("Update order status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, string newStatus)
        {
            try
            {
                var order = await _eStoreDbContext.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("No order found with the id provided.");
                order.Status = newStatus;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Order status has been updated.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("DeleteOrder{id}")]
        [Description("Delete order")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _eStoreDbContext.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("No order found with the id provided.");
                _eStoreDbContext.Orders.Remove(order);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Order has been successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
