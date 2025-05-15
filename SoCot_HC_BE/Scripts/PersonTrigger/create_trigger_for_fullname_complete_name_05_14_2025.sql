-- Drop the trigger if it exists
DROP TRIGGER IF EXISTS trg_UpdateFullNames;


CREATE TRIGGER trg_UpdateFullNames
ON Person
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Person
    SET 
        Fullname = 
            p.Lastname + ', ' + p.Firstname + ' ' +
            (CASE 
                WHEN p.Middlename IS NOT NULL AND LEN(LTRIM(RTRIM(p.Middlename))) > 0 
                THEN LEFT(LTRIM(RTRIM(p.Middlename)), 1) + '.' 
                ELSE '' 
             END),
        
        CompleteName = 
            p.Firstname + ' ' +
            (CASE 
                WHEN p.Middlename IS NOT NULL AND LEN(LTRIM(RTRIM(p.Middlename))) > 0 
                THEN LEFT(LTRIM(RTRIM(p.Middlename)), 1) + '. ' 
                ELSE '' 
             END) +
            p.Lastname
    FROM Person p
    INNER JOIN inserted i ON p.PersonId = i.PersonId; -- assumes primary key is 'Id'
END;
