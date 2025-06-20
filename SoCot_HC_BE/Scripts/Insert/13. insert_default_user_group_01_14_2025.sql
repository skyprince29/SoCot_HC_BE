
DBCC CHECKIDENT ('UserGroup', RESEED, 0);
INSERT INTO UserGroup (UserType, UserGroupName, IsActive)

Values (1, 'Super Admin', 1),
(2, 'Admin', 1),
(3, 'Data Manager', 1),
(4, 'Encoder', 1),
(5, 'Not Set', 1)