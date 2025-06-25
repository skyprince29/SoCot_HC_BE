-- Step 1: Drop both FK constraints referencing Status
--Enable this if you already have transaction
--ALTER TABLE TransactionFlowHistory DROP CONSTRAINT FK_TransactionFlowHistory_Status_PreviousStatusId;
--ALTER TABLE TransactionFlowHistory DROP CONSTRAINT FK_TransactionFlowHistory_Status_CurrentStatusId;


-- Step 2: Delete all rows in Status
DELETE FROM Status;

-- Step 3: Insert new rows (with fixed unique Ids)
INSERT INTO Status (Id, Name, IsActive)
VALUES
    (1, 'Created', 1),
    (2, 'Approved', 1),
    (3, 'Cancelled', 1),
    (4, 'Arrived', 1),
    (5, 'Redirected', 1),
    (6, 'Expired', 1),
    (7, 'Discharged', 1),
    (8, 'Pending', 1),
    (9, 'On-going', 1),
    (10, 'Cleared', 1),
    (11, 'Deferred', 1),
    (12, 'Seen', 1),
    (13, 'Accepted', 1),
    (14, 'Catered', 1),
    (15, 'Admitted', 1),
    (16, 'ER Observation', 1),
    (17, 'Dead on Arrival', 1),
    (18, 'Refuse Admission', 1),
    (19, 'HAMA', 1),
    (20, 'DAMA', 1),
	(21, 'No Show', 1),
	(22, 'ER Death', 1);

 --Enable this if you already have transaction
-- Step 4: Recreate both FK constraints
--ALTER TABLE TransactionFlowHistory
--ADD CONSTRAINT FK_TransactionFlowHistory_Status_PreviousStatusId
--FOREIGN KEY (PreviousStatusId)
--REFERENCES Status(Id);
--ALTER TABLE TransactionFlowHistory
--ADD CONSTRAINT FK_TransactionFlowHistory_Status_CurrentStatusId
--FOREIGN KEY (CurrentStatusId)
--REFERENCES Status(Id);
