using Ecommerce.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Items:BaseClass<int>
    {
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public Color Color { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Categories Category { get; set; }

        [ForeignKey("BrandId")]
        public int BrandId { get; set; }
        public Brand Brands { get; set; }
    }
}
