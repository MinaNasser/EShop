using EShop.Services;
using EShop.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/{Controller}")]
    public class AccountController : ControllerBase
    {
        private AccountServices accountService;
        public AccountController(AccountServices _accountServices)
        {
            accountService = _accountServices;
        }


        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserRegisterViewModel user)
        {

            if (ModelState.IsValid)
            {
                var res = await accountService.CreateAccount(user);
                if (res.Succeeded)
                {
                    return new JsonResult(new
                    {
                        Massage = "Your Account Added Successfully ,Go to Login",
                        Status = 200
                    });
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    res.Errors.ForEach(err => stringBuilder.Append(err.Description));
                    return new JsonResult(new
                    {
                        Massage = stringBuilder.ToString(),
                        Status = 400
                    });
                }
            }
            StringBuilder stringBuilder1 = new StringBuilder();
            foreach (var item in ModelState.Values)
            {
                foreach (var err in item.Errors)
                {
                    stringBuilder1.Append(err.ErrorMessage);
                }
            }

            return new JsonResult(new
            {
                Massage = stringBuilder1.ToString(),
                Status = 400
            });
        }

        [HttpPost("Login")]
        [Route("Login")]
        public async Task<IActionResult> Login( UserLoginViewModel vmodel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new
                {
                    Message = string.Join(" | ", errors),
                    Status = 400
                });
            }

            try
            {
                var res = await accountService.GenerateToken(vmodel);

                if (string.IsNullOrEmpty(res))
                {
                    return BadRequest(new
                    {
                        Message = "Sorry, Invalid Email, Username, or Password.",
                        Status = 400
                    });
                }

                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                string role = roleClaim?.Value ?? "Unknown";

                return Ok(new
                {
                    Message = "Logged in Successfully",
                    Status = 200,
                    Token = res,
                    Role = role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred. Please try again later.",
                    Error = ex.Message,
                    Status = 500
                });
            }
        }

        public async Task<IActionResult> Signout()
        {
            await accountService.Logout();
            return new JsonResult(new
            {
                Massage = "Sign out Successfully",
                Status = 200
            });
        }

    }
}



//using EShop.Services;
//using EShop.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;

//namespace EShop.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AccountController : ControllerBase
//    {
//        private readonly AccountServices accountService;

//        public AccountController(AccountServices _accountServices)
//        {
//            accountService = _accountServices;
//        }

//        [HttpPost("Register")]
//        public async Task<IActionResult> Register(UserRegisterViewModel user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new
//                {
//                    Message = "Invalid input",
//                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
//                });
//            }

//            var res = await accountService.CreateAccount(user);
//            if (res.Succeeded)
//            {
//                return Ok(new
//                {
//                    Message = "Your account has been successfully created. Please log in.",
//                    Status = 200
//                });
//            }
//            else
//            {
//                return BadRequest(new
//                {
//                    Message = "Registration failed",
//                    Errors = res.Errors.Select(err => err.Description)
//                });
//            }
//        }

//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(UserLoginViewModel vmodel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new
//                {
//                    Message = "Invalid input",
//                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
//                });
//            }

//            var token = await accountService.GenerateToken(vmodel);
//            if (string.IsNullOrEmpty(token))
//            {
//                return BadRequest(new
//                {
//                    Message = "Invalid username or password, or account is under review.",
//                    Status = 400
//                });
//            }

//            // استخراج الدور من التوكن JWT
//            var role = ExtractRoleFromJwt(token);

//            return Ok(new
//            {
//                Message = "Logged in successfully",
//                Status = 200,
//                Token = token,
//                Role = role
//            });
//        }

//        [HttpPost("Signout")]
//        public async Task<IActionResult> Signout()
//        {
//            await accountService.Logout();
//            return Ok(new
//            {
//                Message = "Signed out successfully",
//                Status = 200
//            });
//        }

//        // دالة لاستخراج الدور من التوكن JWT
//        private string ExtractRoleFromJwt(string token)
//        {
//            var jwtParts = token.Split('.');
//            if (jwtParts.Length < 2) return "Unknown";

//            var payload = Encoding.UTF8.GetString(Convert.FromBase64String(jwtParts[1]));
//            var claims = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(payload);

//            return claims != null && claims.ContainsKey("role") ? claims["role"].ToString() : "Unknown";
//        }
//    }
//}
