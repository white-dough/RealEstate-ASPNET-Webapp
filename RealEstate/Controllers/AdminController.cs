using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Helper;
using RealEstate.Models;
using System.Security.Claims;
using System.Diagnostics;

namespace RealEstate.Controllers
{
	[Authorize(Policy = "AdminOnly")]
	public class AdminController : Controller
	{
		private IWebHostEnvironment Environment;
		private readonly RealEstateDbContext realEstateDbContext;

		public AdminController(RealEstateDbContext realEstateDbContext, IWebHostEnvironment environment)
		{
			this.realEstateDbContext = realEstateDbContext;
			this.Environment = environment;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Dashboard", "Admin");

			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login(LoginUserModel userModel)
		{
			var passwordHasher = new CustomPasswordHasher();
			var user = await realEstateDbContext.User.Where(x => x.Username == userModel.UserName && x.RoleName == "admin").FirstOrDefaultAsync();

			if (user != null)
			{
				if (passwordHasher.VerifyHashedPassword(user.Password, userModel.Password))
				{
					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userModel.UserName),
					new Claim(ClaimTypes.Role, "admin"),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				};
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);
					var props = new AuthenticationProperties();
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
					return RedirectToAction("Dashboard", "Admin");
				} else
				{
					TempData["message"] = "Username and/or password is incorrect.";
				}
			} else
			{
				TempData["message"] = "Username and/or password is incorrect.";
			}

