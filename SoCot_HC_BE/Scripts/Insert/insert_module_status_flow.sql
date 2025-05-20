DELETE FROM ModuleStatusFlow;
SET IDENTITY_INSERT ModuleStatusFlow ON;
INSERT INTO ModuleStatusFlow (Id, ModuleId, RequiredStatusId, NextStatusId, IsStart, IsComplete)
VALUES
    -- Status flow for PatientRegistry
    -- Starting status: Arrived (no required status, i.e., NULL)
    (1, 1, NULL, 4, 1, 0),  -- This marks Arrived as starting status
    -- From Arrived (4) to Redirected (5)
    (2, 1, 4, 5, 0, 1),
    -- From Arrived (4) to Expired (6)
    (3, 1, 4, 6, 0, 1),
    -- From Arrived (4) to Discharged (7)
    (4, 1, 4, 7, 0, 1),
    -- From Arrived (4) to Cancelled (3)
    (5, 1, 4, 3, 0, 1);
    -- insert other following Status flow below
SET IDENTITY_INSERT ModuleStatusFlow OFF;