use SCHC;

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

DECLARE @FacilityId INT;

-- Check if facility already exists
SELECT @FacilityId = FacilityId
FROM Facility
WHERE FacilityName = 'South Cotabato Provincial Hospital';

-- Insert Facility
IF @FacilityId IS NULL
BEGIN
	INSERT INTO dbo.Facility (FacilityCode, AddressId, AccreditationNo, FacilityName, EmailAddress, TINNumber, ContactNumber, Sector, FacilityLevel, IsActive, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) 
	VALUES ('000-000-001', @AddressId, 'ACCR-123456', 'South Cotabato Provincial Hospital', 'scph@email.com', '123456789012345', '0832281234', 1, 2, 1, '22222222-2222-2222-2222-222222222222', GETDATE(), '22222222-2222-2222-2222-222222222222', GETDATE());
	SET @FacilityId = SCOPE_IDENTITY();
END

INSERT INTO dbo.Person (PersonId, Firstname, Middlename, Lastname, Suffix, BirthDate, BirthPlace, Gender, CivilStatus, Religion, ContactNo, Email, AddressIdResidential, AddressIdPermanent, IsDeceased, Citizenship, BloodType, PatientIdTemp, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) 
VALUES (NEWID(), 'Admin', 'S', 'Admin', '', '1980-01-01', 'Koronadal City', 'Male', 'Single', 'Roman Catholic', '09171234567', 'superadmin@email.com', @AddressId, @AddressId, 0, 'Filipino', 'O+', 0, '22222222-2222-2222-2222-222222222222', GETDATE(), '22222222-2222-2222-2222-222222222222', GETDATE());

DECLARE @PersonId UNIQUEIDENTIFIER;
SET @PersonId = (
    SELECT TOP 1 PersonId
    FROM Person
    ORDER BY PersonId DESC
);

DECLARE @DesignationId UNIQUEIDENTIFIER;
SET @DesignationId = (
    SELECT TOP 1 DesignationId
    FROM Designation
    Where DesignationName = 'SuperAdmin'
);

INSERT INTO dbo.UserAccount (UserAccountId, Username, Password, PersonId, FacilityId, UserGroupId, RememberMeToken, IsOnline, IsinitLogin, IsActive, UserIdTemp, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, DesignationId) 
VALUES (NEWID(), 'SuperAdmin', '2d711642b726b04401627ca9fbac32f5c8530fb1903cc4db02258717921a4881', @PersonId, @FacilityId, 1, NULL, 1, 1, 1, NULL, '22222222-2222-2222-2222-222222222222', GETDATE(), '22222222-2222-2222-2222-222222222222', GETDATE(), @DesignationId);

--delete from Facility;
--delete from Families
--delete from Households;
--delete from FamilyMembers
--delete from Person;
--delete from Address;
--delete from UserAccount;
