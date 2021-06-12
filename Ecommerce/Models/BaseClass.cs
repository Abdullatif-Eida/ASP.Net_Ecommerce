using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class BaseClass<T>
    {
        public BaseClass()
        {
            CreationDate = DateTime.Now;
            IsDeleted = false;
        }
        [Key]
        public T Id{ get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModify { get; set; }
        public bool IsDeleted { get; set; }

    }
}
