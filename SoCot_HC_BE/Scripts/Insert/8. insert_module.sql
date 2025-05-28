DELETE FROM Module;

SET IDENTITY_INSERT Module ON;

INSERT INTO Module (Id, Name, IsActive)
VALUES
    (1, 'PatientRegistry', 1),
    (2, 'OPDVitalSign', 1),
    (3, 'OPDConsultation', 1),
    (4, 'OPDVaccination', 1);

SET IDENTITY_INSERT Module OFF;