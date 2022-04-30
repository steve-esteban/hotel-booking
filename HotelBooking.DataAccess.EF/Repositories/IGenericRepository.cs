using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess.EF.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        Task<bool> SaveChangesAsync();

    }
}