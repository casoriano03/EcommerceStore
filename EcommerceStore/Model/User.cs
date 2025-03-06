namespace EcommerceStore.Model
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PasswordHashed { get; set; } = string.Empty;
        public required string Role { get; set; } = "Customer";
        public required bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
