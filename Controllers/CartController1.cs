using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SHOPVN.Data;
using SHOPVN.Models;

namespace SHOPVN.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        public CartController(AppDbContext db) { _db = db; }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var items = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
            ViewBag.Total = items.Sum(c => c.Product.Price * c.Quantity);
            return View(items);
        }

        // KHÔNG [Authorize] KHÔNG [ValidateAntiForgeryToken]
        // Tự kiểm tra đăng nhập bên trong
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
        {
            try
            {
                if (!User.Identity!.IsAuthenticated)
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });

                var userId = GetUserId();

                var product = await _db.Products.FindAsync(dto.ProductId);
                if (product == null)
                    return Json(new { success = false, message = "Không tìm thấy SP id=" + dto.ProductId });

                if (product.Stock < dto.Quantity)
                    return Json(new { success = false, message = "Hết hàng" });

                var existing = await _db.CartItems
                    .FirstOrDefaultAsync(c => c.UserId == userId
                                           && c.ProductId == dto.ProductId);
                if (existing != null)
                    existing.Quantity += dto.Quantity;
                else
                    _db.CartItems.Add(new CartItem
                    {
                        UserId = userId,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    });

                await _db.SaveChangesAsync();

                var count = await _db.CartItems
                    .Where(c => c.UserId == userId)
                    .SumAsync(c => c.Quantity);

                return Json(new { success = true, count });
            }
            catch (Exception ex)
            {
                // Hiện lỗi chi tiết
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQty([FromBody] UpdateQtyDto dto)
        {
            if (!User.Identity!.IsAuthenticated)
                return Json(new { success = false });

            var userId = GetUserId();
            var item = await _db.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == dto.CartItemId
                                       && c.UserId == userId);
            if (item == null)
                return Json(new { success = false });

            if (dto.Action == "increase") item.Quantity++;
            else if (dto.Action == "decrease") item.Quantity--;

            if (item.Quantity <= 0)
                _db.CartItems.Remove(item);

            await _db.SaveChangesAsync();

            var allItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Json(new
            {
                success = true,
                newQty = item.Quantity,
                total = allItems.Sum(c => c.Product.Price * c.Quantity),
                count = allItems.Sum(c => c.Quantity)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = GetUserId();
            var item = await _db.CartItems
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "Đã xóa sản phẩm!";
            return RedirectToAction("Index");
        }
    }

    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateQtyDto
    {
        public int CartItemId { get; set; }
        public string Action { get; set; } = "";
    }
}