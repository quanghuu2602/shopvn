using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SHOPVN.Models;

namespace SHOPVN.Data
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Product>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");

            mb.Entity<Product>()
              .Property(p => p.OriginalPrice)
              .HasColumnType("decimal(18,2)");

            mb.Entity<Order>()
              .Property(o => o.TotalAmount)
              .HasColumnType("decimal(18,2)");

            mb.Entity<Order>()
              .Property(o => o.Discount)
              .HasColumnType("decimal(18,2)");

            mb.Entity<OrderItem>()
              .Property(o => o.UnitPrice)
              .HasColumnType("decimal(18,2)");

            // Seed Categories
            mb.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Giày dép", Icon = "👟" },
                new Category { Id = 2, Name = "Công nghệ", Icon = "💻" },
                new Category { Id = 3, Name = "Đồng hồ", Icon = "⌚" },
                new Category { Id = 4, Name = "Âm thanh", Icon = "🎧" },
                new Category { Id = 5, Name = "Thời trang", Icon = "👔" },
                new Category { Id = 6, Name = "Gaming", Icon = "🎮" }
            );

            // Seed Products
            mb.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nike Air Max 270",
                    Brand = "Nike",
                    Description = "Giày thể thao cao cấp",
                    Price = 1290000,
                    OriginalPrice = 1590000,
                    Stock = 50,
                    ImageEmoji = "👟",
                    Badge = "sale",
                    CategoryId = 1,
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Sony WH-1000XM5",
                    Brand = "Sony",
                    Description = "Tai nghe chống ồn",
                    Price = 7490000,
                    OriginalPrice = 8990000,
                    Stock = 30,
                    ImageEmoji = "🎧",
                    Badge = "sale",
                    CategoryId = 4,
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "iPhone 15 Pro Max",
                    Brand = "Apple",
                    Description = "Flagship Apple 2024",
                    Price = 34990000,
                    Stock = 20,
                    ImageEmoji = "📱",
                    Badge = "hot",
                    CategoryId = 2,
                    IsActive = true,
                    IsFlashSale = true,
                    FlashSaleSold = 73,
                    FlashSaleTotal = 100
                },
                new Product
                {
                    Id = 4,
                    Name = "Casio G-Shock",
                    Brand = "Casio",
                    Description = "Đồng hồ thể thao bền bỉ",
                    Price = 2150000,
                    Stock = 40,
                    ImageEmoji = "⌚",
                    Badge = "new",
                    CategoryId = 3,
                    IsActive = true
                },
                new Product
                {
                    Id = 5,
                    Name = "Dell XPS 13 Plus",
                    Brand = "Dell",
                    Description = "Laptop mỏng nhẹ cao cấp",
                    Price = 28500000,
                    OriginalPrice = 32000000,
                    Stock = 15,
                    ImageEmoji = "💻",
                    Badge = "sale",
                    CategoryId = 2,
                    IsActive = true
                },
                new Product
                {
                    Id = 6,
                    Name = "Keychron K2 Pro",
                    Brand = "Keychron",
                    Description = "Bàn phím cơ không dây",
                    Price = 1890000,
                    OriginalPrice = 2490000,
                    Stock = 60,
                    ImageEmoji = "⌨️",
                    Badge = "sale",
                    CategoryId = 2,
                    IsActive = true,
                    IsFlashSale = true,
                    FlashSaleSold = 89,
                    FlashSaleTotal = 100
                }
            );
        }
    }
}