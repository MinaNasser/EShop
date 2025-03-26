using EF_Core;
using EF_Core.Models;
using EShop.Managers;
using EShop.Manegers;
using EShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped(typeof(AccountServices));
builder.Services.AddScoped(typeof(CartItemManager));
builder.Services.AddScoped<ICartItemService, CartItemService>();
    

builder.Services.AddAuthentication(
    option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
 ).AddJwtBearer(option =>
{
    //on One Statless Request
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:PrivateKey"]))
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

//app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}");


app.Run();