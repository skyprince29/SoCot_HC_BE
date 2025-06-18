DECLARE @sourceDb NVARCHAR(128) = 'db_uhc';
DECLARE @destDb NVARCHAR(128) = 'SCHC';
DECLARE @sql NVARCHAR(MAX);

SET @sql = '
INSERT INTO [' + @destDb + '].dbo.FamilyMembers
    ([FamilyMemberId], [FamilyId], [PersonId], [CreatedBy], [CreatedDate])

SELECT
    NEWID(),
    fam.FamilyId,
    prsn.PersonId,
    ''22222222-2222-2222-2222-222222222222'',
    GETDATE()
FROM [' + @destDb + '].dbo.Families fam
INNER JOIN [' + @destDb + '].dbo.Households hh
    ON hh.HouseholdId = fam.HouseholdId
INNER JOIN [' + @sourceDb + '].dbo.Patients pt
    ON pt.PHouseholdNo = hh.HouseholdNo
INNER JOIN [' + @destDb + '].dbo.Person prsn
    ON prsn.PatientIdTemp = pt.Id
WHERE NOT EXISTS (
    SELECT 1
    FROM [' + @destDb + '].dbo.FamilyMembers fm
    WHERE fm.FamilyId = fam.FamilyId AND fm.PersonId = prsn.PersonId
)
GROUP BY fam.FamilyId, prsn.PersonId;
';

EXEC sp_executesql @sql;
