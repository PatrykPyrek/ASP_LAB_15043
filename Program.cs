using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Connection string do bazy danych
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection")
                           ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Konfiguracja DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Konfiguracja Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Konfiguracja cache i sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Dodanie kontrolerów i widoków
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Obsługa błędów dla środowisk produkcyjnych
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware dla logów
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");


//app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Project}/{action=Index}");


app.Run();
