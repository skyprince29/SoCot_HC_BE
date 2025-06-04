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
    (5, 1, 4, 3, 0, 1),

     -- Status flow for PatientDepartmentTransaction
    -- From NULL to Pending
    (6, 2, NULL, 8, 1, 0),
    -- From Pending (8) to On-going (9)
    (7, 2, 8, 9, 0, 0),
    -- From Pending (8) to Cancelled (3)
    (8, 2, 8, 3, 0, 1),
    -- From On-going (9) to Deferred (11)
    (9, 2, 9, 11, 0, 0),
    -- From On-going (9) to Cancelled (3)
    (10, 2, 9, 3, 0, 1),
    -- From On-going (9) to Cleared (10)
    (11, 2, 9, 10, 0, 1),
    -- From Deferred (11) to Cancelled (3)
    (12, 2, 11, 3, 0, 1),
    -- From Deferred (11) to Cleared (10)
    (13, 2, 11, 10, 0, 1);
    -- insert other following Status flow below
SET IDENTITY_INSERT ModuleStatusFlow OFF;