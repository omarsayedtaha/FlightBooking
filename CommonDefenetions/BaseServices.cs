using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions
{
    public abstract class BaseServices<T,TEntity>
    {
        public abstract IQueryable<T> ApplyFilter(IQueryable<T> query, BaseRequest<TEntity> request);
        public abstract IQueryable<T> ApplySearch(IQueryable<T> query, string search);

    }
}
