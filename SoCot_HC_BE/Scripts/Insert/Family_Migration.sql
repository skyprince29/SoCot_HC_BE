DECLARE @sourceDb NVARCHAR(128) = 'db_uhc';
DECLARE @destDb NVARCHAR(128) = 'SCHC';
DECLARE @sql NVARCHAR(MAX);

SET @sql = '
WITH FamilyHousehold AS (
    SELECT  
        fam.Id AS famId,
        fam.FamilySerialNo,
        fam.HouseHoldId,
        hhSrc.PHouseholdNo
    FROM [' + @sourceDb + '].dbo.Families fam
    INNER JOIN [' + @sourceDb + '].dbo.HouseHolds hhSrc
        ON hhSrc.Id = fam.HouseHoldId
    GROUP BY fam.Id, fam.FamilySerialNo, fam.HouseHoldId, hhSrc.PHouseholdNo
)

INSERT INTO [dbo].[Families]
    ([FamilyId], [HouseholdId], [IsActive], [FamilyNo], [CreatedBy], [CreatedDate])
SELECT 
    NEWID(),
    hh.HouseholdId,
    1,
    fh.FamilySerialNo,
    ''22222222-2222-2222-2222-222222222222'',
    GETDATE()
FROM FamilyHousehold fh
INNER JOIN [' + @destDb + '].dbo.Households hh
    ON hh.HouseholdNo = fh.PHouseholdNo
	WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[Families] f
    WHERE f.FamilyNo = fh.FamilySerialNo
)
ORDER BY fh.FamilySerialNo ASC;
';

EXEC sp_executesql @sql;
