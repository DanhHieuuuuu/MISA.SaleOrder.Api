using MISA.Core.Dtos;
using MISA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface ICustomerRepo : IBaseRepo<Customer, FilterCustomerDto, PageResultDto<Customer>>
    {
        bool CheckCustomerCode(string customerCode);
    }
}
