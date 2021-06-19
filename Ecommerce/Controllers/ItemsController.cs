using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Ecommerce.Enum;
using Ecommerce.Models;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ecommerce.Vm;
using static Ecommerce.Vm.ChekoutVM;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        public EcommerceContext _dbContext { get; set; }
        public readonly IHostingEnvironment _hostingEnvironment;

        public ItemsController(EcommerceContext context, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var Items = _dbContext.Items.Where(item => item.IsDeleted == false).ToList().Select(item => new ItemVM()
            {
                Name = item.Name,
                Id = item.Id,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _dbContext.Attachments.FirstOrDefault(items=> items.RecordId == item.Id.ToString()&& items.RecordType == RecordType.Items)?.FileName
            });

            return View(Items);
        }

        public IActionResult Create_Items()
        {
            ViewBag.ListCategories = _dbContext.Categories.Where(
                category => category.IsDeleted == false).Select(category => new SelectListItem()
                {
                    Text = category.CategoryName,
                    Value = category.Id.ToString()
                }
                ).ToList();
            ViewBag.ListBrands = _dbContext.Brands.Where(
               category => category.IsDeleted == false).Select(category => new SelectListItem()
               {
                   Text = category.BrandName,
                   Value = category.Id.ToString()
               }
                ).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create_ItemSuccess(ItemVM items, List<IFormFile> files)
        {
            var entity = await _dbContext.Items.AddAsync(items.ToEntity());
            await _dbContext.SaveChangesAsync();
            if(files != null)           
                if (files.Count > 0)
                {
                    foreach(var file in files)
                    {
                        var imageId = Guid.NewGuid();
                        var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Items/{imageId}.png");
                        await using (var stream = System.IO.File.Create(Uploads))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var image = new Attachment()
                        {
                            RecordId = entity.Entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Items
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }

                }         

            return View();         
        }
        public async Task<IActionResult> Edit_Item(int id)
        {
            var ItemDB = await _dbContext.Items.FindAsync(id);
            var itemVM = new ItemVM()
            {
                Id = ItemDB.Id,
                Name = ItemDB.Name,
                Description = ItemDB.Description,
                ShortDescription = ItemDB.ShortDescription,
                Color = ItemDB.Color,
                CategoryId = ItemDB.CategoryId,
                BrandId = ItemDB.BrandId,
                Price = ItemDB.Price,
                Images = _dbContext.Attachments.Where(attachments => attachments.RecordId == ItemDB.Id.ToString() && attachments.RecordType == RecordType.Items).Select(attachments => attachments.FileName).ToList()
            };
              ViewBag.ListCategories = _dbContext.Categories.Where(
                category => category.IsDeleted == false).Select(category => new SelectListItem()
                {
                    Text = category.CategoryName,
                    Value = category.Id.ToString()
                }
                ).ToList();
               ViewBag.ListBrands = _dbContext.Brands.Where(
               category => category.IsDeleted == false).Select(category => new SelectListItem()
               {
                   Text = category.BrandName,
                   Value = category.Id.ToString()
               }
                ).ToList();
            return View(itemVM);
        }

        public IActionResult Edit_ItemSuccess()
        {
            var Items = _dbContext.Items.Where(item => item.IsDeleted == false).ToList().Select(item => new ItemVM()
            {
                Name = item.Name,
                Id = item.Id,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _dbContext.Attachments.FirstOrDefault(items => items.RecordId == item.Id.ToString() && items.RecordType == RecordType.Items)?.FileName
            });
            return View(Items);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Item(ItemVM items, List<IFormFile> files)
        {
            var itemDB = await _dbContext.Items.FindAsync(items.Id);
            itemDB.Name = items.Name;
            itemDB.Description = items.Description;
            itemDB.ShortDescription = items.ShortDescription;
            itemDB.Price = items.Price;
            itemDB.Color = items.Color;
            itemDB.CategoryId = items.CategoryId;
            itemDB.BrandId = items.BrandId;
            var entity = _dbContext.Items.Update(itemDB);
            await _dbContext.SaveChangesAsync();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var imageId = Guid.NewGuid();
                        var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Items/{imageId}.png");
                        await using (var stream = System.IO.File.Create(Uploads))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var image = new Attachment()
                        {
                            RecordId = entity.Entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Items
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }

                }
            return RedirectToAction("Edit_ItemSuccess");
        }
        public async Task<IActionResult> DeleteImageAsync(string id)
        {
            var attachment = _dbContext.Attachments.FirstOrDefault(attachments => attachments.FileName == id);
            var categoryId = Convert.ToInt32(attachment.RecordId);
            _dbContext.Attachments.Remove(attachment);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_ItemSuccess", new { id = categoryId });
        }

        //Delete
        public async Task<IActionResult> Delete_item(ItemVM Items)
        {
            var itemsDB = _dbContext.Items.SingleOrDefault(Item => Item.Id == Items.Id);
            itemsDB.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_ItemSuccess");
        }

       //Get Again
        public async Task<IActionResult> Get_Item_Again(ItemVM Items)
        {
            var itemsDB = _dbContext.Items.SingleOrDefault(Item => Item.Id == Items.Id);
            itemsDB.IsDeleted = false;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_ItemSuccess");
        }

        //Deleted Item
        public IActionResult DeletedItems()
        {
            var Items = _dbContext.Items.Where(item => item.IsDeleted == true).ToList().Select(item => new ItemVM()
            {
                Name = item.Name,
                Id = item.Id,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Price = item.Price,
                ImageId = _dbContext.Attachments.FirstOrDefault(items => items.RecordId == item.Id.ToString() && items.RecordType == RecordType.Items)?.FileName
            });
            return View(Items);
        }

        public IActionResult ContactUsindex()
        {
            var ContactList = _dbContext.ContactUs.ToList().Select(ContactList => new Vm.ContactUs()
            {
                FullName = ContactList.FullName,
                Subject = ContactList.Subject,
                Email = ContactList.Email,
                Message = ContactList.Message,
            });
            return View(ContactList);
        }

        public IActionResult Orderindex()
        {
            var orderList = _dbContext.Orders.ToList().Select(order => new ChekoutVM() 
            {
                Id=order.Id,
                ItemId=order.itemId,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email=order.Email,
                Phone=order.Phone,
                Address= order.Address,
                Country=order.Country,
                City=order.City,
                ZIPCode=order.ZIPCode,
                OrderStatus = order.OrderStatus,
            });
          return View(orderList);
        }

        public async Task<IActionResult> ChangeOrderStatus(OrderStatus status, int id)
        {
            var orders = _dbContext.Orders.FirstOrDefault(order => order.Id == id);     
             orders.OrderStatus = status;
             await _dbContext.SaveChangesAsync();           
            return RedirectToAction("Orderindex");
        }
    }
}
