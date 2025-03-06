using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required int OrderId { get; set; }
        public Order Order { get; set; }
        public required int ProductId { get; set; }
        public Product Product { get; set; }
        public required int Quantity { get; set; }
        [Precision(18, 2)]
        public required decimal Price { get; set; }
    }
}
