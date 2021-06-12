using Ecommerce.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Attachment:BaseClass<Guid>
    {
        public String FileName { get; set; }
        public String RecordId { get; set; }
        public RecordType RecordType { get; set; }
        public String Ext { get; set; }

    }
}
