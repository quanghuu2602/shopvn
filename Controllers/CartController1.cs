using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SHOPVN.Data;
using SHOPVN.Models;

namespace SHOPVN.Controllers
{
    [Authorize] // Phải đăng nhập mới dùng được giỏ hàng
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        // Lấy userId từ cookie đăng nhập
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        // GET: /Cart — xem giỏ hàng
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            // Lấy tất cả item trong giỏ của user này, kèm thông tin sản phẩm
            var cartItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Tính tổng tiền
            var total = cartItems.Sum(c => c.Product.Price * c.Quantity);
            ViewBag.Total = total;

            return View(cartItems);
        }

        // POST: /Cart/Add — thêm sản phẩm vào giỏ (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();

            // Kiểm tra sản phẩm có tồn tại không
            var product = await _db.Products.FindAsync(dto.ProductId);
            if (product == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });

            // Kiểm tra còn hàng không
            if (product.Stock < dto.Quantity)
                return Json(new { success = false, message = "Không đủ hàng" });

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingItem = await _db.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                // Đã có rồi thì tăng số lượng
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                // Chưa có thì thêm mới
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                _db.CartItems.Add(cartItem);
            }

            await _db.SaveChangesAsync();

            // Đếm tổng số item trong giỏ để cập nhật badge
            var count = await _db.CartItems
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);

            return Json(new { success = true, count });
        }

        // POST: /Cart/UpdateQty — cập nhật số lượng (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQty([FromBody] UpdateQtyDto dto)
        {
            var userId = GetUserId();

            var item = await _db.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == dto.CartItemId && c.UserId == userId);

            if (item == null)
                return Json(new { success = false });

            // Tăng hoặc giảm số lượng
            if (dto.Action == "increase")
                item.Quantity++;
            else if (dto.Action == "decrease")
                item.Quantity--;

            // Nếu số lượng về 0 thì xóa khỏi giỏ
            if (item.Quantity <= 0)
                _db.CartItems.Remove(item);

            await _db.SaveChangesAsync();

            // Tính lại tổng
            var allItems = await _db.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var total = allItems.Sum(c => c.Product.Price * c.Quantity);
            var count = allItems.Sum(c => c.Quantity);

            return Json(new
            {
                success = true,
                newQty = item.Quantity,
                total,
                count
            });
        }

        // POST: /Cart/Remove — xóa 1 sản phẩm khỏi giỏ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = GetUserId();

            var item = await _db.CartItems
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
            }

            return RedirectToAction("Index");
        }
    }

    // DTOs — nhận dữ liệu từ AJAX
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateQtyDto
    {
        public int CartItemId { get; set; }
        public string Action { get; set; } = ""; // "increase" hoặc "decrease"
    }
}