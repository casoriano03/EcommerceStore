namespace EcommerceStore.Interface;

public interface IEmailService
{
    void SendEmail(string firstName, string lastName, string recipientEmail, string message);
    string ChangePasswordMessage(string firstName, string password);
    string EmailConfirmationMessage(string firstName, string email);
}