namespace EcommerceStore.Model
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string ImageLink { get; set; } = string.Empty;
    }
}
