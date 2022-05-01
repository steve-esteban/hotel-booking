using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBooking.DataAccess.EF.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HotelBookingContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HotelBookingContext context)
        {
            _context = context ?? throw new ArgumentNullException($"{context}");
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(List<string> includeStrings = null)
        {
            IQueryable<T> queryable = _dbSet;
            includeStrings?.ForEach(s => queryable = queryable.Include(s));
            return await queryable.ToListAsync();
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, List<string> includeStrings = null)
        {
            IQueryable<T> queryable = _dbSet;
            includeStrings?.ForEach(s => queryable = queryable.Include(s));
            return await queryable.Where(filter).ToListAsync();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, List<string> includeStrings = null)
        {
            IQueryable<T> queryable = _dbSet;
            includeStrings?.ForEach(s => queryable = queryable.Include(s));
            return await queryable.SingleOrDefaultAsync(predicate);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}