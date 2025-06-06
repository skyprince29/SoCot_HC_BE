﻿using Microsoft.EntityFrameworkCore;
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
using SCHC_API.Handler;

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
                       ContactNo =p.ContactNo,
                       HouseholdId = p.FamilyMemberships
                        .Where(m => m.Family != null && m.Family.Household != null)
                        .Select(m => m.Family!.Household!.HouseholdId)
                        .FirstOrDefault(),
                       HouseholdNo = p.FamilyMemberships
                        .Where(m => m.Family != null && m.Family.Household != null)
                        .Select(m => m.Family!.Household!.HouseholdNo)
                        .FirstOrDefault(),
                       FamilyNo = p.FamilyMemberships
                        .Where(m => m.Family != null)
                        .Select(m => m.Family!.FamilyNo)
                        .FirstOrDefault(),

                       ResidentialAddress = p.AddressAsResidential == null ? null : new AddressDto
                       {
                           AddressId = p.AddressAsResidential.AddressId,
                           BarangayId = p.AddressAsResidential.BarangayId,
                           MunicipalityId = p.AddressAsResidential.MunicipalityId,
                           ProvinceId = p.AddressAsResidential.ProvinceId,
                           Sitio = p.AddressAsResidential.Sitio,
                           Purok = p.AddressAsResidential.Purok,
                           ZipCode = p.AddressAsResidential.ZipCode,
                           HouseNo = p.AddressAsResidential.HouseNo,
                           LotNo = p.AddressAsResidential.LotNo,
                           BlockNo = p.AddressAsResidential.BlockNo,
                           Street = p.AddressAsResidential.Street,
                           Subdivision = p.AddressAsResidential.Subdivision,
                           FullAddress = p.AddressAsResidential.FullAddress,
                       }

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

            // 🔥 Save Family Memberships
            if (person.FamilyMemberships != null && person.FamilyMemberships.Any())
            {
                foreach (var membership in person.FamilyMemberships)
                {
                    membership.PersonId = person.PersonId;
                    await _context.FamilyMembers.AddAsync(membership, cancellationToken);
                }
            }

            // 🔐 Commit all changes (very important!)
            await _context.SaveChangesAsync(cancellationToken);
        }

        private void ValidateFields(Person person)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(person.Firstname), person.Firstname, "Firstname");
            ValidationHelper.IsRequired(errors, nameof(person.Lastname), person.Lastname, "Lastname");
            ValidationHelper.IsRequired(errors, nameof(person.BirthDate), person.BirthDate, "Birthdate");
            ValidationHelper.IsRequired(errors, nameof(person.Gender), person.Gender, "Gender");
            ValidationHelper.IsRequired(errors, nameof(person.CivilStatus), person.CivilStatus, "CivilStatus");
            ValidationHelper.IsRequired(errors, nameof(person.ContactNo), person.ContactNo, "ContactNo");

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

        public async Task<bool> CheckIfPersonExistsAsync(string firstname, string lastname, DateTime birthDate, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(p =>
                p.Firstname.ToLower() == firstname.ToLower().Trim() &&
                p.Lastname.ToLower() == lastname.ToLower().Trim() &&
                p.BirthDate.Date == birthDate.Date,
                cancellationToken);
        }

        public async Task<PaginationHandler<Person>> GetAllPersonWithUserAccount(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {

            var userAccountQuery = _context.Set<UserAccount>().AsQueryable();
            userAccountQuery = userAccountQuery.Include(ua => ua.PersonAsUserAccount);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower(); // Convert keyword to lowercase once for case-insensitive comparison

                userAccountQuery = userAccountQuery.Where(ua =>
                    // Check if the associated Person exists and its name properties contain the keyword
                    ua.PersonAsUserAccount != null && (
                        (ua.PersonAsUserAccount.Firstname != null && ua.PersonAsUserAccount.Firstname.ToLower().Contains(lowerKeyword)) ||
                        (ua.PersonAsUserAccount.Middlename != null && ua.PersonAsUserAccount.Middlename.ToLower().Contains(lowerKeyword)) ||
                        (ua.PersonAsUserAccount.Lastname != null && ua.PersonAsUserAccount.Lastname.ToLower().Contains(lowerKeyword))
                    )
                );
            }

            var personsQuery = userAccountQuery.Select(ua => ua.PersonAsUserAccount).Distinct();
            int totalRecords = await personsQuery.CountAsync(cancellationToken);

            var personsList = await personsQuery
           .Skip((pageNo - 1) * limit) // Skip records from previous pages
           .Take(limit)                // Take the specified number of records for the current page
           .ToListAsync(cancellationToken); // Execute and materialize the results

            var paginatedResult = new PaginationHandler<Person>(personsList!, totalRecords, pageNo, limit);

            return paginatedResult!;
        }
    }
}
