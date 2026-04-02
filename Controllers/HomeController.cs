using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOPVN.Data;

namespace SHOPVN.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _db.Categories.ToListAsync();
        ViewBag.FlashProducts = await _db.Products
            .Where(p => p.IsFlashSale && p.IsActive)
            .Take(4).ToListAsync();
        ViewBag.FeaturedProducts = await _db.Products
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8).ToListAsync();

        // Số lượng giỏ hàng
        if (User.Identity!.IsAuthenticated)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            ViewBag.CartCount = await _db.CartItems.Where(c => c.UserId == userId).SumAsync(c => c.Quantity);
        }
        return View();
    }
}