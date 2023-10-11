using Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        Task<bool> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<T> GetDefaultAsync(Expression<Func<T, bool>> predicate); // id ye göre T gelecek

        //GetFilteredFirstOrDefault => Tek bir kayıt döndürecek
        //GetFilteredList => Liste döndürecek

        Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<List<TResult>> GetFilteredListAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<List<T>> GetAllAsync();
    }
}
