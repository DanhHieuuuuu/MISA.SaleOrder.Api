using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Dtos
{
    public class PageResultDto<T>
    {
        public List<T>? Results { get; set; }
        public int TotalItem { get; set; }
    }
}
