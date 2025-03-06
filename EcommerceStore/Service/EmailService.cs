using EcommerceStore.Interface;
using EcommerceStore.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EcommerceStore.Service
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly string _confirmationLink = "https://localhost:7023/api/Auth/Confirm_Email";
        public void SendEmail(string firstName, string lastName, string recipientEmail,
            string message)
        {
            var name = firstName + " " + lastName;
            var fromName = configuration["EmailSettings:FromName"];
            var fromEmail = configuration["EmailSettings:FromEmail"];
            var subject = configuration["EmailSettings:Subject"];
            var server = configuration["EmailSettings:SmtpServer"];
            var port = configuration.GetValue<int>("EmailSettings:SmtpPort");
            var password = configuration["EmailSettings:Password"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            emailMessage.To.Add(new MailboxAddress(name, recipientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = message
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

        public string ChangePasswordMessage(string firstName, string password)
        {
            return $"Hi {firstName},\n\nHere is your randomly generated password: {password}\n\nPlease change it after logging in.\n\nBest regards,\nEcommerce Store Team";
        }

        public string EmailConfirmationMessage(string firstName, string email)
        {
            return
                $"Hi {firstName},\n\nPlease confirm your email address by clicking the button below:\n\n" +
                $"<form action={_confirmationLink} method='POST'>" +
                $"<input type='hidden' name='email' value='{email}' />" +
                $"<button type='submit' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #ffffff; background-color: #007bff; border: none; text-decoration: none; border-radius: 5px; cursor: pointer;'>Confirm Email</button>" +
                $"</form>\n\n" +
                "If you didn't sign up, you can ignore this message.\n\nBest regards,\nEcommerce Store Team";
        }
    }
}
