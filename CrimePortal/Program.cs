using CrimePortal.Data;  // <-- Add this
using CrimePortal.Helpers;
using CrimePortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Log connection strings for debugging
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
var crimeRegConn = builder.Configuration.GetConnectionString("CrimeRegisterConnection");
Console.WriteLine($"🔍 DefaultConnection: {MaskPassword(defaultConn)}");
Console.WriteLine($"🔍 CrimeRegisterConnection: {MaskPassword(crimeRegConn)}");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CRDbContext>(options =>
    options.UseNpgsql(crimeRegConn));

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(defaultConn));

static string MaskPassword(string? connStr)
{
    if (string.IsNullOrEmpty(connStr)) return "NULL";
    var parts = connStr.Split(';');
    var masked = parts.Select(p => p.ToLower().Contains("password") ? "Password=***MASKED***" : p);
    return string.Join(";", masked);
}

// ✅ (नया जोड़ा गया कोड) — Authentication जोड़ने के लिए
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";                // कुकी का नाम
        options.LoginPath = "/Account/Login";                // लॉगिन पेज का पथ
        options.AccessDeniedPath = "/Account/AccessDenied";  // Access denied पेज का पथ
    });

builder.Services.AddAuthorization();  // ✅ (नया जोड़ा गया कोड)

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Users.Any(u => u.UserName == "admin"))
    {
        var admin = new User
        {
            UserName = "admin",
            PasswordHash = PasswordHelper.Hash("admin"), // ✅ use PasswordHash
            UserType = "Admin"
        };

        db.Users.Add(admin);
        db.SaveChanges();
        Console.WriteLine("✅ Default admin user created: username=admin, password=admin");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// Default route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
