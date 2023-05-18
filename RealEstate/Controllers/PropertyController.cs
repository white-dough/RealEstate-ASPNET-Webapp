using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Models;

namespace RealEstate.Controllers
{
    public class PropertyController : Controller
    {
        private readonly RealEstateDbContext realEstateDbContext;
        public PropertyController(RealEstateDbContext realEstateDbContext) {
            this.realEstateDbContext = realEstateDbContext;
        }

       [HttpGet]
       public async Task<IActionResult> ShowProperties(string categoryType)
        {
            var properties = await realEstateDbContext.Property.Join(
                realEstateDbContext.User,
                p => p.Uid,
                u => u.Id.ToString(),
                (p, u) => new
                {
                    Id = p.Id,
                    PImage = p.PImage,
                    Stype = p.Stype,
                    Size = p.Size,
                    Location = p.Location,
                    IsFeatured = p.IsFeatured,
                    UserName = u.Name,
                    Date = p.Date,
                    Price = p.Price,
                    Title = p.Title,
                    Status = p.Status,
                    Type = p.Type
                }
               )
               .Where(p => p.Status == "available")
               .ToListAsync();

            if (!string.IsNullOrEmpty(categoryType))
            {
                properties = await realEstateDbContext.Property.Join(
                realEstateDbContext.User,
                p => p.Uid,
                u => u.Id.ToString(),
                (p, u) => new
                {
                    Id = p.Id,
                    PImage = p.PImage,
                    Stype = p.Stype,
                    Size = p.Size,
                    Location = p.Location,
                    IsFeatured = p.IsFeatured,
                    UserName = u.Name,
                    Date = p.Date,
                    Price = p.Price,
                    Title = p.Title,
                    Status = p.Status,
                    Type = p.Type,
                }
               )
               .Where(p => p.Type == categoryType)
               .Where(p => p.Status == "available")
               .ToListAsync();
            }

            List<PropertyListUserModel> propertyList = new List<PropertyListUserModel>();
            foreach (var property in properties)
            {
                PropertyListUserModel propertyItem = new PropertyListUserModel()
                {
                    Id = property.Id,
                    PImage = property.PImage,
                    Stype = property.Stype,
                    Size = property.Size,
                    Location = property.Location,
                    IsFeatured = property.IsFeatured,
                    UserName = property.UserName,
                    Date = property.Date,
                    Price = property.Price,
                    Title = property.Title
                };

                propertyList.Add(propertyItem);
            }


            return await Task.Run(() => PartialView("_IndexPropertyPartialView", propertyList));
        }

        [HttpGet]
        public async Task<IActionResult> Index(string categoryType)
        {

            var properties = await realEstateDbContext.Property.Join(
                realEstateDbContext.User,
                p => p.Uid,
                u => u.Id.ToString(),
                (p, u) => new
                {
                    Id = p.Id,
                    PImage = p.PImage,
                    Stype = p.Stype,
                    Size = p.Size,
                    Location = p.Location,
                    IsFeatured = p.IsFeatured,
                    UserName = u.Name,
                    Date = p.Date,
                    Price = p.Price,
                    Title = p.Title,
                    Status = p.Status
                }
               )
               .Where(p => p.Status == "available")
               .ToListAsync();

            List<PropertyListUserModel> propertyList = new List<PropertyListUserModel>();
            foreach ( var property in properties )
            {
                PropertyListUserModel propertyItem = new PropertyListUserModel()
                {
                    Id = property.Id,
                    PImage = property.PImage,
                    Stype = property.Stype,
                    Size = property.Size,
                    Location = property.Location,
                    IsFeatured = property.IsFeatured,
                    UserName = property.UserName,
                    Date = property.Date,
                    Price = property.Price,
                    Title = property.Title
                };

                propertyList.Add(propertyItem);
            }


            return await Task.Run(() => View(propertyList));
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetail(Guid id)
        {
            var property = await realEstateDbContext.Property.FirstOrDefaultAsync(x => x.Id == id);

            if (property != null)
            {
                PropertyDetailModel propertyDetailModel = new PropertyDetailModel();

                var properties = await realEstateDbContext.Property.ToListAsync();

                propertyDetailModel.property = property;
                propertyDetailModel.properties = properties;

                return await Task.Run(() => View("PropertyDetail", propertyDetailModel));
            }

            return RedirectToAction("Index", "Property");
        }
    }
}
