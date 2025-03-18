using EF_Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShop.Manegers
{

    public class BaseManager<T> where T : class
    {

        private readonly EShopContext dbcontext;
        private DbSet<T> tables;
        public BaseManager()
        {
            //DI
            dbcontext = new EShopContext();
            tables = dbcontext.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T,bool>> filter = null, int pageSize = 4 ,int pageNumber = 1)
        {
            IQueryable<T> quary = tables.AsQueryable();

            if (filter != null)
                quary = quary.Where(filter);

            if(pageSize < 0)
                pageSize = 4;

            if(pageNumber < 0)
                pageNumber = 1;
            

            int count = quary.Count();

            if (count < pageSize)
            {
                pageSize = count;
                pageNumber = 1;
            }
            
            int ToSkip = (pageNumber - 1) * pageSize;

            quary = quary.Skip(ToSkip).Take(pageSize);

            return quary;
        }
    }
}
