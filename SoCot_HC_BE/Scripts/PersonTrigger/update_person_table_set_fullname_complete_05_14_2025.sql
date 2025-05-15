UPDATE Person
SET 
    Fullname = 
        Lastname + ', ' + Firstname + ' ' +
        (CASE 
            WHEN Middlename IS NOT NULL AND LEN(LTRIM(RTRIM(Middlename))) > 0 
            THEN LEFT(LTRIM(RTRIM(Middlename)), 1) + '.' 
            ELSE '' 
         END),
    
    CompleteName = 
        Firstname + ' ' +
        (CASE 
            WHEN Middlename IS NOT NULL AND LEN(LTRIM(RTRIM(Middlename))) > 0 
            THEN LEFT(LTRIM(RTRIM(Middlename)), 1) + '. ' 
            ELSE '' 
         END) +
        Lastname;