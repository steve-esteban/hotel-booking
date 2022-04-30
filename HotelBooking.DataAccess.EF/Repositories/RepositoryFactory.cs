namespace HotelBooking.DataAccess.EF.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly HotelBookingContext _dbContext;

        public RepositoryFactory(HotelBookingContext context)
        {
            _dbContext = context;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }
    }
}