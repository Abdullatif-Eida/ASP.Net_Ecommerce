using Ecommerce.Areas.Identity.Data;
using Ecommerce.Models;
using Ecommerce.Vm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceContext _context;

        public HomeController(EcommerceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ListCollections = _context.Items.Where(item => item.CategoryId==14 && item.IsDeleted == false).Select(item => new ItemVM()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _context.Attachments.FirstOrDefault(r => r.RecordId == item.Id.ToString()&&r.RecordType ==Enum.RecordType.Items).FileName
            }).ToList();

            var ListItems = _context.Items.Where(item => item.CategoryId != 14 && item.IsDeleted == false).Take(6).Select(item => new ItemVM()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _context.Attachments.FirstOrDefault(r => r.RecordId == item.Id.ToString() && r.RecordType == Enum.RecordType.Items).FileName
            }).ToList();

            return View(new HomeVM() { ListCollections= ListCollections,ListItems = ListItems });
        }

        public IActionResult PaymentComplete()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id) 
        {
            var itemDB = await _context.Items.Include(item => item.Brands).Include(x => x.Category).FirstAsync(item => item.Id == id);
            var itemVM = new ItemVM()
            {
                Id = itemDB.Id,
                Name = itemDB.Name,
                Description = itemDB.Description,
                ShortDescription = itemDB.ShortDescription,
                Price = itemDB.Price,
                BrandName = itemDB.Brands.BrandName,
                CategoryName = itemDB.Category.CategoryName,
                Images = _context.Attachments.Where(r => r.RecordId == itemDB.Id.ToString() && r.RecordType == Enum.RecordType.Items).Select(item => item.FileName).ToList()
            };
            return View(itemVM);
        }

        public async Task<IActionResult> Checkout(int id)
        {
            var itemDB = await _context.Items.Include(item => item.Brands).Include(x => x.Category).FirstAsync(item => item.Id == id);
            var itemVM = new ItemVM()
            {
                Id = itemDB.Id,
                Name = itemDB.Name,
                Description = itemDB.Description,
                ShortDescription = itemDB.ShortDescription,
                Price = itemDB.Price,
                BrandName = itemDB.Brands.BrandName,
                CategoryName = itemDB.Category.CategoryName,
                Images = _context.Attachments.Where(r => r.RecordId == itemDB.Id.ToString() && r.RecordType == Enum.RecordType.Items).Select(item => item.FileName).ToList()
            };

            var checkoutVM = new ChekoutVM
            {
                Item = itemVM,
            };
            return View(checkoutVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(ChekoutVM chekout)
        {
           var orderentity = await _context.Orders.AddAsync(new Orders() { FirstName = chekout.FirstName, 
                Phone = chekout.Phone, Address = chekout.Address, Email = chekout.Email, 
                Country = chekout.Country, City = chekout.City, ZIPCode = chekout.ZIPCode, 
                itemId = chekout.Item.Id 
            });


            await _context.SaveChangesAsync();
            return RedirectToAction("PaymentComplete");
        }
    }
}
