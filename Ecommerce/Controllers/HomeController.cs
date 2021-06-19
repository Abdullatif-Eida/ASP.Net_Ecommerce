using Ecommerce.Areas.Identity.Data;
using Ecommerce.Enum;
using Ecommerce.Models;
using Ecommerce.Vm;
using Microsoft.AspNetCore.Authorization;
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
            var ListCollections = _context.Items.Where(item => item.IsDeleted == false).Select(item => new ItemVM()
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

            var listCategoryItems = _context.Categories.Where(categories => categories.IsDeleted == false).Take(9).Select(categories => new CategoryVM() {
                Name = categories.CategoryName,
                Description = categories.Description,
                CreationDate = categories.CreationDate,
                Id = categories.Id,
                ImageId = _context.Attachments
                        .FirstOrDefault(image => image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
                        .FileName

            }).ToList();

            var listCategory = _context.Categories.Where(categories => categories.IsDeleted == false).Select(categories => new CategoryVM()
            {
                    Name = categories.CategoryName,
                    Description = categories.Description,
                    CreationDate = categories.CreationDate,
                    Id = categories.Id,
                ImageId = _context.Attachments
            .FirstOrDefault(image => image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
            .FileName

            }).ToList();
            var listBrands = _context.Brands.Where(brands => brands.IsDeleted == false).Take(6).Select(brands => new BrandVM()
            {
                Name = brands.BrandName,
                ImageId = brands.ImageId,
                Description = brands.Description,
                CreationDate = brands.CreationDate,
                Id = brands.Id,
            }).ToList();
            return View(new HomeVM() { ListCollections= ListCollections,ListItems = ListItems, ListCategoryItems=listCategoryItems, ListCategory=listCategory ,ListBrands=listBrands, });
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

        public IActionResult Shop()
        {
            var ListItems = _context.Items.Where(item => item.IsDeleted == false).Select(item => new ItemVM()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _context.Attachments.FirstOrDefault(r => r.RecordId == item.Id.ToString() && r.RecordType == Enum.RecordType.Items).FileName
            }).ToList();

            var listCategoryItems = _context.Categories.Where(categories => categories.IsDeleted == false).Take(5).Select(categories => new CategoryVM()
            {
                Name = categories.CategoryName,
                Description = categories.Description,
                CreationDate = categories.CreationDate,
                Id = categories.Id,
                ImageId = _context.Attachments
                        .FirstOrDefault(image => image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
                        .FileName

            }).ToList();
            var listBrands = _context.Brands.Where(brands => brands.IsDeleted == false).Select(brands => new BrandVM()
            {
                Name = brands.BrandName,
                ImageId = brands.ImageId,
                Description = brands.Description,
                CreationDate = brands.CreationDate,
                Id = brands.Id,
            }).ToList();
            return View(new HomeVM() { ListItems = ListItems, ListCategoryItems = listCategoryItems, ListBrands=listBrands });

        }

        public IActionResult GetItemByCategory(int id)
        {
            var listofCategoryItems = _context.Items.Where(categories => categories.CategoryId == id && categories.IsDeleted == false).Select(x => new ItemVM() { 
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                BrandName = x.Brands.BrandName,
                CategoryName = x.Category.CategoryName,
                ImageId = _context.Attachments.FirstOrDefault(items => items.RecordId == x.Id.ToString() && items.RecordType == RecordType.Items).FileName

            });         
            return View(listofCategoryItems);
        }

        public IActionResult GetItemByBrand(int id)
        {
            var listItems = _context.Items.Where(items => items.BrandId == id && items.IsDeleted == false).Select(x => new ItemVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                BrandName = x.Brands.BrandName,
                CategoryName = x.Category.CategoryName,
                ImageId = _context.Attachments.FirstOrDefault(items => items.RecordId == x.Id.ToString() && items.RecordType == RecordType.Items).FileName
            });
            return View(listItems);
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(Vm.ContactUs contactUs)
        {
            var contactentity = await _context.ContactUs.AddAsync(new Models.ContactUs()
            {
                FullName = contactUs.FullName,
                Subject = contactUs.Subject,
                Email = contactUs.Email,
                Message = contactUs.Message,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("PaymentComplete");
        }

        [Authorize]
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
