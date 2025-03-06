namespace EcommerceStore.Model
{
    public class Review
    {
        public int Id { get; set; }
        public required int ProductId { get; set; }
        public Product Product { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; }
        public required int Rating { get; set; }
        public required string Comment { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
