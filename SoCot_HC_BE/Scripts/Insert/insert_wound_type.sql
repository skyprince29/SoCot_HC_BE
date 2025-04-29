-- Enable identity insert
SET IDENTITY_INSERT WoundType ON;

-- Insert Wound Types
INSERT INTO WoundType (WoundTypeId, WoundTypeName, Description, IsActive, CreatedBy, CreatedDate)
VALUES 
    (1, 'Abrasion', NULL, 1, '00000000-0000-0000-0000-000000000001', GETDATE()),
    (2, 'Lacerations', NULL, 1, '00000000-0000-0000-0000-000000000001', GETDATE()),
    (3, 'Puncture', NULL, 1, '00000000-0000-0000-0000-000000000001', GETDATE()),
    (4, 'Avulsion', NULL, 1, '00000000-0000-0000-0000-000000000001', GETDATE());

-- Disable identity insert
SET IDENTITY_INSERT WoundType OFF;
