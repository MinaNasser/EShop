using EF_Core;
using EShop.Manegers;
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

builder.Services.AddScoped(typeof(ProductManager));
builder.Services.AddScoped(typeof(CategoryManager));

var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();//Force WWWRoot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}/{id?}");

app.Run();
