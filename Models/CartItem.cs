namespace SHOPVN.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Nên dùng int
        public int Quantity { get; set; }
        public int UserId { get; set; } // Giữ int nhưng phải đảm bảo ApplicationUser cũng dùng int
        public ApplicationUser User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
