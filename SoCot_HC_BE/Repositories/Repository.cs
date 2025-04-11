using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Repositories.Intefaces;

namespace SoCot_HC_BE.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        // Constructor that accepts the AppDbContext
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Get a single entity by its ID
        public async Task<T?> GetAsync(TKey id)
        {
            return await _dbSet.FindAsync(id); // This will now return null if not found
        }

        // Get all entities
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Add a new entity
        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Update an existing entity
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Delete an entity by its ID
        public async Task DeleteAsync(TKey id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            T? entity = await _dbSet.FindAsync(id);  // Now this can be null
            if (entity == null)
            {
                throw new ArgumentException("Entity not found", nameof(id));
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }


}
