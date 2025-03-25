using EF_Core;
using EF_Core.Models;
using EShop.Managers;
using EShop.Manegers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EShopContext>
    (i => i.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DBEshop")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();
builder.Services.AddScoped(typeof(ProductManager));
builder.Services.AddScoped(typeof(CategoryManager));
builder.Services.AddScoped(typeof(RoleManager));
builder.Services.AddScoped(typeof(AccountManager));
builder.Services.AddScoped(typeof(VendorManager));
builder.Services.AddScoped(typeof(ClientManager));



var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}");


app.Run();