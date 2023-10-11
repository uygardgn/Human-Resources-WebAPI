using Domain.Entities.Abstract;
using Domain.Repositories;
using Infrastructer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        private readonly AppDbContext dbContext;
        protected DbSet<T> table;

        public BaseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<T>();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            await table.AddAsync(entity);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            table.Remove(entity);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<T> GetDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await table.FirstOrDefaultAsync(predicate);
        }

        public async Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table; //select*from posts
            if (where != null) //select*from where genreId=2
            {
                query = query.Where(where);
            }
            if (include != null) //join
            {
                query = include(query);
            }
            if (orderBy != null) //orderBy
            {
                return await orderBy(query).Select(select).SingleOrDefaultAsync();
            }
            else
            {
                return await query.Select(select).SingleOrDefaultAsync();
            }
        }

        public async Task<List<TResult>> GetFilteredListAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                return await orderBy(query).Select(select).ToListAsync();
            }
            else
            {
                return await query.Select(select).ToListAsync();
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            dbContext.Entry<T>(entity).State = EntityState.Modified;
            return await dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }
    }
}
