namespace EcommerceStore.Model
{
    public class Shipping
    {
        public int Id { get; set; }
        public required int OrderId { get; set; }
        public Order Order { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Address { get; set; } = string.Empty;
        public required string ZipCode { get; set; } = string.Empty;
        public required string City { get; set; } = string.Empty;
        public required string Phone { get; set; } = string.Empty;
        public required string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
