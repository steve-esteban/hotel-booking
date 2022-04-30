namespace HotelBooking.DataAccess.EF.Repositories
{
    public interface IRepositoryFactory
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}