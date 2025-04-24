using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IAddressService : IRepository<Address, Guid>
    {
        Task<Address?> GetExistingAddressAsync(Address address, CancellationToken cancellationToken = default);
        Task SaveAddressAsync(Address address, CancellationToken cancellationToken = default);
        Task<Address> GetOrCreateAddressAsync(Address address, CancellationToken cancellationToken = default);
        void ValidateAddress(Address? address, Dictionary<string, List<string>> errors);
    }
}                                                                