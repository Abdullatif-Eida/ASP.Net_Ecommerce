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
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        public EcommerceContext _dbContext { get; set; }
        public readonly IHostingEnvironment _hostingEnvironment;

        public CategoriesController(EcommerceContext context, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var listOfCategories = _dbContext.Categories.
                Where(categories => categories.IsDeleted == false).
                Select(categories => new CategoryVM() { 
                    Name = categories.CategoryName,
                    Description = categories.Description,
                    CreationDate = categories.CreationDate,
                    Id = categories.Id,
                    ImageId = _dbContext.Attachments
                        .FirstOrDefault(image=>image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
                        .FileName

                }
                    ).ToList();
            return View(listOfCategories);
        }

        public IActionResult Create_Category()
        {
            ViewBag.ListParentCategories = _dbContext.Categories.Where(
                category => category.IsDeleted == false).Select(category => new SelectListItem()
                {
                   Text= category.CategoryName,Value= category.Id.ToString()
                }
                ).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create_CategorySuccess(CategoryVM categories, List<IFormFile> files)
        {
            var entity = await _dbContext.Categories.AddAsync(categories.ToEntity());
            await _dbContext.SaveChangesAsync();
            if(files != null)           
                if (files.Count > 0)
                {
                    foreach(var file in files)
                    {
                        var imageId = Guid.NewGuid();
                        var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Categories/{imageId}.png");
                        await using (var stream = System.IO.File.Create(Uploads))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var image = new Attachment()
                        {
                            RecordId = entity.Entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Categories
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }

                }         

            return View();         
        }
        public async Task<IActionResult> Edit_Category(int id)
        {
            var CategoryDB = await _dbContext.Categories.FindAsync(id);
            var categoryVM = new CategoryVM()
            {
                Id = CategoryDB.Id,
                Name = CategoryDB.CategoryName,
                Description = CategoryDB.Description,
                ShortDescription = CategoryDB.CategoryShortDescription,
                ParentId = CategoryDB.ParentId,
            };
            var listOfImages = _dbContext.Attachments.Where(attachments => attachments.RecordId == CategoryDB.Id.ToString() && attachments.RecordType == RecordType.Categories)
                .Select(attachments => attachments.FileName);
            categoryVM.Images = listOfImages.ToList();
            ViewBag.ListParentCategories = _dbContext.Categories.Where(
               category => category.IsDeleted == false).Select(category => new SelectListItem()
               {
                   Text = category.CategoryName,
                   Value = category.Id.ToString()
               }
               ).ToList();
            return View(categoryVM);
        }

        public IActionResult Edit_CategorySuccess()
        {
            var listOfCategories = _dbContext.Categories.
                  Where(categories => categories.IsDeleted == false).
                  Select(categories => new CategoryVM()
                  {
                      Name = categories.CategoryName,
                      Description = categories.Description,
                      CreationDate = categories.CreationDate,
                      Id = categories.Id,
                      ImageId = _dbContext.Attachments
                          .FirstOrDefault(image => image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
                          .FileName
                  }
                      ).ToList();
            return View(listOfCategories);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Category(CategoryVM categories, List<IFormFile> files)
        {
            var entity = await _dbContext.Categories.FindAsync(categories.Id);
            entity.CategoryName = categories.Name;
            entity.Description = categories.Description;
            entity.CategoryShortDescription = categories.ShortDescription;
            entity.ParentId = categories.ParentId;
            await _dbContext.SaveChangesAsync();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var imageId = Guid.NewGuid();
                        var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Categories/{imageId}.png");
                        await using (var stream = System.IO.File.Create(Uploads))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var image = new Attachment()
                        {
                            RecordId = entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Categories
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }

                }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_CategorySuccess");
        }
        public async Task<IActionResult> DeleteImageAsync(string id)
        {
            var attachment = _dbContext.Attachments.FirstOrDefault(attachments => attachments.FileName == id);
            var categoryId = Convert.ToInt32(attachment.RecordId);
            _dbContext.Attachments.Remove(attachment);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_Category", new { id = categoryId });
        }

        //Delete
        public async Task<IActionResult> Delete_Category(CategoryVM categories)
        {
            var categoryDb = _dbContext.Categories.SingleOrDefault(category => category.Id == categories.Id);
            categoryDb.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_CategorySuccess");
        }

       //Get Again
        public async Task<IActionResult> Get_Category_Again(BrandVM category)
        {
            var categoryDb = _dbContext.Categories.SingleOrDefault(categories => categories.Id == category.Id);
            categoryDb.IsDeleted = false;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_CategorySuccess");
        }

        //Deleted Category
        public IActionResult DeletedCategories()
        {
            var listOfCategories = _dbContext.Categories.
                Where(categories => categories.IsDeleted == true).
                Select(categories => new CategoryVM()
                {
                    Name = categories.CategoryName,
                    Description = categories.Description,
                    CreationDate = categories.CreationDate,
                    Id = categories.Id,
                    ImageId = _dbContext.Attachments
                        .FirstOrDefault(image => image.RecordId == categories.Id.ToString() && image.RecordType == RecordType.Categories)
                        .FileName

                }
                    ).ToList();
            return View(listOfCategories);
        }
    }
}
