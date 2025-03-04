using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceStore.Interface;
using EcommerceStore.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using static EcommerceStore.Service.AuthService;

namespace EcommerceStore.Service
{
    public class AuthService(IConfiguration configuration) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

     
        
        public string GenerateRandomPassword()
        {
            var chars = _configuration["CharString:Key"];
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void SendChangePasswordEmail(string firstName, string lastName, string recipientEmail,
            string randomGeneratedPassword)
        {
            var name = firstName + " " + lastName;
            var fromName = _configuration["EmailSettings:FromName"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var subject = _configuration["EmailSettings:Subject"];
            var server = _configuration["EmailSettings:SmtpServer"];
            var port = _configuration.GetValue<int>("EmailSettings:SmtpPort");
            var password = _configuration["EmailSettings:Password"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            emailMessage.To.Add(new MailboxAddress(name, recipientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text =
                    $"Hi {firstName},\n\nHere is your randomly generated password: {randomGeneratedPassword}\n\nPlease change it after logging in.\n\nBest regards,\nEcommerce Store Team"
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    client.Connect(server, port, SecureSocketOptions.StartTls);
                    client.Authenticate(fromEmail, password);
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
