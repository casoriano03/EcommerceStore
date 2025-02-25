using EcommerceStore.Interface;
using EcommerceStore.Model;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Data
{
    public class EStoreDbContext: DbContext, IEStoreDbContext
    {
        public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
    }
}
