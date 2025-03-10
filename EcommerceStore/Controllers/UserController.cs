using System.ComponentModel;
using EcommerceStore.Interface;
using EcommerceStore.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IEStoreDbContext eStoreDbContext) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;

        [HttpGet("GetUsers")]
        [Description("Get all users in the database")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _eStoreDbContext.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("GetUserById{id}")]
        [Description("Get a user by id in the database")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) return NotFound("No user found with the id provided.");
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("UpdateUser{id}")]
        [Description("Update a user in the database")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) return NotFound("No user found with the id provided.");

                user.Email = updateUserDto.Email;
                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Role = updateUserDto.Role;

                await _eStoreDbContext.SaveChangesAsync();
                return Ok("User updated successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("DeleteUser{id}")]
        [Description("Delete a user in the database")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) return NotFound("No user found with the id provided.");

                _eStoreDbContext.Users.Remove(user);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("User deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
