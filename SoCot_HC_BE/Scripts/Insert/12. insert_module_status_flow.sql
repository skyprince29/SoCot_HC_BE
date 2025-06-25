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


    -- Status flow for PatientDepartmentTransaction
    -- From NULL to Pending
    (5, 2, NULL, 8, 1, 0),
    -- From Pending (8) to On-going (9)
    (6, 2, 8, 3, 0, 0),
    -- From On-going (9) to Deferred (11)
    (7, 2, 9, 11, 0, 0),
    -- From On-going (9) to Redirected (5)
    (8, 2, 9, 5, 0, 1),
    -- From On-going (9) to Cleared (10)
    (9, 2, 9, 10, 0, 1),
    -- From Deferred (11) to On-going (9)
    (10, 2, 11, 9, 0, 0),

     -- Status flow for Referral
     -- From NULL to Created
     (11, 3, NULL, 1, 1, 0),
     -- From Created (1) to Seen (12)
     (12, 3, 1, 12, 0, 0),
     -- From Seen (12) to Accepted (13)
     (13, 3, 12, 13, 0, 0),
     -- From Accepted (13) to Arrived (4)
     (14, 3, 13, 4, 0, 0),
     -- From Accepted (13) to Redirected (5)
     (15, 3, 13, 5, 0, 1),
     -- From Arrived (4) to Redirected (5)
     (16, 3, 4, 5, 0, 1),
      -- From Arrived (4) to Catered (14)
     (17, 3, 4, 14, 0, 1),
      -- From Arrived (4) to DOA (17)
     (18, 3, 4, 17, 0, 1),
      -- From Arrived (4) to No Show (21)
     (19, 3, 4, 21, 0, 1),
      -- From Arrived (4) to Admitted (15)
     (20, 3, 4, 15, 0, 0),
      -- From Admitted (15) to Discharged (7)
     (21, 3, 15, 7, 0, 1),
      -- From Admitted (15) to Expired (6)
     (22, 3, 15, 6, 0, 1),
      -- From Admitted (15) to HAMA (19)
     (23, 3, 15, 19, 0, 1),
      -- From Admitted (15) to DAMA (20)
     (24, 3, 15, 20, 0, 1),
      -- From Arrived (4) to ER Observation (16)
     (25, 3, 4, 16, 0, 0),
      -- From ER Observation (16) to Admitted (15)
     (26, 3, 16, 15, 0, 0),
      -- From ER Observation (16) to Redirected (5)
     (27, 3, 16, 5, 0, 1),
      -- From ER Observation (16) to Discharged (7)
     (28, 3, 16, 7, 0, 1),
      -- From ER Observation (16) to ER Death (22)
     (29, 3, 16, 22, 0, 1),
      -- From ER Observation (16) to Refuse Admission (18)
     (30, 3, 16, 18, 0, 0),
     -- From Refuse Admission (18) to HAMA (19)
     (31, 3, 16, 19, 0, 1),
      -- From Refuse Admission (18) to DAMA (20)
     (32, 3, 18, 20, 0, 1);
    -- insert other following Status flow below
SET IDENTITY_INSERT ModuleStatusFlow OFF;