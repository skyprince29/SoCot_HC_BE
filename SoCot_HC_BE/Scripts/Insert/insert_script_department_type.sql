-- Insert Department Types with Description, CreatedBy, and CreatedDate
INSERT INTO DepartmentType (DepartmentTypeId, DepartmentTypeName, Description, IsActive, CreatedBy, CreatedDate)
VALUES 
    (NEWID(), 'OPD', 'Outpatient department offering consultation and follow-up services', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- OPD
    (NEWID(), 'ER', 'Emergency Room providing immediate care for critical patients', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- ER
    (NEWID(), 'Dispensing', 'Departments that dispense medication or supplies', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Dispensing
    (NEWID(), 'Inventory', 'Department responsible for managing inventory and warehouse operations', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Inventory
    (NEWID(), 'Diagnostic', 'Departments for diagnostic services such as laboratory or imaging', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Diagnostic
    (NEWID(), 'Service', 'Departments that provide clinical or treatment services', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),   -- Service
    (NEWID(), 'Admin', 'Departments related to administration and non-clinical operations', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Admin
    (NEWID(), 'Ward', 'Inpatient departments with assigned rooms for patient care', 1, '00000000-0000-0000-0000-000000000001', GETDATE());  -- Ward
