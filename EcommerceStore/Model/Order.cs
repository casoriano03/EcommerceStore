using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Model
{
    public class Order
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; }
        public List<Cart> OrderItems { get; set; } = new List<Cart>();
        public required string Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Precision(18,2)]
        public required decimal Total { get; set; }
    }
}
