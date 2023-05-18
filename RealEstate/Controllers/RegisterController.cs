using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using RealEstate.Data;
using RealEstate.Helper;
using RealEstate.Models;
using System.Web;

namespace RealEstate.Controllers
{
    public class RegisterController : Controller
	{
		private readonly RealEstateDbContext realEstateDbContext;
		public RegisterController(RealEstateDbContext realEstateDbContext) 
        {
            this.realEstateDbContext = realEstateDbContext;
		}

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public IActionResult RegisterAdmin()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserModel registerUserModel)
        {
            try
            {
				var passwordHasher = new CustomPasswordHasher();
				var user = new User()
				{
					Name = registerUserModel.Name,
					Password = passwordHasher.HashPassword(registerUserModel.Password),
					RoleName = "user",
					Username = registerUserModel.Username
				};

				await realEstateDbContext.User.AddAsync(user);
				await realEstateDbContext.SaveChangesAsync();
				return RedirectToAction("Index", "Login");
			}
            catch (Exception)
            {

                throw;
            }
        }

		[HttpPost]
		public async Task<IActionResult> RegisterAdmin(RegisterUserModel registerUserModel)
		{
			try
			{
				var passwordHasher = new CustomPasswordHasher();
				var user = new User()
				{
					Name = registerUserModel.Name,
					Password = passwordHasher.HashPassword(registerUserModel.Password),
					RoleName = "admin",
					Username = registerUserModel.Username
				};

				await realEstateDbContext.User.AddAsync(user);
				await realEstateDbContext.SaveChangesAsync();
				return RedirectToAction("Index", "Admin");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
