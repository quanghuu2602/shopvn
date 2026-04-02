using System.Collections;
using Microsoft.AspNetCore.Identity;
namespace SHOPVN.Models
{
    public class ApplicationUser: IdentityUser<int>
    {
        public string FirstName { get; set; } = " ";
        public string LastName { get; set; } = " ";
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
