using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Helper;
using RealEstate.Models;
using System.Security.Claims;

namespace RealEstate.Controllers
{
    public class LoginController : Controller
	{
		private readonly RealEstateDbContext realEstateDbContext;
		public LoginController(RealEstateDbContext realEstateDbContext)
		{
			this.realEstateDbContext = realEstateDbContext;
		}
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(LoginUserModel userModel)
		{
			var passwordHasher = new CustomPasswordHasher();
			var user = await realEstateDbContext.User.Where(x => x.Username == userModel.UserName && x.RoleName == "user").FirstOrDefaultAsync();

			if (user != null)
			{
				if (passwordHasher.VerifyHashedPassword(user.Password, userModel.Password))
				{
					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userModel.UserName),
					new Claim(ClaimTypes.Role, "user"),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				};
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);
					var props = new AuthenticationProperties();
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
					return RedirectToAction("Index", "Home");
				} else
				{
					TempData["message"] = "Username and/or password is incorrect.";
				}
			} else
			{
				TempData["message"] = "Username and/or password is incorrect.";
			}

			return RedirectToAction("Index", "Login");
		}

        [HttpGet]
        public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
    }
}
