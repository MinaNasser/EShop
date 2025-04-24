// ... your using statements

using EF_Core.Models;
using EF_Core;
using EShop.API;
using EShop.Managers;
using EShop.Manegers;
using EShop.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(i =>
{
    i.Filters.Add<GenaralExceptionFilter>();
});

builder.Services.AddDbContext<EShopContext>(i =>
    i.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DBEshop")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EShopContext>();

// Managers & Services
builder.Services.AddScoped<ProductManager>();
builder.Services.AddScoped<CategoryManager>();
builder.Services.AddScoped<RoleManager>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<VendorManager>();
builder.Services.AddScoped<ClientManager>();
builder.Services.AddScoped<AccountServices>();
builder.Services.AddScoped<CartItemManager>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:PrivateKey"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
    )
);

// Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Static files
app.UseStaticFiles();

// Swagger Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop API v1");
        c.RoutePrefix = string.Empty; // Swagger ›Ì «·’›Õ… «·—∆Ì”Ì…
    });
}

// HTTP pipeline
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
