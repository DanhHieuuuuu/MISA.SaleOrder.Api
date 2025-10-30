using MISA.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Service
{
    public interface IBaseService<T, FilterBase, PageResult> 
        where T : class
        where FilterBase : FilterBaseDto
        where PageResult : PageResultDto<T>
    {
        List<T> GetAll();
        T FindById(Guid id);
        T Insert(T entity);
        int Update(T entity);
        int Delete(Guid id);

        PageResult Paging(FilterBase filter);
    }
}
