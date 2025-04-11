namespace SoCot_HC_BE.Repositories.Intefaces
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<T?> GetAsync(TKey id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
    }
}
