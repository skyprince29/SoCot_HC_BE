﻿using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Utils;
using SoCot_HC_BE.DTO;

public class HouseholdService : IHouseholdService
{
    private readonly AppDbContext _context;
    private readonly DbSet<Household> _dbSet;

    public HouseholdService(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Household>();
    }

    public async Task<Household?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
        .Include(h => h.PersonAsHeadOfHousehold)
        .Include(h => h.Address)
            .ThenInclude(a => a.Barangay)
        .Include(h => h.Address)
            .ThenInclude(a => a.Municipality)
        .Include(h => h.Address)
            .ThenInclude(a => a.Province)
       .Include(h => h.Families)
            .ThenInclude(f => f.Person)
       .Include(h => h.Families)
            .ThenInclude(f => f.FamilyMembers)
            .ThenInclude(fm => fm.Person)
        .FirstOrDefaultAsync(h => h.HouseholdId == id, cancellationToken);
    }


    public async Task<List<Household>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(h => h.PersonAsHeadOfHousehold)
            .Include(h => h.Address)
                .ThenInclude(a => a.Barangay)
            .Include(h => h.Address)
                .ThenInclude(a => a.Municipality)
            .Include(h => h.Address)
                .ThenInclude(a => a.Province)
            .Include(h => h.Families)
            .ToListAsync(cancellationToken);

    }

    public async Task AddAsync(Household entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Household entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var household = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (household != null)
        {
            _dbSet.Remove(household);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<Household>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
             .Include(h => h.PersonAsHeadOfHousehold)
             .Include(h => h.Address).ThenInclude(a => a.Barangay)
             .Include(h => h.Address).ThenInclude(a => a.Municipality)
             .Include(h => h.Address).ThenInclude(a => a.Province)
             .Include(h => h.Families)
             .AsQueryable();


        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(h => h.HouseholdNo.Contains(keyword));
        }

        return await query
            .Skip((pageNo - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(h => h.HouseholdNo.Contains(keyword));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task SavePersonDetails(SaveHouseholdRequest request, CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, List<string>>();

        if (request == null)
            throw new ModelValidationException("Request is empty", errors);

        if (request.Families == null || !request.Families.Any())
            throw new ModelValidationException("At least one family is required.", errors);

        // 1️⃣ Validate Persons first
        foreach (var familyReq in request.Families)
        {
            foreach (var personReq in familyReq.Persons)
            {
                if (string.IsNullOrWhiteSpace(personReq.Firstname))
                    AddValidationError(errors, $"Families[{request.Families.IndexOf(familyReq)}].Persons[{familyReq.Persons.IndexOf(personReq)}].Firstname", "The Firstname field is required.");
                if (string.IsNullOrWhiteSpace(personReq.Lastname))
                    AddValidationError(errors, $"Families[{request.Families.IndexOf(familyReq)}].Persons[{familyReq.Persons.IndexOf(personReq)}].Lastname", "The Lastname field is required.");
                if (string.IsNullOrWhiteSpace(personReq.Birthdate))
                    AddValidationError(errors, $"Families[{request.Families.IndexOf(familyReq)}].Persons[{familyReq.Persons.IndexOf(personReq)}].Birthdate", "The Birthdate field is required.");
            }
        }

        if (errors.Any())
            throw new ModelValidationException("Validation failed", errors);

        // 2️⃣ Insert Address first
        var address = new Address
        {
            AddressId = Guid.NewGuid(),
            HouseNo = request.Address.HouseNo,
            LotNo = request.Address.LotNo,
            BlockNo = request.Address.BlockNo,
            Street = request.Address.Street,
            Subdivision = request.Address.Subdivision,
            Sitio = request.Address.Sitio,
            ProvinceId = request.Address.ProvinceId,
            MunicipalityId = request.Address.MunicipalityId,
            BarangayId = request.Address.BarangayId
        };
        await _context.Addresses.AddAsync(address, cancellationToken);

        // 3️⃣ Insert all Persons first
        var insertedPersons = new List<Person>();
        foreach (var familyReq in request.Families)
        {
            if (familyReq.Persons == null || !familyReq.Persons.Any())
                throw new ModelValidationException($"Family {request.Families.IndexOf(familyReq)} must have at least one person.", errors);

            foreach (var personReq in familyReq.Persons)
            {
              var person = new Person
              {
                    PersonId = personReq.PersonId,
                    Firstname = personReq.Firstname,
                    Middlename = string.IsNullOrWhiteSpace(personReq.Middlename) ? null : personReq.Middlename,
                    Lastname = personReq.Lastname,
                    Suffix = string.IsNullOrWhiteSpace(personReq.Suffix) ? null : personReq.Suffix,
                    BirthDate = DateTime.Parse(personReq.Birthdate),
                    BirthPlace = string.IsNullOrWhiteSpace(personReq.Birthplace) ? null : personReq.Birthplace,
                    Gender = string.IsNullOrWhiteSpace(personReq.Gender) ? null : personReq.Gender,
                    CivilStatus = string.IsNullOrWhiteSpace(personReq.CivilStatus) ? null : personReq.CivilStatus,
                    Religion = string.IsNullOrWhiteSpace(personReq.Religion) ? null : personReq.Religion,
                    ContactNo = string.IsNullOrWhiteSpace(personReq.ContactNo) ? null : personReq.ContactNo,
                    Email = string.IsNullOrWhiteSpace(personReq.Email) ? null : personReq.Email,
                    Citizenship = string.IsNullOrWhiteSpace(personReq.Citizenship) ? "FILIPINO" : personReq.Citizenship,
                    BloodType = string.IsNullOrWhiteSpace(personReq.BloodType) ? null : personReq.BloodType,
                    IsDeceased = false,
                    PatientIdTemp = 0,
                    AddressIdResidential = address.AddressId,
                    AddressIdPermanent = address.AddressId,
                  CreatedDate = DateTime.UtcNow
              };

                await _context.Person.AddAsync(person, cancellationToken);
                insertedPersons.Add(person);
            }
        }

        await _context.SaveChangesAsync(cancellationToken); // 🔥 Important: Save Persons FIRST

        // 4️⃣ Now we can Insert Household (PersonIdHeadOfHousehold is valid now)
        var firstPerson = insertedPersons.First(); // Use first inserted person as head
        var household = new Household
        {
            HouseholdId = request.HouseholdId,
            AddressId = address.AddressId,
            ResidenceName = "Temporary Name",
            HouseholdNo = GenerateHouseholdNo(),
            PersonIdHeadOfHousehold = firstPerson.PersonId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
        };
        await _context.Households.AddAsync(household, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);  

        // 5️⃣ Insert Families and FamilyMembers
        foreach (var familyReq in request.Families)
        {
            var familyPersonIds = familyReq.Persons.Select(p => p.PersonId).ToList();
            var headPersonId = familyPersonIds.First();

            var family = new Family
            {
                FamilyId = familyReq.FamilyId,
                HouseholdId = household.HouseholdId,
                PersonId = headPersonId,
                IsActive = true,
                FamilyNo = GenerateFamilyNo()
            };

            await _context.Families.AddAsync(family, cancellationToken);

            foreach (var personId in familyPersonIds)
            {
                var familyMember = new FamilyMember
                {
                    FamilyMemberId = Guid.NewGuid(),
                    FamilyId = family.FamilyId,
                    PersonId = personId
                };
                await _context.FamilyMembers.AddAsync(familyMember, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AppendFamilyToExistingHousehold(AppendFamilyRequest request, CancellationToken cancellationToken)
    {
        var household = await _context.Households
            .Include(h => h.Families)
            .FirstOrDefaultAsync(h => h.HouseholdId == request.HouseholdId, cancellationToken);

        if (household == null)
            throw new Exception("Household not found");

        var addressId = household.AddressId;

        var persons = request.Family.Persons.Select(p => new Person
        {
            PersonId = p.PersonId,
            Firstname = p.Firstname,
            Middlename = p.Middlename,
            Lastname = p.Lastname,
            Suffix = p.Suffix,
            BirthDate = DateTime.Parse(p.Birthdate),
            BirthPlace = p.Birthplace,
            Gender = p.Gender,
            CivilStatus = p.CivilStatus,
            Religion = p.Religion,
            ContactNo = p.ContactNo,
            Email = p.Email,
            Citizenship = string.IsNullOrWhiteSpace(p.Citizenship) ? "FILIPINO" : p.Citizenship,
            BloodType = p.BloodType,
            AddressIdResidential = addressId,
            AddressIdPermanent = addressId,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        foreach (var person in persons)
        {
            await _context.Person.AddAsync(person, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken); // must save first to get FK references

        var headPersonId = persons.First().PersonId;

        var family = new Family
        {
            FamilyId = request.Family.FamilyId,
            FamilyNo = request.Family.FamilyNo,
            HouseholdId = household.HouseholdId,
            PersonId = headPersonId,
            IsActive = true
        };

        await _context.Families.AddAsync(family, cancellationToken);

        foreach (var person in persons)
        {
            await _context.FamilyMembers.AddAsync(new FamilyMember
            {
                FamilyMemberId = Guid.NewGuid(),
                FamilyId = family.FamilyId,
                PersonId = person.PersonId
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AppendMemberToExistingFamily(AppendMemberRequest request, CancellationToken cancellationToken)
    {
        var household = await _context.Households
            .Include(h => h.Families)
            .FirstOrDefaultAsync(h => h.HouseholdId == request.HouseholdId, cancellationToken);

        if (household == null) throw new Exception("Household not found");

        var family = await _context.Families
            .FirstOrDefaultAsync(f => f.FamilyId == request.FamilyId && f.HouseholdId == request.HouseholdId, cancellationToken);

        if (family == null) throw new Exception("Family not found in household");

        var addressId = household.AddressId;

        var person = new Person
        {
            PersonId = request.Person.PersonId,
            Firstname = request.Person.Firstname,
            Middlename = request.Person.Middlename,
            Lastname = request.Person.Lastname,
            Suffix = request.Person.Suffix,
            BirthDate = DateTime.Parse(request.Person.Birthdate),
            BirthPlace = request.Person.Birthplace,
            Gender = request.Person.Gender,
            CivilStatus = request.Person.CivilStatus,
            Religion = request.Person.Religion,
            ContactNo = request.Person.ContactNo,
            Email = request.Person.Email,
            Citizenship = string.IsNullOrWhiteSpace(request.Person.Citizenship) ? "FILIPINO" : request.Person.Citizenship,
            BloodType = request.Person.BloodType,
            AddressIdResidential = addressId,
            AddressIdPermanent = addressId,
            CreatedDate = DateTime.UtcNow
        };

        await _context.Person.AddAsync(person, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var familyMember = new FamilyMember
        {
            FamilyMemberId = Guid.NewGuid(),
            FamilyId = request.FamilyId,
            PersonId = person.PersonId
        };

        await _context.FamilyMembers.AddAsync(familyMember, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }



    private void AddValidationError(Dictionary<string, List<string>> errors, string fieldName, string errorMessage)
    {
        if (!errors.ContainsKey(fieldName))
            errors[fieldName] = new List<string>();

        errors[fieldName].Add(errorMessage);
    }

    private string GenerateHouseholdNo()
    {
        return $"HH-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }

    private string GenerateFamilyNo()
    {
        return $"FAM-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}

