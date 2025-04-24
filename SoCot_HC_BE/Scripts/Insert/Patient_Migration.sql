DECLARE @SourceDB NVARCHAR(100) = 'db_uhc'; -- Database Name for Source
DECLARE @DestDB NVARCHAR(100) = 'SCHC'; -- Database Name for Destination
DECLARE @SQL NVARCHAR(MAX);

SET @SQL = '
-- Step 1: Insert into Person
INSERT INTO ' + QUOTENAME(@DestDB) + '.dbo.Person
(
    PersonId,
    Firstname,
    Middlename,
    Lastname,
    Suffix,
    BirthDate,
    BirthPlace,
    Gender,
    CivilStatus,
    Religion,
    ContactNo,
    Email,
    IsDeceased,
    Citizenship,
    BloodType,
    PatientIdTemp
)
SELECT 
    NEWID() as PKId,
    pt.Firstname,
    REPLACE(
        REPLACE(
            REPLACE(
                REPLACE(LTRIM(RTRIM(pt.Middlename)), CHAR(13), ''''), 
            CHAR(10), ''''), 
        CHAR(9), ''''), 
    ''  '', '' '') as Middlename,
    LEFT(REPLACE(
        REPLACE(
            REPLACE(
                REPLACE(LTRIM(RTRIM(pt.Lastname)), CHAR(13), ''''), 
            CHAR(10), ''''), 
        CHAR(9), ''''), 
    ''  '', '' ''), 30) as Lastname,
    pt.Suffix,
    pt.Birthdate,
    pt.BirthPlace,
    pt.Sex as Gender,
    CivilStatus,
    rl.ReligionName,
    CASE 
        WHEN LEN(pt.ContactNumber) > 15 THEN NULL 
        ELSE pt.ContactNumber
    END AS ContactNo,
    pt.EmailAddress,
    0 as IsDeceased,
    pt.Citizenship,
    pt.BloodType,
    pt.Id
FROM ' + QUOTENAME(@SourceDB) + '.dbo.Patients pt
LEFT JOIN ' + QUOTENAME(@SourceDB) + '.dbo.Religions rl
    ON rl.Id = pt.ReligionId
WHERE NOT EXISTS (
    SELECT 1 
    FROM ' + QUOTENAME(@DestDB) + '.dbo.Person p 
    WHERE p.PatientIdTemp = pt.Id
)
ORDER BY pt.Id ASC;

-- Step 2: Insert into Patient
INSERT INTO ' + QUOTENAME(@DestDB) + '.dbo.Patient
(
    PatientId,
    PHICMemberType,
    PhilHealthNo,
    PersonIdPatient,
    IsActive,
    PatientIdTemp
)
SELECT
    NEWID() as PKId,
    ''MM'' as PHICMemberType,
    pt.PhilHealthNo,
    prsn.PersonId,
    pt.IsActive,
    pt.Id
FROM ' + QUOTENAME(@SourceDB) + '.dbo.Patients pt
INNER JOIN ' + QUOTENAME(@DestDB) + '.dbo.Person prsn
    ON prsn.PatientIdTemp = pt.Id
WHERE NOT EXISTS (
    SELECT 1
    FROM ' + QUOTENAME(@DestDB) + '.dbo.Patient p
    WHERE p.PatientIdTemp = pt.Id
)
ORDER BY pt.Id ASC;
';

-- Execute the combined dynamic SQL
EXEC sp_executesql @SQL;
