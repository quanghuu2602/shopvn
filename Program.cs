using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SHOPVN.Data;
using SHOPVN.Models;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(opt => {
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
    opt.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie login
builder.Services.ConfigureApplicationCookie(opt => {
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
});

// Antiforgery đọc token từ header AJAX
builder.Services.AddAntiforgery(opt => {
    opt.HeaderName = "RequestVerificationToken";
});

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Seed roles + admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var role in new[] { "Admin", "Customer" })
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));

    if (await userManager.FindByEmailAsync("admin@shopvn.com") == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@shopvn.com",
            Email = "admin@shopvn.com",
            FirstName = "Admin",
            LastName = "ShopVN",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();