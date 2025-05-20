
DELETE FROM DepartmentType;
-- Insert Department Types with Description, CreatedBy, and CreatedDate
INSERT INTO DepartmentType (DepartmentTypeId, DepartmentTypeName, Description, IsActive, CreatedBy, CreatedDate)
VALUES 
    ('46d15d71-2434-45dd-8f6c-899246953f22', 'OPD', 'Outpatient department offering consultation and follow-up services', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- OPD
    ('abe4ffdc-ac0c-4efd-8744-2c0d85b83f4f', 'ER', 'Emergency Room providing immediate care for critical patients', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- ER
    ('7a477d9c-ae1c-494f-8517-c4d14935f1fe', 'Dispensing', 'Departments that dispense medication or supplies', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Dispensing
    ('50149ced-e86d-416d-bbb6-1e321cc69517', 'Inventory', 'Department responsible for managing inventory and warehouse operations', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Inventory
    ('ffa68048-e47b-4ea1-8f9e-cc3c6cf3175f', 'Diagnostic', 'Departments for diagnostic services such as laboratory or imaging', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Diagnostic
    ('839c0fc0-0d19-4d21-95ec-ccd2674f0d36', 'Service', 'Departments that provide clinical or treatment services', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),   -- Service
    ('af0e8938-e829-43e7-9df8-f09d2a91d963', 'Admin', 'Departments related to administration and non-clinical operations', 1, '00000000-0000-0000-0000-000000000001', GETDATE()),  -- Admin
    ('f73aa807-1920-4f7b-9b07-bc0d4a8ace28', 'Ward', 'Inpatient departments with assigned rooms for patient care', 1, '00000000-0000-0000-0000-000000000001', GETDATE());  -- Ward