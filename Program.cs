
using Microsoft.EntityFrameworkCore;
using IhtiyacMolasi.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Servisler ─────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// Database First – SQL Server bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)
    ));

// Session (anonim kullanıcı takibi)
builder.Services.AddSession(opts =>
{
    opts.IdleTimeout = TimeSpan.FromDays(30);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
    opts.Cookie.Name = ".IhtiyacMolasi.Session";
});

builder.Services.AddMemoryCache();

// ── Uygulama pipeline ─────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// ── Route yapılandırması ──────────────────────────────────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
