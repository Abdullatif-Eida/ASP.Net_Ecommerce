using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using Ecommerce.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class BrandsController : Controller
    {
        public EcommerceContext _dbContext { get; set; }
        public readonly IHostingEnvironment _hostingEnvironment;

        public BrandsController(EcommerceContext context, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var listOfBrands = _dbContext.Brands.
                Where(brands => brands.IsDeleted == false).
                Select(brands => new BrandVM() { 
                    Name = brands.BrandName,
                    ImageId = brands.ImageId,
                    Description = brands.Description,
                    CreationDate = brands.CreationDate,
                    Id = brands.Id,
                }
                    ).ToList();
            return View(listOfBrands);
        }

        public IActionResult Create_Brand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create_BrandSuccess(BrandVM brands, IFormFile file)
        {
            var newBrands = new Models.Brand() { BrandName = brands.Name, Description = brands.Description };
            if(file != null)           
                if (file.Length > 0)
                {
                    var imageId = Guid.NewGuid();
                    var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Brands/{imageId}.png");
                    using (var stream = System.IO.File.Create(Uploads))
                    {
                        await file.CopyToAsync(stream);
                    }
                    newBrands.ImageId = imageId.ToString();
                }         
             await _dbContext.Brands.AddAsync(newBrands);
             await _dbContext.SaveChangesAsync();
             return View();         
        }

        public IActionResult Edit_BrandSuccess()
        {
            var listOfBrands = _dbContext.Brands.
                 Where(brands => brands.IsDeleted == false).
                 Select(brands => new BrandVM()
                 {
                     Name = brands.BrandName,
                     Description = brands.Description,
                     CreationDate = brands.CreationDate,
                     Id = brands.Id,
                     ImageId = brands.ImageId,
                 }
                     ).ToList();
            return View(listOfBrands);
        }
        public IActionResult Edit_Brand(int id)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(brands => brands.Id==id);
            var brand = new BrandVM()
            {
                Id = brandDb.Id,
                Name = brandDb.BrandName,
                Description = brandDb.Description,
                ImageId = brandDb.ImageId,
            };
            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_BrandAsync(BrandVM brand, IFormFile file)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(brands => brands.Id == brand.Id);
            brandDb.BrandName = brand.Name;
            brandDb.Description = brand.Description;
            brandDb.LastModify = DateTime.Now;
            if (file != null)
                if (file.Length > 0)
                {
                    var imageId = Guid.NewGuid();
                    var Uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"Imageuploads/Brands/{imageId}.png");
                    using (var stream = System.IO.File.Create(Uploads))
                    {
                        await file.CopyToAsync(stream);
                    }
                    brandDb.ImageId = imageId.ToString();
                }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_BrandSuccess");
        }

        //Delete
        public async Task<IActionResult> Delete_Brand(BrandVM brand)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(brands => brands.Id == brand.Id);
            brandDb.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_BrandSuccess");
        }

       //Get Again
        public async Task<IActionResult> Get_Brand_Again(BrandVM brand)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(brands => brands.Id == brand.Id);
            brandDb.IsDeleted = false;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_BrandSuccess");
        }

        // Final Delete
        public async Task<IActionResult> FinalDelete_Brand(BrandVM brand)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(brands => brands.Id == brand.Id);
            _dbContext.Brands.Remove(brandDb);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit_BrandSuccess");
        }

        //Deleted Brands
        public IActionResult DeletedBrands()
        {
            var listOfBrands = _dbContext.Brands.
                Where(brands => brands.IsDeleted == true).
                Select(brands => new BrandVM()
                {
                    Name = brands.BrandName,
                    ImageId = brands.ImageId,
                    Description = brands.Description,
                    CreationDate = brands.CreationDate,
                    Id = brands.Id,
                }
                    ).ToList();
            return View(listOfBrands);
        }
    }
}