			return RedirectToAction("Index", "Admin");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Admin");
		}

		[HttpGet]
		public IActionResult Dashboard()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AddProperty() { return View(); }

		[HttpPost]
		public async Task<IActionResult> AddProperty(AddPropertyModel propertyRequest)
		{
			try
			{
				string path = Path.Combine(this.Environment.WebRootPath, "UploadFiles");

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				List<IFormFile> postedFiles = new List<IFormFile>();
				postedFiles.Add(propertyRequest.PImageFile);
				postedFiles.Add(propertyRequest.PImageOneFile);
				postedFiles.Add(propertyRequest.PImageTwoFile);
				postedFiles.Add(propertyRequest.PImageThreeFile);
				postedFiles.Add(propertyRequest.PImageFourFile);
				postedFiles.Add(propertyRequest.MapImageFile);
				postedFiles.Add(propertyRequest.GroundMapImageFile);
				postedFiles.Add(propertyRequest.TopMapImageFile);

				List<string> uploadedFiles = new List<string>();

				foreach (IFormFile file in postedFiles)
				{
					string fileName = Path.GetFileName(file.FileName);
					using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
					{
						file.CopyTo(stream);
						uploadedFiles.Add(fileName);
					}
				}

				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

				var property = new PropertyModel()
				{
					Id = Guid.NewGuid(),
					Title = propertyRequest.Title,
					PContent = propertyRequest.PContent,
					Type = propertyRequest.Type,
					Bhk = propertyRequest.Bhk,
					Stype = propertyRequest.Stype,
					Floor = propertyRequest.Floor,
					Bedroom = propertyRequest.Bedroom,
					Bathroom = propertyRequest.Bathroom,
					Balcony = propertyRequest.Balcony,
					Hall = propertyRequest.Hall,
					Size = propertyRequest.Size,
					Price = propertyRequest.Price,
					Location = propertyRequest.Location,
					City = propertyRequest.City,
					Barangay = propertyRequest.Barangay,
					Feature = propertyRequest.Feature,
					PImage = uploadedFiles[0],
					PImageOne = uploadedFiles[1],
					PImageTwo = uploadedFiles[2],
					PImageThree = uploadedFiles[3],
					PImageFour = uploadedFiles[4],
					Uid = userId,
					Status = propertyRequest.Status,
					MapImage = uploadedFiles[5],
					TopMapImage = uploadedFiles[7],
					GroundMapImage = uploadedFiles[6],
					TotalFloor = propertyRequest.TotalFloor,
					IsFeatured = propertyRequest.IsFeatured,
					Date = DateTime.Now,
				};

				await realEstateDbContext.Property.AddAsync(property);
				await realEstateDbContext.SaveChangesAsync();

				TempData["success_message"] = "Property Added";

				return RedirectToAction("ListProperty");

			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet]
		public async Task<IActionResult> ListProperty()
		{
			var properties = await realEstateDbContext.Property.ToListAsync();
			return View(properties);
		}

		[HttpGet]
		public async Task<IActionResult> ViewProperty(Guid id)
		{
			var property = await realEstateDbContext.Property.FirstOrDefaultAsync(x => x.Id == id);

			if (property != null)
			{
				var viewModel = new EditPropertyModel()
				{
					Id = property.Id,
					Title = property.Title,
					PContent = property.PContent,
					Type = property.Type,
					Bhk = property.Bhk,
					Stype = property.Stype,
					Floor = property.Floor,
					Bedroom = property.Bedroom,
					Bathroom = property.Bathroom,
					Balcony = property.Balcony,
					Hall = property.Hall,
					Size = property.Size,
					Price = property.Price,
					Location = property.Location,
					City = property.City,
					Barangay = property.Barangay,
					Feature = property.Feature,
					PImage = property.PImage,
					PImageOne = property.PImageOne,
					PImageTwo = property.PImageTwo,
					PImageThree = property.PImageThree,
					PImageFour = property.PImageFour,
					Uid = property.Uid,
					Status = property.Status,
					MapImage = property.MapImage,
					TopMapImage = property.TopMapImage,
					GroundMapImage = property.GroundMapImage,
					TotalFloor = property.TotalFloor,
					IsFeatured = property.IsFeatured,
					Date = property.Date,
				};

				return await Task.Run(() => View("ViewProperty", viewModel));
			}

			return RedirectToAction("ListProperty");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProperty(EditPropertyModel model)
		{
			try
			{
				var property = await realEstateDbContext.Property.FindAsync(model.Id);

				if (property != null)
				{
					string path = Path.Combine(this.Environment.WebRootPath, "UploadFiles");

					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					List<IFormFile> postedFiles = new List<IFormFile>();
					postedFiles.Add(model.PImageFile);
					postedFiles.Add(model.PImageOneFile);
					postedFiles.Add(model.PImageTwoFile);
					postedFiles.Add(model.PImageThreeFile);
					postedFiles.Add(model.PImageFourFile);
					postedFiles.Add(model.MapImageFile);
					postedFiles.Add(model.GroundMapImageFile);
					postedFiles.Add(model.TopMapImageFile);

					List<string> uploadedFiles = new List<string>();

					foreach (IFormFile file in postedFiles)
					{

						if (file != null)
						{
							string fileName = Path.GetFileName(file.FileName);
							using FileStream stream = new(Path.Combine(path, fileName), FileMode.Create);
							file.CopyTo(stream);
							uploadedFiles.Add(fileName);
						}
						else
						{
							uploadedFiles.Add("");
						}
					}


					property.Title = model.Title;
					property.PContent = model.PContent;
					property.Type = model.Type;
					property.Bhk = model.Bhk;
					property.Stype = model.Stype;
					property.Floor = model.Floor;
					property.Bedroom = model.Bedroom;
					property.Bathroom = model.Bathroom;
					property.Balcony = model.Balcony;
					property.Hall = model.Hall;
					property.Size = model.Size;
					property.Price = model.Price;
					property.Location = model.Location;
					property.City = model.City;
					property.Barangay = model.Barangay;
					property.Feature = model.Feature;
					property.PImage = string.IsNullOrEmpty(uploadedFiles[0]) ? model.PImage : uploadedFiles[0];
					property.PImageOne = string.IsNullOrEmpty(uploadedFiles[1]) ? model.PImageOne : uploadedFiles[1];
					property.PImageTwo = string.IsNullOrEmpty(uploadedFiles[2]) ? model.PImageTwo : uploadedFiles[2];
					property.PImageThree = string.IsNullOrEmpty(uploadedFiles[3]) ? model.PImageThree : uploadedFiles[3];
					property.PImageFour = string.IsNullOrEmpty(uploadedFiles[4]) ? model.PImageFour : uploadedFiles[4];
					property.Uid = model.Uid;
					property.Status = model.Status;
					property.MapImage = string.IsNullOrEmpty(uploadedFiles[5]) ? model.MapImage : uploadedFiles[5];
					property.TopMapImage = string.IsNullOrEmpty(uploadedFiles[7]) ? model.TopMapImage : uploadedFiles[7];
					property.GroundMapImage = string.IsNullOrEmpty(uploadedFiles[6]) ? model.GroundMapImage : uploadedFiles[6];
					property.TotalFloor = model.TotalFloor;
					property.IsFeatured = model.IsFeatured;
					property.Date = DateTime.Now;

					await realEstateDbContext.SaveChangesAsync();

					return RedirectToAction("ListProperty");
				}

				return RedirectToAction("ListProperty");

			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet]
		public async Task<IActionResult> DeleteProperty(Guid id)
		{
			var property = await realEstateDbContext.Property.FindAsync(id);

			if (property != null)
			{
				realEstateDbContext.Property.Remove(property);
				await realEstateDbContext.SaveChangesAsync();

				return RedirectToAction("ListProperty");
			}

			return RedirectToAction("ListProperty");
		}

		[HttpGet]
		public async Task<IActionResult> OrderList()
		{
			var orders = await realEstateDbContext.Order.Join(
				realEstateDbContext.User,
				o => o.UserId,
				u => u.Id.ToString(),
				(o, u) => new
				{
					Id = o.Id,
					UserId = o.UserId, 
					UserName = u.Name,
					Status = o.Status,
					Date = o.Date,
					PropertyId = o.PropertyId,
					Title = "",
				}).Join(
				realEstateDbContext.Property,
				o => o.PropertyId,
				p => p.Id.ToString(),
				(o, p) => new
				{
					Id = o.Id,
					UserId = o.UserId,
					UserName = o.UserName,
					Status = o.Status,
					Date = o.Date,
					PropertyId = o.PropertyId,
					Title = p.Title,
				}).ToListAsync();

			List<OrderListDetail> orderDetailModelList = new List<OrderListDetail>();

			foreach (var order in orders)
			{
				OrderListDetail orderDetail = new OrderListDetail();
				orderDetail.Id = order.Id;
				orderDetail.UserId = order.UserId;
				orderDetail.UserName = order.UserName;
				orderDetail.Status = order.Status;
				orderDetail.Date = order.Date;
				orderDetail.PropertyId = order.PropertyId;
				orderDetail.Title = order.Title;

				orderDetailModelList.Add(orderDetail);
			}

			return await Task.Run(() => View("OrderList", orderDetailModelList));
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOrder(UpdateOrderModel model)
		{
			var order = await realEstateDbContext.Order.FindAsync(model.Id);

			if (order != null)
			{
				order.Status = model.Status;
				await realEstateDbContext.SaveChangesAsync();

				return RedirectToAction("OrderList");
			}
			return RedirectToAction("OrderList");
		}
	}
}
