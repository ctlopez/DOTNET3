print '' print '*** Using database eventDBSecurity'
GO
USE [eventDBSecurity]
GO

print '' print '*** Creating users to log in'
GO
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'58073d7e-0e2f-4c3f-833c-47c02d238c79', N'drinkless@gmail.com', 0, N'AGJKzyXuSZNTLSCsz1FpVxxbQIP0MGZej/YhFnjietENraVcTLNCF4Aoq2G2+Eb3GQ==', N'5cf20a2b-5998-49ec-b42e-90377671d26e', NULL, 0, 0, NULL, 1, 0, N'200')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'61ed9f08-c2d4-481a-a2b7-59308e909b7c', N'amandahuggkiss@yahoo.com', 0, N'AMZWnXslCDoR9guaNh+3dtAmOIEVS8tZwI3FvKY+f4VSuVLh2XNHIavvtwTGjaa9jQ==', N'ea417778-9e58-4f59-8e90-729cd228e420', NULL, 0, 0, NULL, 1, 0, N'103')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'66d46014-9c18-4f23-804e-934a3c96cbdd', N'atattention@yahoo.com', 0, N'ANmLA479bTXpsZCLhGiNwGTyz7M5SSXYIxz0WeviVcbx1HhAukEG6JA6iVISapuQ9w==', N'a76417cf-65fe-4b29-823e-6cb2c7ad7c46', NULL, 0, 0, NULL, 1, 0, N'306')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'7cf51803-428a-43c0-b4c6-f2ddf1ceced6', N'joanne@hotel.com', 0, N'AKkeT820q5RRLYAq6Hzu3cyRvKq8N2cru8gqsls1g9ojsEMnCeHw0OmDzfWZXkCfFw==', N'6fafd3b1-c546-40db-bddb-5667d1dd3861', NULL, 0, 0, NULL, 1, 0, N'jsmith')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'800be4d6-58ef-440b-8963-61f36a69d94c', N'usethejohn@yahoo.com', 0, N'AKgRwbn/VhETcvuOw8F3DXb8M6mhKa3vRXi1yyZfET9gI7GDt6zQrk8ddtGBBj3vTw==', N'8f8f3aa7-c593-4477-8ca4-57ec2a0884d0', NULL, 0, 0, NULL, 1, 0, N'215')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b2971283-ae94-424d-86b3-a2c00aa08364', N'bendover@gmail.com', 0, N'ACspvEWrsL0ru2o3cBfMo+ujjS0TV03BlvVZuHnN9ns0QXt1DbfrdcaUI69M9qi4ug==', N'b07db61c-5572-41eb-9837-db3f21a7f24b', NULL, 0, 0, NULL, 1, 0, N'101')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'b335e4aa-dab6-4fea-91ca-682f54a9edbb', N'leo@hotel.com', 0, N'AGOPSX5nBi6R1+K0bvns73Id9k88+c7gT+CTiDiBDv4jZsS7seR9wqtFHkVxSrwaZg==', N'c6c9d250-22ec-4434-9fc2-e098e806f6e4', NULL, 0, 0, NULL, 1, 0, N'lwilliams')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'admin@test.com', 0, N'AOoTtdlYmu+3bqA6CTDYbpq5q5CIio+toMSEJyyupRmz4xRzYX4gK9k/SlrrpCky5A==', N'9681dcd4-46ae-491b-90e3-7311a048776d', NULL, 0, 0, NULL, 0, 0, N'admin@test.com')
GO

print '' print '*** Creating claims on the users'
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'System')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Administrator')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (3, N'b2971283-ae94-424d-86b3-a2c00aa08364', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Ben')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (4, N'b2971283-ae94-424d-86b3-a2c00aa08364', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Dover')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (5, N'61ed9f08-c2d4-481a-a2b7-59308e909b7c', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Amanda')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (6, N'61ed9f08-c2d4-481a-a2b7-59308e909b7c', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Huggenkiss')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (7, N'58073d7e-0e2f-4c3f-833c-47c02d238c79', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Al')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (8, N'58073d7e-0e2f-4c3f-833c-47c02d238c79', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Coholic')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (9, N'800be4d6-58ef-440b-8963-61f36a69d94c', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Ivana')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (10, N'800be4d6-58ef-440b-8963-61f36a69d94c', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Tinkle')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (11, N'66d46014-9c18-4f23-804e-934a3c96cbdd', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Stan')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (12, N'66d46014-9c18-4f23-804e-934a3c96cbdd', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Dup')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (13, N'7cf51803-428a-43c0-b4c6-f2ddf1ceced6', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Joanne')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (14, N'7cf51803-428a-43c0-b4c6-f2ddf1ceced6', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Smith')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (15, N'b335e4aa-dab6-4fea-91ca-682f54a9edbb', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', N'Leo')
INSERT INTO [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (16, N'b335e4aa-dab6-4fea-91ca-682f54a9edbb', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname', N'Williams')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF

GO

print '' print '*** Creating static roles'
GO
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'da6db3b9-2f05-4fe5-b2d5-694846a0de1f', N'Administrator')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'aeb7f98a-02f8-4a49-a3d9-5deeebb47b99', N'Clerk')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8', N'Guest')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'd1ecd1a5-a235-4e60-909a-19df2dd6198b', N'Manager')
GO


print '' print '*** Creating link between user and roles'
GO
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7cf51803-428a-43c0-b4c6-f2ddf1ceced6', N'aeb7f98a-02f8-4a49-a3d9-5deeebb47b99')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b335e4aa-dab6-4fea-91ca-682f54a9edbb', N'aeb7f98a-02f8-4a49-a3d9-5deeebb47b99')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'aeb7f98a-02f8-4a49-a3d9-5deeebb47b99')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b335e4aa-dab6-4fea-91ca-682f54a9edbb', N'd1ecd1a5-a235-4e60-909a-19df2dd6198b')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'd1ecd1a5-a235-4e60-909a-19df2dd6198b')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e45a192a-d2b3-4d4c-a9f0-3e243cca0557', N'da6db3b9-2f05-4fe5-b2d5-694846a0de1f')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'58073d7e-0e2f-4c3f-833c-47c02d238c79', N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'61ed9f08-c2d4-481a-a2b7-59308e909b7c', N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'66d46014-9c18-4f23-804e-934a3c96cbdd', N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'800be4d6-58ef-440b-8963-61f36a69d94c', N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b2971283-ae94-424d-86b3-a2c00aa08364', N'f55911ef-b634-43f6-86bd-bc9e3a2d81b8')
GO
