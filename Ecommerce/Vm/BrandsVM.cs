using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce
{
    public class BrandVM
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ImageId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
