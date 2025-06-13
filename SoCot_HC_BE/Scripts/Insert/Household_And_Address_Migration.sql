DECLARE @sourceDb NVARCHAR(100) = 'db_uhc';
DECLARE @destDb NVARCHAR(100) = 'SCHC';
DECLARE @sql NVARCHAR(MAX);

SET @sql = '
-- STEP 1: Insert into Address from source HouseHolds/Patients
WITH RankedHouseholds AS (
    SELECT 
        hh.PHouseholdNo,
        hh.barangay_id,
        hh.city_municipality_id,
        hh.province_id,
        hh.Sitio,
        hh.Purok,
        hh.Zipcode,
        hh.HouseNo,
        hh.LotNo,
        hh.BlockNo,
        hh.Street,
        hh.Subdivision,
        ROW_NUMBER() OVER (PARTITION BY hh.PHouseholdNo ORDER BY pt.Id) AS rn
    FROM [' + @sourceDb + '].dbo.HouseHolds hh
    INNER JOIN [' + @sourceDb + '].dbo.Patients pt
        ON pt.PHouseholdNo = hh.PHouseholdNo
)
INSERT INTO [' + @destDb + '].[dbo].[Address]
       ([AddressId],[BarangayId], [MunicipalityId], [ProvinceId], [Sitio], [Purok],
        [ZipCode], [HouseNo], [LotNo], [BlockNo], [Street], [Subdivision], [TempId])
SELECT 
    NEWID(),
    barangay_id,
    city_municipality_id,
    province_id,
    CASE 
        WHEN LEN(LTRIM(RTRIM(Sitio))) > 50 THEN NULL 
        ELSE LTRIM(RTRIM(Sitio)) 
    END AS Sitio,
    CASE 
        WHEN LEN(LTRIM(RTRIM(Purok))) > 50 THEN NULL 
        ELSE LTRIM(RTRIM(Purok)) 
    END AS Purok,
    LTRIM(RTRIM(Zipcode)),
    LTRIM(RTRIM(HouseNo)),
    LTRIM(RTRIM(LotNo)),
    LTRIM(RTRIM(BlockNo)),
    CASE 
        WHEN LEN(LTRIM(RTRIM(Street))) > 50 THEN NULL 
        ELSE LTRIM(RTRIM(Street)) 
    END AS Street,
    CASE 
        WHEN LEN(LTRIM(RTRIM(Subdivision))) > 50 THEN NULL 
        ELSE LTRIM(RTRIM(Subdivision)) 
    END AS Subdivision,
    PHouseholdNo
FROM RankedHouseholds rh
WHERE rh.rn = 1
AND NOT EXISTS (
    SELECT 1 FROM [' + @destDb + '].[dbo].[Address] a
    WHERE a.TempId = rh.PHouseholdNo
);

-- STEP 2: Insert into Households using the Address table
INSERT INTO [' + @destDb + '].[dbo].[Households]
           ([HouseholdId]
           ,[HouseholdNo]
           ,[ResidenceName]         
           ,[AddressId]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedDate])
SELECT 
    NEWID() as HouseholdId,
    hh.PHouseholdNo,
	LTRIM(RTRIM(ISNULL(hh.ResidenceName, ''''))) as ResidenceName,
    adrs.AddressId, 
	1 as IsActive, -- default active, to confirm if this property is to be remove
	''22222222-2222-2222-2222-222222222222'' as CreatedBy, -- update with actual default user account data 
    GETDATE() as CreatedDate
FROM [' + @sourceDb + '].dbo.HouseHolds hh
INNER JOIN [' + @destDb + '].dbo.[Address] adrs
    ON adrs.TempId = hh.PHouseholdNo
WHERE NOT EXISTS (
    SELECT 1 FROM [' + @destDb + '].dbo.Households h
    WHERE h.HouseholdNo = hh.PHouseholdNo
);
';

EXEC sp_executesql @sql;
