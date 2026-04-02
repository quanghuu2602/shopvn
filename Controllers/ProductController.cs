using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOPVN.Data;

namespace SHOPVN.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Product — danh sách sản phẩm, có lọc và tìm kiếm
        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            // Bắt đầu từ tất cả sản phẩm đang bán
            var query = _db.Products
                .Include(p => p.Category) // kèm thông tin danh mục
                .Where(p => p.IsActive)
                .AsQueryable();

            // Nếu có tìm kiếm thì lọc theo tên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Brand.Contains(search));
                ViewData["Search"] = search;
            }

            // Nếu chọn danh mục thì lọc theo danh mục
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
                ViewBag.SelectedCategory = categoryId;
            }

            // Lấy danh sách danh mục để hiện filter
            ViewBag.Categories = await _db.Categories.ToListAsync();

            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }

        // GET: /Product/Detail/5 — chi tiết 1 sản phẩm
        public async Task<IActionResult> Detail(int id)
        {
            // Tìm sản phẩm theo id, kèm danh mục
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Không tìm thấy thì về trang 404
            if (product == null)
                return NotFound();

            // Lấy 4 sản phẩm cùng danh mục để gợi ý
            ViewBag.Related = await _db.Products
                .Where(p => p.CategoryId == product.CategoryId
                         && p.Id != id
                         && p.IsActive)
                .Take(4)
                .ToListAsync();

            return View(product);
        }
    }
}