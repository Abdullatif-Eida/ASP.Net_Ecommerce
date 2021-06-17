using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Vm
{
    public class HomeVM
    {
        public List<ItemVM> ListCollections { get; set; }
        public List<ItemVM> ListItems { get; set; }
        public List<CategoryVM> ListCategoryItems { get; set; }
        public List<CategoryVM> ListCategory { get; set; }

        public List<BrandVM> ListBrands { get; set; }
    }
}
