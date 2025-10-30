using MISA.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    [MISATableName("customer")]
    public class Customer
    {
        [MISAKey]
        [MISAColumnName("customer_id")]
        public Guid CustomerId { get; set; }

        [MISAKeyword]
        [MISAColumnName("customer_code")]
        public string CustomerCode { get; set; }
        [MISAColumnName("customer_name")]
        public string CustomerName { get; set; }
        [MISAColumnName("customer_addr")]
        public string CustomerAddr { get; set; }
    }
}
