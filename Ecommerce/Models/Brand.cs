using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Brand:BaseClass<int>
    {
        public String BrandName { get; set; }
        public String Description { get; set; }
        public String ImageId { get; set; }


    }
}
