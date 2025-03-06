using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Precision(18,2)]
        public required decimal Price { get; set; }
        public required int Inventory { get; set; }
        public required string ImageUrl { get; set; } = string.Empty;
        public required int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
