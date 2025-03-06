using EcommerceStore.Model;

namespace EcommerceStore.Interface;

public interface IAuthService
{
    string GenerateToken(User user);
    string GenerateRandomPassword();
}