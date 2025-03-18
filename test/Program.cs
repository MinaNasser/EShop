using EF_Core;
using System.Text;

namespace test
{
    public class Program
    {
        public static async Task Welcome (HttpContext http)
        {
            if(http.Request.Path == "" || http.Request.Path == "/")
            {
                await http.Response.WriteAsync("Welcome to Oure Store");
            }
            else if(http.Request.Path == "/products")
            {
                var context = new EShopContext();
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in context.Categories)
                {
                    stringBuilder.Append($"{item.Name} \n");
                }
                await http.Response.WriteAsync(stringBuilder.ToString());
            }
        }
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            var app = builder.Build();


            app.Run(Welcome);

            app.Run();
        }
    }
}