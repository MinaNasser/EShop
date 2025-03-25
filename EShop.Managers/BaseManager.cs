using EF_Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EShop.Manegers
{

    public class BaseManager<T> where T : class
    {

        private readonly EShopContext dbcontext;
        private DbSet<T> table;
        public BaseManager(EShopContext _eShop)
        {
            //DI
            //dbcontext = new EShopContext();
            this.dbcontext = _eShop;
            table = dbcontext.Set<T>();
        }

        public IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            int pageSize = 4,
            int pageNumber = 1)
        {
            IQueryable<T> quary = table.AsQueryable();

            if (filter != null)
                quary = quary.Where(filter);

            //Pagination
            if (pageSize < 0)
                pageSize = 4;

            if (pageNumber < 0)
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


        public IQueryable<T> GetList(
            Expression<Func<T, bool>> filter = null)
        {
            if(filter == null)return table.AsQueryable();
            else return table.Where(filter);
        }
        public void Add(T newRow)
        {
            table.Add(newRow);
            Commit();
        }
        public void Edit(T newRow)
        {
            table.Update(newRow);
            Commit();
        }
        public void Delete(T row)
        {
            table.Remove(row);
            Commit();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await table.FirstOrDefaultAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await table.CountAsync(filter);
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }
        public async Task<int> Commit()
        {
            return await dbcontext.SaveChangesAsync();
        }


    }
}
