using EF_Core;
using EF_Core.Models;
using EShop.Managers;
using EShop.Manegers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
    .AddDbContext<EShopContext>
    (
        i => i.UseLazyLoadingProxies()
        .UseSqlServer
        (
            builder.Configuration.GetConnectionString("DBEshop")
        )

    );
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();


builder.Services.AddScoped(typeof(ProductManager));
builder.Services.AddScoped(typeof(CategoryManager));
builder.Services.AddScoped(typeof(AccountManager));
var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();//Force WWWRoot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}/{id?}");

app.Run();
