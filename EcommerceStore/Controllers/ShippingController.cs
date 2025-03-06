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
    public class ShippingController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet]
        [Description("Get all shipped orders or for shipping orders in the database")]
        public async Task<IActionResult> GetAllShippingMethods()
        {
            try
            {
                var shippingMethods = await _eStoreDbContext.Shippings.ToListAsync();
                return Ok(shippingMethods);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Get shipped order by id")]
        public async Task<IActionResult> GetOrderShippedById(int id)
        {
            try
            {
                var orderShipped = await _eStoreDbContext.Shippings
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (orderShipped == null) return NotFound("Order shipped not found.");
                return Ok(orderShipped);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("{id}")]
        [Description("Get shipped order by orderId")]
        public async Task<IActionResult> GetOrderShippedByOrderId(int orderId)
        {
            try
            {
                var orderShipped = await _eStoreDbContext.Shippings
                    .FirstOrDefaultAsync(s => s.OrderId == orderId);
                if (orderShipped == null) return NotFound("Order shipped not found.");
                return Ok(orderShipped);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Description("Get shipped orders by userId")]
        public async Task<IActionResult> GetOrderShippedByUserId(int userId)
        {
            try
            {
                var orderShipped = await _eStoreDbContext.Shippings
                    .Include(s => s.Order)
                    .Where(s => s.Order.UserId == userId)
                    .ToListAsync();
                if (orderShipped.Count == 0) return NotFound("No orders shipped found for the customer.");
                return Ok(orderShipped);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Description("Create a new shipping order")]
        public async Task<IActionResult> CreateShippingOrder(ShippingDto shippingDto)
        {
            try
            {
                var shippingOrder = new Shipping
                {
                    OrderId = shippingDto.OrderId,
                    Name = shippingDto.Name,
                    Address = shippingDto.Address,
                    ZipCode = shippingDto.ZipCode,
                    City = shippingDto.City,
                    Phone = shippingDto.Phone,
                    Status = "Pending",
                    UpdatedDate = DateTime.Now
                };
                await _eStoreDbContext.Shippings.AddAsync(shippingOrder);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok(shippingOrder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        [Description("Update shipping order status")]
        public async Task<IActionResult> UpdateShippingOrderStatus(int id, string status)
        {
            try
            {
                var shippingOrder = await _eStoreDbContext.Shippings
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (shippingOrder == null) return NotFound("Shipping order not found.");
                shippingOrder.Status = status;
                shippingOrder.UpdatedDate = DateTime.Now;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Shipping order status updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        [Description("Update shipping order")]
        public async Task<IActionResult> UpdateShippingOrder(int id, ShippingDto shippingDto)
        {
            try
            {
                var shippingOrder = await _eStoreDbContext.Shippings
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (shippingOrder == null) return NotFound("Shipping order not found.");
                shippingOrder.Name = shippingDto.Name;
                shippingOrder.Address = shippingDto.Address;
                shippingOrder.ZipCode = shippingDto.ZipCode;
                shippingOrder.City = shippingDto.City;
                shippingOrder.Phone = shippingDto.Phone;
                shippingOrder.UpdatedDate = DateTime.Now;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Shipping order updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Description("Delete shipping order")]
        public async Task<IActionResult> DeleteShippingOrder(int id)
        {
            try
            {
                var shippingOrder = await _eStoreDbContext.Shippings
                    .FirstOrDefaultAsync(s => s.Id == id);
                if (shippingOrder == null) return NotFound("Shipping order not found.");
                _eStoreDbContext.Shippings.Remove(shippingOrder);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Shipping order has been successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
