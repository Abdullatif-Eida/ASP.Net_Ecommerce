using Ecommerce.Vm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static Ecommerce.Vm.ChekoutVM;

namespace Ecommerce.Models
{
    public class Orders: BaseClass<int>
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String ZIPCode { get; set; }
        public int itemId { get; set; }
        [ForeignKey("itemId")]
        public Items Items { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
