using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class AddressService : Repository<Address, Guid>, IAddressService
    {
        public AddressService(AppDbContext context) : base(context)
        {
        }

        public async Task<Address?> GetExistingAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(a =>
                a.BarangayId == address.BarangayId &&
                a.MunicipalityId == address.MunicipalityId &&
                a.ProvinceId == address.ProvinceId &&
                a.Sitio == address.Sitio &&
                a.Purok == address.Purok &&
                a.ZipCode == address.ZipCode &&
                a.HouseNo == address.HouseNo &&
                a.LotNo == address.LotNo &&
                a.BlockNo == address.BlockNo &&
                a.Street == address.Street &&
                a.Subdivision == address.Subdivision,
                cancellationToken);
        }

        public async Task SaveAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            address.AddressId = Guid.NewGuid();
            await AddAsync(address, cancellationToken);
        }

        public async Task<Address> GetOrCreateAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            var existingAddress = await GetExistingAddressAsync(address, cancellationToken);
            if (existingAddress != null)
            {
                return existingAddress;
            }

            // Save new address
            await SaveAddressAsync(address, cancellationToken);

            // Return the saved address with full tracking (optional)
            return address;
        }

        public void ValidateAddress(Address? address, Dictionary<string, List<string>> errors)
        {
            if (address == null)
            {
                ValidationHelper.AddError(errors, "Address", "Address is required.");
                return;
            }

            ValidationHelper.IsRequired(errors, "BarangayId", address.BarangayId, "Barangay");
            ValidationHelper.IsRequired(errors, "MunicipalityId", address.MunicipalityId, "Municipality");
            ValidationHelper.IsRequired(errors, "ProvinceId", address.ProvinceId, "Province");
            // You can add more validations here if needed
        }

    }
}
