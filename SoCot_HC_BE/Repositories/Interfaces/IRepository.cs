namespace SoCot_HC_BE.Repositories.Interfaces
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<T?> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
