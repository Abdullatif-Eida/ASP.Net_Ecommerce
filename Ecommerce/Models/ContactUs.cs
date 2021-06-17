using Ecommerce.Vm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static Ecommerce.Vm.ChekoutVM;

namespace Ecommerce.Models
{
    public class ContactUs: BaseClass<int>
    {
        public String FullName { get; set; }
        public String Subject { get; set; }
        public String Email { get; set; }
        public String Message { get; set; }
    }
}
