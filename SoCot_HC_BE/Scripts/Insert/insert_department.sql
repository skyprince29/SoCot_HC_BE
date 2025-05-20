-- === DECLARE COMMON VARIABLES ===
DECLARE @FacilityId INT = 2289;-- ===SCPH
DECLARE @CreatedBy UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';
DECLARE @Now DATETIME = GETDATE();

-- === DELETE EXISTING Facility Departments ===
DELETE FROM Department WHERE FacilityId = @FacilityId;

-- === INITIALIZE CODE COUNTER ===
DECLARE @DepartmentCodeCounter INT = 1;

-- === FUNCTION TO GENERATE NEXT CODE ===
DECLARE @DepartmentCode NVARCHAR(10);
DECLARE @ParentId UNIQUEIDENTIFIER;
DECLARE @ChildId UNIQUEIDENTIFIER;

-- === FUNCTION: Generate Next Code (formatted '001', '002', etc.) ===
DECLARE @NextCode NVARCHAR(10);
CREATE TABLE #DepartmentTemp (DeptName NVARCHAR(255), ParentDeptName NVARCHAR(255) NULL);

-- === Insert Departments and Parent-Child Structure Here ===
INSERT INTO #DepartmentTemp (DeptName, ParentDeptName)
VALUES 
-- Emergency Room Structure
('Emergency Room Department', NULL),
('Triage', 'Emergency Room Department'),
('ER Obstetrics', 'Emergency Room Department'),
('ER Surgery', 'Emergency Room Department'),
('ER Pediatrics', 'Emergency Room Department'),
('ER Medicine', 'Emergency Room Department'),

-- OPD Structure
('OPD', NULL),
('Vital Sign', 'OPD'),
('Animal Bite', 'OPD'),
('Dental', 'OPD'),

-- Others
('Supply', NULL),
('Central Supply', NULL),
('Pharmacy', NULL),
('Family Planning', NULL),

-- Wards
('Female Medical Ward', NULL),
('Male Medical Ward', NULL),
('Pedia Ward', NULL),
('Surgery Ward', NULL),
('OB Ward', NULL),
('OB Surgery Ward', NULL),
('Isolation Ward', NULL),
('Labor Room/Delivery Room', NULL);

-- === TEMP TABLE to Map Inserted Departments ===
CREATE TABLE #InsertedDepartments (
    DepartmentId UNIQUEIDENTIFIER,
    DeptName NVARCHAR(255)
);

-- === INSERT LOOP ===
DECLARE InsertCursor CURSOR FOR
SELECT DeptName, ParentDeptName
FROM #DepartmentTemp;

OPEN InsertCursor;

DECLARE @DeptName NVARCHAR(255);
DECLARE @ParentDeptName NVARCHAR(255);

FETCH NEXT FROM InsertCursor INTO @DeptName, @ParentDeptName;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Generate Department Code (example: 001, 002, etc.)
    SET @DepartmentCode = RIGHT('000' + CAST(@DepartmentCodeCounter AS NVARCHAR), 3);

    -- Find ParentId if applicable
    IF @ParentDeptName IS NOT NULL
    BEGIN
        SELECT @ParentId = DepartmentId
        FROM #InsertedDepartments
        WHERE DeptName = @ParentDeptName;
    END
    ELSE
    BEGIN
        SET @ParentId = NULL;
    END

    -- Insert Department
    SET @ChildId = NEWID();
    INSERT INTO Department (DepartmentId, FacilityId, DepartmentName, DepartmentCode, IsActive, IsReferable, ParentDepartmentId, CreatedBy, CreatedDate)
    VALUES (@ChildId, @FacilityId, @DeptName, @DepartmentCode, 1, 1, @ParentId, @CreatedBy, @Now);

    -- Save Inserted Department to Temp for Mapping
    INSERT INTO #InsertedDepartments (DepartmentId, DeptName)
    VALUES (@ChildId, @DeptName);

    -- Increment Code Counter
    SET @DepartmentCodeCounter = @DepartmentCodeCounter + 1;

    FETCH NEXT FROM InsertCursor INTO @DeptName, @ParentDeptName;
END

CLOSE InsertCursor;
DEALLOCATE InsertCursor;

-- === CLEANUP TEMP TABLES ===
DROP TABLE #DepartmentTemp;
DROP TABLE #InsertedDepartments;

