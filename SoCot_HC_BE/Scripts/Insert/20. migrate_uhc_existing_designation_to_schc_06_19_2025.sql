-- delete from Designation where tempId is not null;
Use SCHC;
-- Insert Designation for super admin
Insert Into Designation (DesignationId, DesignationCode, DesignationName, IsActive,CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
Values ( NEWID(), '000-250508-111111', 'SuperAdmin', 1,'00000001-0001-0001-0001-000000000001', GETDATE(),'00000001-0001-0001-0001-000000000001',GETDATE());


-- Declare a timestamp string in the format yyMMdd-HHmmss
DECLARE @timestamp VARCHAR(20) = FORMAT(GETDATE(), 'yyMMdd-HHmmss');
DECLARE @userId UNIQUEIDENTIFIER =  (SELECT TOP 1 DesignationId FROM Designation Where DesignationName = 'SuperAdmin');
DECLARE @now DATETIME = GETDATE();

-- Temporary table holding the Model B data
DECLARE @ModelB TABLE (
    RowNum INT IDENTITY(1,1),
    DesignationName VARCHAR(300)
);

-- Insert Model B data
INSERT INTO @ModelB (DesignationName)
VALUES 
('OPD_VS'),
('OPD_RECEIVING'),
('OPD_CONSULTATION'),
('OPD_VP'),
('AB QUEUING'),
('ENCODER'),
('REFERRAL MANAGER'),
('REFERRAL ADMIN'),
('DATA MANAGER'),
('VIEWER'),
('ADMINISTRATOR'),
('EKONSULTA ENCODER'),
('Pharmacy'),
('CSR');

-- Insert into Designations (Model A)
INSERT INTO Designation (
    DesignationId,
    DesignationCode,
    DesignationName,
    IsActive,
    tempId,
    CreatedBy,
    UpdatedBy,
    CreatedDate,
    UpdatedDate
)
SELECT 
    NEWID(), -- Generate a new Guid
    RIGHT('000' + CAST(RowNum AS VARCHAR), 3) + '-' + @timestamp AS DesignationCode,
    DesignationName,
    1 AS IsActive,
    RowNum AS tempId,
    @userId AS CreatedBy,
    @userId AS UpdatedBy,
    @now AS CreatedDate,
    @now AS UpdatedDate
FROM @ModelB;
