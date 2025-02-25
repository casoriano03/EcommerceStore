using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Precision(18,2)]
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
