using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ShortDescription { get; set; }
        public String ImageId { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ParentId { get; set; }
        public List<String> Images { get; set; }
        public Categories ToEntity()
        {
            return new Categories()
            {
                CategoryName = Name,
                Id = Id,
                Description = Description,
                CategoryShortDescription = ShortDescription,
                ParentId = ParentId
            };
        }
    }
}
