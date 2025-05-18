using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Persons.Interfaces
{
    public interface IPersonService : IRepository<Person, Guid>
    {
        // Get a list of Persons with paging, using CancellationToken for async cancellation support.
        Task<List<PersonDto>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of Person, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        // Save Person
        Task SavePersonAsync(Person person, CancellationToken cancellationToken = default);

        // Get Person based on Id
        Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Get Person details by Id
        Task<PersonDetailsDto?> GetPersonDetailsAsync(Guid personId, CancellationToken cancellationToken = default);

        // Check Existing Person
        Task<bool> CheckIfPersonExistsAsync(string firstname, string lastname, DateTime birthDate, CancellationToken cancellationToken = default);

    }
}