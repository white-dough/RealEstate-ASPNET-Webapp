using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Models;
using System.Security.Claims;

namespace RealEstate.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly RealEstateDbContext realEstateDbContext;
        public OrderController(RealEstateDbContext realEstateDbContext) {
            this.realEstateDbContext = realEstateDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await realEstateDbContext.Order.Join(
                realEstateDbContext.Property,
                o => o.PropertyId,
                p => p.Id.ToString(),
                (o, p) => new
                {
                    Id = o.Id,
                    Title = p.Title,
                    PropertyId = p.Id,
                    Status = o.Status,
                    Date = o.Date
                }).ToListAsync();

            List<OrderListModel> orderList = new List<OrderListModel>();
            foreach (var order in orders)
            {
                OrderListModel orderItem = new OrderListModel()
                {
                    Id = order.Id,
                    Date = order.Date,
                    Title = order.Title,
                    PropertyId = order.PropertyId.ToString(),
                    Status = order.Status,
                };

                orderList.Add(orderItem);
            }

            return await Task.Run(() => View("Index", orderList));
        }

        [HttpPost]
        public async Task<IActionResult> OrderProperty(string propertyId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = new OrderModel()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PropertyId = propertyId,
                Status = 0,
                Date = DateTime.Now
            };

            await realEstateDbContext.Order.AddAsync(order);
            await realEstateDbContext.SaveChangesAsync();

            TempData["success_message"] = "Order Successful";

            return RedirectToAction("Index", "Order");
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(Guid id)
        {
            var order = await realEstateDbContext.Order.FirstOrDefaultAsync(x => x.Id == id);

            if (order != null)
            {
                OrderDetailModel orderDetailModel = new OrderDetailModel();

                var property = await realEstateDbContext.Property.FirstOrDefaultAsync(p => p.Id.ToString() == order.PropertyId);

                orderDetailModel.order = order;
                orderDetailModel.property = property;

                return await Task.Run(() => View("OrderDetail", orderDetailModel));
            }


            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        public async Task<IActionResult> OrderCancel(Guid id)
        {
            var order = await realEstateDbContext.Order.FindAsync(id);

            if (order != null)
            {
                order.Status = 2;

                await realEstateDbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Order");
            }

            return RedirectToAction("Index", "Order");
        }
    }
}
