using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Categories:BaseClass<int>
    {
        public String CategoryName { get; set; }
        public String CategoryShortDescription { get; set; }
        public String Description { get; set; }

        [ForeignKey("ParentId")]
        public int? ParentId { get; set; }
        public Categories Parent { get; set; }


    }
}
