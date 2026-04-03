//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using SHOPVN.Data;
//using SHOPVN.Models;

//namespace SHOPVN.Controllers
//{
//    [Authorize]
//    public class OrderController : Controller
//    {
//        private readonly AppDbContext _db;

//        public OrderController(AppDbContext db)
//        {
//            _db = db;
//        }

//        // Lấy userId từ cookie đăng nhập
//        private int GetUserId()
//        {
//            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
//        }

//        // ==================== CHECKOUT ====================
//        // GET: /Order/Checkout — hiện trang thanh toán
//       public async Task<IActionResult> Checkout()
//        {
//            var userId = GetUserId();
//            // Lấy giỏ hàng hiện tại
//            var cartItems = await _db.CartItems
//                .Include(c => c.Product)
//                 .Where(c => c.UserId == userId)
//                .ToListAsync();

//            // Giỏ hàng trống thì về trang giỏ hàng
//            if (!cartItems.Any()) 
//            {
//                TempData["Error"] = "Giỏ hàng trống!";
//                return RedirectToAction("Index", "Cart");
//            }
//            // Lấy thông tin user để điền sẵn form
//            var user = await _db.Users.FindAsync(userId);

//            // Tính tổng tiền
//            var total = cartItems.Sum(c => c.Product.Price * c.Quantity);

//            ViewBag.CartItems = cartItems;
//            ViewBag.Total = total;
//            ViewBag.User = user;

//            return View();
//       }
//        // POST: /Order/PlaceOrder — xử lý đặt hàng
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
//        {
//            var userId = GetUserId();

//            // Lấy giỏ hàng
//            var cartItems = await _db.CartItems
//                .Include(c => c.Product)
//                .Where(c => c.UserId == userId)
//                .ToListAsync();

//            if (!cartItems.Any())
//                return RedirectToAction("Index", "Cart");
//            //Tính tổng tiền
//            var total = cartItems.Sum(c => c.Product.Price * c.Quantity);

//            // Tạo mã đơn hàng tự động: DH + timestamp
//            var orderCode = "DH" + DateTime.Now.ToString("yyMMddHHmmss");
//        }
//    }
//}
