-- Delete all rows from the table
DELETE FROM Barangay;

-- Reset the IDENTITY column to start from 1
DBCC CHECKIDENT ('Barangay', RESEED, 0);

-- Delete all rows from the table
DELETE FROM Municipality;

-- Reset the IDENTITY column to start from 1
DBCC CHECKIDENT ('Municipality', RESEED, 0);

-- Delete all rows from the table
DELETE FROM Province;

-- Reset the IDENTITY column to start from 1
DBCC CHECKIDENT ('Province', RESEED, 0);


USE [SCHC]
GO
SET IDENTITY_INSERT [dbo].[Province] ON 

INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (1, N'Abra                                              ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (2, N'Agusan del Norte                                  ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (3, N'Agusan del Sur                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (4, N'Aklan                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (5, N'Albay                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (6, N'Antique                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (7, N'Apayao                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (8, N'Aurora                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (9, N'Bacolod City                                      ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (10, N'Basilan                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (11, N'Bataan                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (12, N'Batanes                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (13, N'Batangas                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (14, N'Benguet                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (15, N'Biliran                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (16, N'Bohol                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (17, N'Bukidnon                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (18, N'Bulacan                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (19, N'Cagayan                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (20, N'Camarines Norte                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (21, N'Camarines Sur                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (22, N'Camiguin                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (23, N'Capiz                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (24, N'Catanduanes                                       ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (25, N'Cavite                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (26, N'Cebu                                              ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (27, N'City of Isabela                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (28, N'City of Manila                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (29, N'Cotabato (North Cotabato)                         ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (30, N'Cotabato City                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (31, N'Davao City                                        ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (32, N'Davao de Oro                                      ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (33, N'Davao del Norte                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (34, N'Davao del Sur                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (35, N'Davao Occidental                                  ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (36, N'Davao Oriental                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (37, N'Dinagat Islands                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (38, N'Eastern Samar                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (39, N'General Santos City                               ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (40, N'Guimaras                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (41, N'Ifugao                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (42, N'Ilocos Norte                                      ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (43, N'Ilocos Sur                                        ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (44, N'Iloilo                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (45, N'Isabela                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (46, N'Kalinga                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (47, N'La Union                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (48, N'Laguna                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (49, N'Lanao del Norte                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (50, N'Lanao del Sur                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (51, N'Leyte                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (52, N'Maguindanao                                       ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (53, N'Marinduque                                        ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (54, N'Masbate                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (55, N'Misamis Occidental                                ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (56, N'Misamis Oriental                                  ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (57, N'Mountain Province                                 ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (58, N'NCR, City of Manila, First District               ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (59, N'NCR, Fourth District                              ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (60, N'NCR, Second District                              ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (61, N'NCR, Third District                               ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (62, N'Negros Occidental                                 ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (63, N'Negros Oriental                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (64, N'Northern Samar                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (65, N'Nueva Ecija                                       ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (66, N'Nueva Vizcaya                                     ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (67, N'Occidental Mindoro                                ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (68, N'Oriental Mindoro                                  ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (69, N'Palawan                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (70, N'Pampanga                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (71, N'Pangasinan                                        ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (72, N'Quezon                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (73, N'Quirino                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (74, N'Rizal                                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (75, N'Romblon                                           ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (76, N'Samar (Western Samar)                             ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (77, N'Sarangani                                         ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (78, N'Siquijor                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (79, N'Sorsogon                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (80, N'South Cotabato                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (81, N'Southern Leyte                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (82, N'Sultan Kudarat                                    ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (83, N'Sulu                                              ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (84, N'Surigao del Norte                                 ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (85, N'Surigao del Sur                                   ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (86, N'Tarlac                                            ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (87, N'Tawi-Tawi                                         ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (88, N'Zambales                                          ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (89, N'Zamboanga del Norte                               ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (90, N'Zamboanga del Sur                                 ')
INSERT [dbo].[Province] ([ProvinceId], [ProvinceName]) VALUES (91, N'Zamboanga Sibugay                                 ')
SET IDENTITY_INSERT [dbo].[Province] OFF
GO
