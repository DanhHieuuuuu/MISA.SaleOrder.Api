using MISA.Core.Dtos;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using MISA.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class BaseService<T, FilterBase, PageResult> : IBaseService<T, FilterBase, PageResult> 
        where T : class
        where FilterBase : FilterBaseDto
        where PageResult : PageResultDto<T>
    {
        protected IBaseRepo<T, FilterBase, PageResult> _repo;

        public BaseService(IBaseRepo<T, FilterBase, PageResult> repo)
        {
            _repo = repo;
        }
        public int Delete(Guid id)
        {
            return _repo.Delete(id);
        }

        public T FindById(Guid id)
        {
            return _repo.FindById(id);
        }

        public List<T> GetAll()
        {
            return _repo.GetAll();
        }

        public virtual T Insert(T entity)
        {
            return _repo.Insert(entity);
        }

        public PageResult Paging(FilterBase filter)
        {
            return _repo.Paging(filter);
        }

        public virtual int Update(T entity)
        {
            return _repo.Update(entity);
        }
    }
}
