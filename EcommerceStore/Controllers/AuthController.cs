using EcommerceStore.Interface;
using EcommerceStore.Model;
using EcommerceStore.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;


namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IEStoreDbContext eStoreDbContext, IAuthService authService, IEmailService emailService) : ControllerBase
    {
        private readonly IEStoreDbContext _eStoreDbContext = eStoreDbContext;
        private readonly IAuthService _authService = authService;
        private readonly IEmailService _emailService = emailService;

        [HttpPost("register")]
        [Description("Register a user in the database")]

        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            try
            {
                var user = new User
                {
                    Email = registrationDto.Email,
                    PasswordHashed = null,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    Role = "Customer",
                    IsEmailConfirmed = false
                };
                user.PasswordHashed = new PasswordHasher<User>().HashPassword(user, registrationDto.Password);
                await _eStoreDbContext.Users.AddAsync(user);
                await _eStoreDbContext.SaveChangesAsync();

                var message = _emailService.EmailConfirmationMessage(registrationDto.FirstName, registrationDto.Email);
                _emailService.SendEmail(registrationDto.FirstName, registrationDto.LastName, registrationDto.Email, message);
                return Ok("User registered successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("login")]
        [Description("Login a user in the database")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null) return NotFound("No user found with the credentials provided.");
                var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, loginDto.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Failed) return BadRequest("Invalid credentials provided");

                var token = _authService.GenerateToken(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("Random_Password")]
        [Description("Generate a random password for a user")]
        public async Task<IActionResult> GenerateRandomPassword(string userEmailInput)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmailInput);
                if (user == null) return NotFound("No user found with the email provided.");
               
                var password = _authService.GenerateRandomPassword();
                var message = _emailService.ChangePasswordMessage(user.FirstName, password);

                _emailService.SendEmail(user.FirstName, user.LastName, user.Email, message);

                user.PasswordHashed = new PasswordHasher<User>().HashPassword(user, password);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Please check your email for you random generated password "+ password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("Change_Password")]
        [Description("Change a user's password in the database")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Email == changePasswordDto.Email);
                if (user == null) return NotFound("No user found with the email provided.");
                var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, changePasswordDto.OldPassword);
                if (passwordVerificationResult == PasswordVerificationResult.Failed) return BadRequest("Invalid credentials provided");
                user.PasswordHashed = new PasswordHasher<User>().HashPassword(user, changePasswordDto.NewPassword);
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Password changed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("Confirm_Email")]
        [Description("Confirm a user's email in the database")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            try
            {
                var user = await _eStoreDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return NotFound("No user found with the email provided.");
                user.IsEmailConfirmed = true;
                await _eStoreDbContext.SaveChangesAsync();
                return Ok("Email confirmed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
