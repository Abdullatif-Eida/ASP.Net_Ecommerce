using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Vm
{
    public class ChekoutVM
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemVM Item { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String ZIPCode { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Preparation,
        Delivery,
        AtAddress,
    }
}
