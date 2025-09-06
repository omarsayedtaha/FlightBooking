using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public abstract class BaseServices<T, TEntity>
    {
        public abstract IQueryable<T> ApplyFilter(IQueryable<T> query, BaseRequest<TEntity> request);
        public abstract IQueryable<T> ApplySearch(IQueryable<T> query, string search);

        public abstract IQueryable<T> ApplyPagination(IQueryable<T> query, int PageIndex, int PageSize);

    }
}
