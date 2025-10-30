using MISA.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IBaseRepo<T, FilterBase, PageResult> 
        where T : class
        where FilterBase : FilterBaseDto
        where PageResult : PageResultDto<T>
    {

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="item">Thêm mới</param>
        /// <returns>Thông tin thêm mới</returns>
        /// CreatedBy: (NDH 22/10/2025)
        T Insert(T entity);

        T? FindById(Guid id);
        List<T> GetAll();
        int Delete(Guid id);
        int Update(T entity);
        PageResult Paging(FilterBase filter);
    }
}
