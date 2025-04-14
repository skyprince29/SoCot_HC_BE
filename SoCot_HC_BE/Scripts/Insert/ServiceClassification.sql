-- Enable identity insert
SET IDENTITY_INSERT ServiceClassification ON;

-- Insert Service Classifications
INSERT INTO ServiceClassification (ServiceClassificationId, Name, IsActive)
VALUES 
    (1, 'ER', 1),  -- Emergency Room
    (2, 'OPD', 1); -- Outpatient Department

-- Disable identity insert
SET IDENTITY_INSERT ServiceClassification OFF; 