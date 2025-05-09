using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Models.Enums;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SoCot_HC_BE.DTO;

namespace SoCot_HC_BE.Services
{
    public class PersonService : Repository<Person, Guid>, IPersonService
    {
        //private readonly IPersonService _personService;

        public PersonService(AppDbContext context) : base(context)
        {
        }

        public async Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //return await _dbSet.FirstOrDefaultAsync(p => p.PersonId == id, cancellationToken);
            return await _dbSet
         .Include(p => p.AddressAsResidential!)
             .ThenInclude(a => a.Barangay)
         .Include(p => p.AddressAsResidential!)
             .ThenInclude(a => a.Municipality)
         .Include(p => p.AddressAsResidential!)
             .ThenInclude(a => a.Province)
         .FirstOrDefaultAsync(p => p.PersonId == id, cancellationToken);
        }


        public override async Task<List<Person>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken); 
        }

        public async Task<List<PersonDto>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
           .Include(p => p.FamilyMemberships)
               .ThenInclude(m => m.Family)
                   .ThenInclude(f => f!.Household)
           .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p =>
                    p.Firstname.Contains(keyword) ||
                    p.Lastname.Contains(keyword) ||
                    (p.Middlename != null && p.Middlename.Contains(keyword)));
            }

            return await query
                   .OrderBy(p => p.Lastname)
                   .Skip((pageNo - 1) * limit)
                   .Take(limit)
                   .Select(p => new PersonDto
                   {
                       PersonId = p.PersonId,
                       Firstname = p.Firstname,
                       Middlename = p.Middlename,
                       Lastname = p.Lastname,
                       BirthDate = p.BirthDate,
                       Gender = p.Gender,
                       HouseholdNo = p.FamilyMemberships
                        .Where(m => m.Family != null && m.Family.Household != null)
                        .Select(m => m.Family!.Household!.HouseholdNo)
                        .FirstOrDefault(),
                                           FamilyNo = p.FamilyMemberships
                        .Where(m => m.Family != null)
                        .Select(m => m.Family!.FamilyNo)
                        .FirstOrDefault(),

                   })
                   .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p =>
                    p.Firstname.Contains(keyword) ||
                    p.Lastname.Contains(keyword) ||
                    (p.Middlename != null && p.Middlename.Contains(keyword)));
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task SavePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            bool isNew = person.PersonId == Guid.Empty;
            ValidateFields(person);

            if (isNew)
            {
                person.PersonId = Guid.NewGuid();
                await AddAsync(person, cancellationToken);
            }
            else
            {
                var existing = await _dbSet
                    .FirstOrDefaultAsync(p => p.PersonId == person.PersonId, cancellationToken);

                if (existing == null)
                    throw new Exception("Person not found.");

                _context.Entry(existing).CurrentValues.SetValues(person);
                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(Person person)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(person.Firstname), person.Firstname, "Firstname");
            ValidationHelper.IsRequired(errors, nameof(person.Lastname), person.Lastname, "Lastname");
            ValidationHelper.IsRequired(errors, nameof(person.BirthDate), person.BirthDate, "Birthdate");
            ValidationHelper.IsRequired(errors, nameof(person.BirthPlace), person.BirthPlace, "Birth Place");
            ValidationHelper.IsRequired(errors, nameof(person.ContactNo), person.ContactNo, "Contact No");
            ValidationHelper.IsRequired(errors, nameof(person.Email), person.Email, "Email");
            ValidationHelper.IsRequired(errors, nameof(person.Citizenship), person.Citizenship, "Citizenship");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

        public async Task<PersonDetailsDto?> GetPersonDetailsAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            var person = await _dbSet
                .Include(p => p.AddressAsResidential)
                .FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken);

            if (person == null)
            {
                return null; // Person not found
            }
            else 
            {
                if (person.AddressAsResidential != null)
                {
                    // Ensure Barangay is assigned
                    if (person.AddressAsResidential.Barangay == null)
                    {
                        person.AddressAsResidential.Barangay = await _context.Barangay
                            .FirstOrDefaultAsync(b => b.BarangayId == person.AddressAsResidential.BarangayId, cancellationToken);
                    }

                    // Ensure Municipality is assigned
                    if (person.AddressAsResidential.Municipality == null)
                    {
                        person.AddressAsResidential.Municipality = await _context.Municipality
                            .FirstOrDefaultAsync(m => m.MunicipalityId == person.AddressAsResidential.MunicipalityId, cancellationToken);
                    }

                    // Ensure Province is assigned
                    if (person.AddressAsResidential.Province == null)
                    {
                        person.AddressAsResidential.Province = await _context.Province
                            .FirstOrDefaultAsync(p => p.ProvinceId == person.AddressAsResidential.ProvinceId, cancellationToken);
                    }
                }

                 var fullAddress = person.AddressAsResidential?.FullAddress;

                // Return the required details
                return new PersonDetailsDto
                {
                    FullName = person.Fullname,
                    Gender = person.Gender,
                    Age = CalculateAge(person.BirthDate),
                    ContactNumber = person.ContactNo,
                    FullAddress = fullAddress
                };
            }
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}
