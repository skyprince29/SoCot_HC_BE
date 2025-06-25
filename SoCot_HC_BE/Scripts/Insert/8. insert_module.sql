DELETE FROM Module;

SET IDENTITY_INSERT Module ON;

INSERT INTO Module (Id, Name, IsActive)
VALUES
    (1, 'PatientRegistry', 1),
    (2, 'PatientDepartmentTransaction', 1),
    (3, 'Referral', 1);

SET IDENTITY_INSERT Module OFF;