using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce
{
    public class ItemVM
    {
        public List<ItemVM> ListCollections { get; set; }
        public List<ItemVM> ListItems { get; set; }
        public List<CategoryVM> ListCategoryItems { get; set; }
        public List<CategoryVM> ListCategory { get; set; }
        public List<BrandVM> ListBrands { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public Color Color { get; set; }
        public String ImageId { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Categories Category { get; set; }
        public String BrandName { get; set; }
        public String CategoryName { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public Brand Brands { get; set; }
        public List <String>  Images{ get; set; }
        public Items ToEntity()
        {
            return new Items()
            {
                Name = Name,
                Id = Id,
                Description = Description,
                ShortDescription = ShortDescription,
                Price = Price,
                Color = Color,
                CategoryId= CategoryId,
                BrandId= BrandId,
            };
        }
    }
}

