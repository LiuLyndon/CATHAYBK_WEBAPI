namespace BasicEIP_Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }


    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task SaveChangesAsync();
    }
}