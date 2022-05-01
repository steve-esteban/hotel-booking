using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess.EF.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(List<string> includeStrings = null);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, List<string> includeStrings = null);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, List<string> includeStrings = null);

        void Add(T entity);

        Task AddAsync(T entity);

        Task SaveChangesAsync();

    }
}