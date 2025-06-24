-- use SCHC;

-- Declare and set ProvinceId
DECLARE @ProvinceId INT;
SET @ProvinceId = (
    SELECT TOP 1 ProvinceId
    FROM Province
    WHERE ProvinceName = 'South Cotabato'
);

-- Declare and set MunicipalityId
DECLARE @MunicipalityId INT;
SET @MunicipalityId = (
    SELECT TOP 1 MunicipalityId
    FROM Municipality
    WHERE MunicipalityName = 'City of Koronadal'
      AND ProvinceId = @ProvinceId
);

-- Declare and set BarangayId
DECLARE @BarangayId INT;
SET @BarangayId = (
    SELECT TOP 1 BarangayId
    FROM Barangay
    WHERE BarangayName = 'Zone I (Pob.)'
      AND MunicipalityId = @MunicipalityId
);

INSERT INTO Address (AddressId, BarangayId, MunicipalityId, ProvinceId, Sitio, Purok, ZipCode, HouseNo, LotNo, BlockNo, Street, Subdivision)
	VALUES (NEWID(), @BarangayId, @MunicipalityId, @ProvinceId, 'Sitio Uno', 'Purok Dos', '9506', '12A', 'Lot 7', 'Block 3', 'Rizal Street', 'Happy Homes');


DECLARE @AddressId UNIQUEIDENTIFIER;

-- Get the most recently saved AddressId
SET @AddressId = (
    SELECT TOP 1 AddressId
    FROM Address
    ORDER BY AddressId DESC  -- Use a timestamp column if available
);


SET @FacilityId = 2289;
SET @PersonId = '{CF246EDF-5900-4356-8205-48554232DB0A}';
SET @CreatedBy ='{CECB264D-B24B-42F8-AC73-D4A6F983FA91}';

INSERT INTO dbo.Person (PersonId, Firstname, Middlename, Lastname, Suffix, BirthDate, BirthPlace, Gender, CivilStatus, Religion, ContactNo, Email, AddressIdResidential, AddressIdPermanent, IsDeceased, Citizenship, BloodType, PatientIdTemp, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) 
VALUES (@PersonId, 'Admin', 'S', 'Admin', '', '1980-01-01', 'Koronadal City', 'Male', 'Single', 'Roman Catholic', '09171234567', 'superadmin@email.com', @AddressId, @AddressId, 0, 'Filipino', 'O+', 0,@CreatedBy, GETDATE(), @CreatedBy, GETDATE());


DECLARE @DesignationId UNIQUEIDENTIFIER;
SET @DesignationId = (
    SELECT TOP 1 DesignationId
    FROM Designation
    Where DesignationName = 'SuperAdmin'
);

INSERT INTO dbo.UserAccount (UserAccountId, Username, Password, PersonId, FacilityId, UserGroupId, RememberMeToken, IsOnline, IsinitLogin, IsActive, UserIdTemp, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, DesignationId) 
VALUES (@CreatedBy, 'SuperAdmin', '2d711642b726b04401627ca9fbac32f5c8530fb1903cc4db02258717921a4881', @PersonId, @FacilityId, 1, NULL, 1, 1, 1, NULL, @CreatedBy, GETDATE(),@CreatedBy, GETDATE(), @DesignationId);

--delete from Facility;
--delete from Families
--delete from Households;
--delete from FamilyMembers
--delete from Person;
--delete from Address;
--delete from UserAccount;
