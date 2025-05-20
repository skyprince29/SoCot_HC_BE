DELETE FROM Status;

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
    (10, 'Cleared', 1);