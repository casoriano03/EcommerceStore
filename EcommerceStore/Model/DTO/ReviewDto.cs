namespace EcommerceStore.Model.DTO
{
    public class ReviewDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
