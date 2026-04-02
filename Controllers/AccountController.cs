using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SHOPVN.Models;

namespace SHOPVN.Controllers
{
    public class AccountController : Controller
    {
        // UserManager: quản lý user (tạo, tìm, xóa...)
        // SignInManager: xử lý đăng nhập / đăng xuất
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==================== ĐĂNG NHẬP ====================

        // GET: /Account/Login — hiện form đăng nhập
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập rồi thì chuyển về trang chủ
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login — xử lý khi bấm nút đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            // Kiểm tra dữ liệu nhập có hợp lệ không
            if (!ModelState.IsValid)
                return View(model);

            // Tìm user theo email
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,   // ghi nhớ đăng nhập
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng nhập thành công!";
                // Quay về trang trước đó hoặc trang chủ
                return LocalRedirect(returnUrl ?? "/");
            }

            // Đăng nhập thất bại
            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng!");
            return View(model);
        }

        // ==================== ĐĂNG KÝ ====================

        // GET: /Account/Register — hiện form đăng ký
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Account/Register — xử lý khi bấm nút đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Tạo user mới
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                EmailConfirmed = true // bỏ qua xác thực email cho đơn giản
            };

            // Lưu user vào database kèm mật khẩu đã mã hóa
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gán role Customer cho user mới
                await _userManager.AddToRoleAsync(user, "Customer");

                // Tự động đăng nhập luôn sau khi đăng ký
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["Success"] = $"Chào mừng {user.FirstName}! Tài khoản đã được tạo.";
                return RedirectToAction("Index", "Home");
            }

            // Có lỗi thì hiện ra (ví dụ: email đã tồn tại, mật khẩu yếu...)
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ==================== ĐĂNG XUẤT ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Đã đăng xuất!";
            return RedirectToAction("Index", "Home");
        }

        // ==================== HỒ SƠ ====================

        // GET: /Account/Profile — xem thông tin tài khoản
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
    }

    // ===== ViewModels (đặt ngay dưới cùng file cho tiện) =====

    // Dữ liệu form đăng nhập
    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập email")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }

    // Dữ liệu form đăng ký
    public class RegisterViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập họ")]
        public string FirstName { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập tên")]
        public string LastName { get; set; } = "";

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập email")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";

        public string? Phone { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = "";
    }
}