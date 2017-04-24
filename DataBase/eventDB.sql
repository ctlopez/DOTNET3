/* Check if database already exists and delete it if it does exist */
IF EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'eventDBSecurity')
BEGIN
	DROP DATABASE eventDBSecurity
	print '' print '*** Dropping database eventDBSecurity'
END
GO


IF EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'eventDB')
BEGIN
	DROP DATABASE eventDB
	print '' print '*** Dropping database eventDB'
END
GO

print '' print '*** Creating database eventDB'
GO
CREATE DATABASE eventDB
GO

print '' print '*** Using database eventDB'
GO
USE [eventDB]
GO

/* guest table */
print '' print '** Creating Table Guest'
GO
CREATE TABLE [dbo].[Guest] (
	[GuestID] 		[int] IDENTITY (10000, 1)	NOT NULL,
	[RoomID]		[char](3)					NOT NULL,
	[FirstName]		[nvarchar](100)				NOT NULL,
	[LastName]		[nvarchar](200)				NOT NULL,
	[Phone]			[NVARCHAR](10)				NOT NULL,
	[Email]			[nvarchar](100)				NOT NULL,
	[Active]		[bit]			NOT NULL	DEFAULT 1,
	
	CONSTRAINT [pk_GuestID] PRIMARY KEY([GuestID] ASC)
)
GO

print '' print '*** Creating Index on RoomID in Guest Table'
GO
CREATE INDEX [Guest_RoomID] ON [Guest]([RoomID])
GO

print '' print '*** Creating Index on LastName in Guest Table'
GO
CREATE INDEX [Guest_LastName] ON [Guest]([LastName])
GO

print '' print '*** Inserting Guest Records ***'
GO
INSERT INTO [dbo].[Guest]
		([RoomID], [FirstName], [LastName], [Phone], [Email])
	VALUES
		('101', 'Ben', 'Dover', '1234567899', 'bendover@gmail.com'),
		('102', 'Seymour', 'Butz', '5467895468', 'fullmoon@yahoo.com'),
		('103', 'Amanda', 'Huggenkiss', '1324567896', 'amandahuggkiss@yahoo.com'),
		('115', 'Hugh', 'Jass', '9992245555', 'abigdonkey@yahoo.com'),
		('117', 'Bill', 'Loni', '7894567845', 'oscarmeyer@yahoo.com'),
		('118', 'Cole', 'Kutz', '3246876541', 'oscarmeyer@gmail.com'),
		('119', 'Sal', 'Lami', '3216549877', 'oscarmayar@gmail.com'),
		('200', 'Al', 'Coholic', '8521479645', 'drinkless@gmail.com'),
		('206', 'Jim', 'Nasium', '1324651245', 'excersize@gmail.com'),
		('210', 'Lou', 'Kout', '6542561389', 'eyesopen@yahoo.com'),
		('211', 'Eura', 'Snotball', '9649752050', 'itsacold@gmail.com'),
		('212', 'Ollie', 'Tabooker', '9875600240', 'stillacold@gmail.com'),
		('215', 'Ivana', 'Tinkle', '0021345200', 'usethejohn@yahoo.com'),
		('220', 'Stu', 'Pied', '1020345001', 'thinkharder@gmail.com'),
		('301', 'Ahmed', 'Adoodie', '8527419633', 'missedthejohn@gmail.com'),
		('303', 'Barry', 'Cade', '5469871233', 'lockup@gmail.com'),
		('305', 'Sid', 'Down', '2001458723', 'takeabreak@yahoo.com'),
		('306', 'Stan', 'Dup', '2001458724', 'atattention@yahoo.com'),
		('310', 'Adam', 'Zapel', '8857692349', 'caughtinyourthroat@gmail.com'),
		('313', 'Jean', 'Poole', '1324651002', 'youcantchoose@gmail.com'),
		('318', 'April', 'Furst', '7895014760', 'itsaprank@yahoo.com'),
		('320', 'Noah', 'Lott', '4782214543', 'einstein@gmail.com')
GO

print '' print '** Creating Table Room'
GO
CREATE TABLE [dbo].[Room] (
	[RoomID]		[char](3)			NOT NULL,
	[PINHash]		[nvarchar](100)		NOT NULL,
	
	CONSTRAINT [pk_RoomID] PRIMARY KEY([RoomID] ASC)
)
GO

print'' print '*** Creating Room Records ***'
GO
INSERT INTO [dbo].[Room]
		([RoomID], [PINHash])
	VALUES
		('100', '5e7b571a60a7c187d6a4cb8bbedbe4e69d4caa49b51d9ddf3320afd793f146bf'),
		('101', '07334386287751ba02a4588c1a0875dbd074a61bd9e6ab7c48d244eacd0c99e0'),
		('102', 'f19ccf1eb395a4d74606c59b491ecd0a37b5f26eae0ec55c33bbd2743c658b26'),
		('103', '06843e3f58776ec2eb5e0cc7a44a3c3fc1b4b9af2e75504da3d299dc566cc395'),
		('104', 'aeb32cfe00d196040e9758c276853282721fbd222038a54e9ae04d6686066e1b'),
		('105', '81dc948cd3fa9ec2064515b4267ef9a339993233dbdc0e984ce7b0fde6e1a0a9'),
		('106', 'd102a5e24978f472c57411fb2d5a04a7e23451955316112d8276637bda628eb0'),
		('107', '0c3d5517d7fe7feafefb2e59f5e1a9820cf2163afc655a74af95ee94e7e0f5a7'),
		('108', '61f9dc872774a4ba7e0034069aca7208febce8fae5bd55edf78bdc7a3286982e'),
		('109', '21e4bb031502fc787f31077ecbbf3c4050b0b0b72d86028c7970ce202b35919a'),
		('110', 'a5c3dd48facf21ed5f916d0ae979091fead570e6aea6c1d8038d1f68b26fa51f'),
		('111', '5bedd317328c9ed79ecd3aecee74d37f97813cb4ce61ef3402eb388fb369fa86'),
		('112', 'eb01dd90291c243ab2bfd5716c6b6bccd2a579c5bb5a1b987fb930bc5b78fbca'),
		('113', '9e337f285d52d6145ee7ed9f2fea1eddd59e07309b598f06a864785265b40186'),
		('114', '8cee7c089ae28cd5b43949ada8a93a6a343baf4e7efd8b9bbae686a2dbbdddeb'),
		('115', '7435242c50614bf90b6cf5f4eaed709af3afea3bf9c0c71f2383f4bb5f33d84c'),
		('116', '02ad1032015a37ff232529b9304192f22bb0cfd6603282115e851a627e79bb9d'),
		('117', '7c6886ff572fd550ea7fb64059a9f7082863f528c60f99a397dcd9b66fcab25f'),
		('118', '824d377a3a802d8610310c6fbb207f64f5432537a767ccc822a293f3c3725934'),
		('119', 'dd6570d45cfc25d7e0bec8b4ef11da488d983d0a5fa7b30f805ec01f05e8aeb6'),
		('120', 'd0d7f67ca7157d04e6b0d0f2e64bc99f50569100438d43f8f7d4759541753595'),
		('200', '1fdc13485c8abdffe049567b9d704156b8922886f60ea97141a895715da301ac'),
		('201', '1a1ebe2a67031edc537328fa42c56a4d639f116a534379f0be810bc533a11eac'),
		('202', '3472adbbcb9677d1b45365d37d96d1c33217d745567577fd9bd5c2766a258320'),
		('203', 'c2576dd8541a225cce015475e2d5abbac99f247b91447a5389826e2bc62740c0'),
		('204', '6249017f9372350bfc9cf3456c324bbb3661e1bb5a7a10d61912fd1be650d52f'),
		('205', 'b7fbb9f14ca908bf20c16e78647d36945e254a3b494982890cffb2e58cbc3d95'),
		('206', '8428b6c06c97baf6afe872c270bb5ca0fb32b0522cfcd14710d56747d375f61d'),
		('207', '2e3c969e2e9b367ade00279743f88ea0e32f283152223e3d9aaf1c14669d7cbe'),
		('208', '0421773648ad7d96b578b34b423057eeba404de4bce1a71cc3e296cd6faf20dc'),
		('209', 'cd2d44c931be8534bd5fa86f9d3cd8117537783d070895848e969e98791109b3'),
		('210', '82b87b56b096bcf4be784d993717c10443d843ebc880e4dae7cd5d049240d451'),
		('211', 'd852329eb1f82209e4c83c7a843552d9ee080716765cc96560189a2e982bef5a'),
		('212', 'b7ca93a0ea3d4b2d2330b5363aa251bf2c8bd184f3b4d8032b645064772ad214'),
		('213', 'f020272c938ba0a213d31613fd5a1a8b053c693d489551f8b24e900db43d6873'),
		('214', '025ac3ddef52e0aaa62896954c6fe57948a7c144cb203610499a912b832fd473'),
		('215', 'f3d17c70bc22a3b04c361fefa88e76b99877d3f82620fd59a432af4247abbf43'),
		('216', '3b2cbc8be13f3bad7d9049caf98fd558b191bc3ff2107638cf773eefbc4df512'),
		('217', '737513f7afb8668a9c9a489db2c4812419ad909bbc7d1989611852d8535909e7'),
		('218', 'f3fec7b5757f10b380bbcf7b5ac7e9fdb7a64e93392520ec6dbfdfe5e4f06ab3'),
		('219', '297a190ef0cdaef3c4b585c6e92893f1ee1bc9bcde2734cc2ea4eb9d38df52e5'),
		('220', '42ca2cc50238a49486ffbdacfe7229988db5049c4bb1c5f1de26fdb0883a48a0'),
		('300', 'cf3caaee01b6f4307b73c943b5363f1fe8594f9d9fbe1dbe4870c4861259c4ec'),
		('301', 'ddb1369d147b442dd34d5a1b000084cdd96f70ca6535b8d6636fe676b6248955'),
		('302', 'c28b39301e7e776610caa863459bf05c4fc9890471d9c77ad061253930dc36ff'),
		('303', '1a1024939ae1a1395ad5675ee62113f77267b55f46a805b15ccc0bbe7773aacb'),
		('304', '3812dd069593766d28dd6fe95a40d2540631adaffe8a6fead9a684fec89ddcfd'),
		('305', 'eaf493e8af54e4fb893988a51b83b390a725e24bbe1575a50625c811c08ecb6f'),
		('306', '7e831593833c68e3f2f6323a7d176d98fe2dd518e6847f813f528a513679333d'),
		('307', '76e685a70c02544b3fd01c0f03515ac859831ff260d09be12a866bb2b85c3922'),
		('308', 'd86d84d945596a7836b26b9e06e05042ba9617bac697b793b008fb4c4f7c218a'),
		('309', '5a796fc123ab4b7a692b6c57a8d78c013c34f7f0f9339e1d16f1d8942ad8af10'),
		('310', '408a5ee676694af19215cb22b2cd871e39c693f907933583ece00eda8d73ee16'),
		('311', '6d0cefe4c0ded1cd8ff2da4e69475dc5b83c7defea8f182200c5a3c18f2a0d46'),
		('312', '49df9bcdc4525530de9dbd9e677fe9e4897a1fe9b32e42ef1f9da60501739a00'),
		('313', '16d6eb9c4bf648aeb7e584e7b93b11e02da84596fe25bd0fdc438bd5f2e554e7'),
		('314', '8901e5df06abf35b898199e4b78a8376b724832f2206616315f5115b03896378'),
		('315', '63075cf5a34e6685ffb9be2e009b5ece6efae26232223575ac382fafa6ae335a'),
		('316', '37f6d9c7335d7a61a44de3aef5d6c209d043713bcdcf8ec362fa31764e510bc6'),
		('317', '5870a3c4ae138b679f29a8e90693123bf956b830c8c153ad902e112eef74f509'),
		('318', '9f79d46789159fde2ce018beb8417f80b6856be31d861688b59a2db120ae7e2b'),
		('319', '535bd5a2a206638a84a2a049d0ca1326b8e207c3b75a0ac6727bcfa2b63a5ac3'),
		('320', '1b8ba0b107410f67c70ab6ed4abf4e0ec0e70df78298e41d1670c7c1e94a703f')
GO

print '' print '** Creating Table RoomEvent'
GO
CREATE TABLE [dbo].[RoomEvent] (
	[RoomID]			[char](3)			NOT NULL,
	[EventID]			[int]				NOT NULL,
	[NumberReserved]	[int]				NOT NULL,
	
	CONSTRAINT [pk_RoomIDEventID] PRIMARY KEY([RoomID] ASC, [EventID] ASC)
)
GO

print '' print '*** Inserting RoomEvent Records **'
GO
INSERT INTO [dbo].[RoomEvent]
		([RoomID], [EventID], [NumberReserved])
	VALUES
		('101', 10000, 2),
		('101', 10003, 2),
		('202', 10001, 1),
		('303', 10000, 4),
		('303', 10002, 3)
GO

print '' print '** Creating Table Event'
GO
CREATE TABLE [dbo].[Event] (
	[EventID]		[int]	IDENTITY (10000, 1) 	NOT NULL,
	[Name]			[nvarchar](100)					NOT NULL,
	[Description]	[nvarchar](300)					NOT NULL,
	[Date]			[date]							NOT NULL,
	[Time]			[int]							NOT NULL,
	[Location]		[nvarchar](300)					NOT NULL,
	[MaxSeats]		[int]							NOT NULL,
	[Price]			[decimal](5,2)					NOT NULL,
	[AddedBy]		[int]							NOT NULL,
	[Active]		[bit]			NOT NULL		DEFAULT 1,
	
	CONSTRAINT [pk_EventID] PRIMARY KEY([EventID] ASC)
)
GO

print '' print '** Creating Date index on Event Table'
GO
CREATE INDEX [Event_Date] ON [Event]([Date])
GO

print '' print '** Creating Time index on Event Table'
GO
CREATE INDEX [Event_Time] ON [Event]([Time])
GO

print '' print '** Creating Price index on Event Table'
GO
CREATE INDEX [Event_Price] ON [Event]([Price])
GO

print '' print '*** Inserting Event Records ***'
GO
INSERT INTO [dbo].[Event]
		([Name], [Description], [Date], [Time], [Location], [MaxSeats], [Price], [AddedBy])
	VALUES
		('Night on the Town', 'Have a night in the city with discounts on popular places.', CONVERT(DATE, DATEADD(day, 1, GETDATE())), 1080, 'In the city', 500, 25.00, 10001),
		('City Bus Tour', 'Take a look at the city hot spots with this bus tour.', CONVERT(DATE, DATEADD(day, 5, GETDATE())), 600, 'Right outside the hotel', 30, 15.00, 10002),
		('City Bus Tour', 'Take a look at the city hot spots with this bus tour.', CONVERT(DATE, DATEADD(day, -2, GETDATE())), 870, 'Right outside the hotel', 30, 15.00, 10002),
		('A night of Dancing', 'We are bringing in a live band, "The Swing Boys," for you to dance the night away!', CONVERT(DATE, DATEADD(day, 15, GETDATE())), 1350, 'Hotel ballroom', 250, 40.00, 10001),
		('Carraige Rides', 'Take a horse drawn carraige to enjoy the city''s Christmas Lights', CONVERT(DATE, DATEADD(day, 7, GETDATE())), 1095, 'Right outside the hotel', 100, 10.00, 10002),
		('Bingo', 'Join us for a fun filled time playing bingo! Prizes are available!', CONVERT(DATE, DATEADD(day, 11, GETDATE())), 1170, 'Hotel Event Center', 400, 10.00, 10001),
		('Taste the World', 'We are bringing in great chefs to create tantalizing dishes from around the world!', CONVERT(DATE, DATEADD(day, 1, GETDATE())), 1050, 'Hotel Event Center', 350, 75.00, 10001)
GO

print '' print '** Creating Table Employee'
GO
CREATE TABLE [dbo].[Employee] (
	[EmployeeID]	[int]	IDENTITY (10000, 1)	NOT NULL,
	[FirstName]		[nvarchar](100)				NOT NULL,
	[LastName]		[nvarchar](200)				NOT NULL,
	[Phone]			[nvarchar](10)				NOT NULL,
	[Email]			[nvarchar](100)				NOT NULL,
	[Username]		[nvarchar](20)				NOT NULL,
	[PasswordHash]	[nvarchar](100)	NOT NULL	DEFAULT '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', /*password*/
	[Active]		[bit]			NOT NULL	DEFAULT 1,
	
	CONSTRAINT [pk_EmployeeID] PRIMARY KEY([EmployeeID] ASC),
	CONSTRAINT [ak_Username] UNIQUE([Username] ASC)
)
GO

print '' print '** Creating Index LastName on Employee Table'
GO
CREATE INDEX [Employee_LastName] ON [Employee]([LastName])
GO

print '' print '** Inserting Employee Records **'
GO
INSERT INTO [dbo].[Employee]
		([FirstName], [LastName], [Phone], [Email], [Username])
	VALUES
		('Joanne', 'Smith', '3195557777', 'joanne@hotel.com', 'jsmith'),
		('Martin', 'Jones', '3195558888', 'martin@hotel.com', 'mjones'),
		('Leo', 'Williams', '3195559999', 'leo@hotel.com', 'lwilliams')
GO

print '' print '** Creating Table EmployeeRole'
GO
CREATE TABLE [dbo].[EmployeeRole] (
	[EmployeeID]	[int]						NOT NULL,
	[RoleID]		[nvarchar](50)				NOT NULL,
	
	CONSTRAINT [pk_EmployeeIDRoleID] PRIMARY KEY([EmployeeID] ASC, [RoleID] ASC)
)
GO

print '' print '** Inserting EmployeeRole Records **'
GO
INSERT INTO [dbo].[EmployeeRole]
		([EmployeeID], [RoleID])
	VALUES
		(10000, 'Clerk'),
		(10001, 'Manager'),
		(10002, 'Clerk'),
		(10002, 'Manager')
GO

print '' print '** Creating Table Role'
GO
CREATE TABLE [dbo].[Role] (
	[RoleID]		[nvarchar](50)					NOT NULL,
	[Description]	[NVARCHAR](100)					NOT NULL,
	
	CONSTRAINT [pk_RoleID] PRIMARY KEY([RoleID] ASC)
)
GO

print '' print '** Inserting Role Records **'
GO
INSERT INTO [dbo].[Role]
		([RoleID], [Description])
	VALUES
		('Clerk', 'Able to assist guests regarding their events'),
		('Manager', 'Able to add, modify, or remove events in the system')
GO

print '' print '** Creating Guest RoomID foreign key'
GO
ALTER TABLE [dbo].[Guest] WITH NOCHECK
	ADD CONSTRAINT [fk_GuestRoomID] FOREIGN KEY ([RoomID])
	REFERENCES [dbo].[Room]([RoomID])
	ON UPDATE CASCADE
GO

print '' print '** Creating RoomEvent RoomID foreign key'
GO
ALTER TABLE [dbo].[RoomEvent] WITH NOCHECK
	ADD CONSTRAINT [fk_RoomEventRoomID] FOREIGN KEY ([RoomID])
	REFERENCES [dbo].[Room]([RoomID])
	ON UPDATE CASCADE
GO

print '' print '** Creating RoomEvent EventID foreign key'
GO
ALTER TABLE [dbo].[RoomEvent] WITH NOCHECK
	ADD CONSTRAINT [fk_RoomEventEventID] FOREIGN KEY ([EventID])
	REFERENCES [dbo].[Event]([EventID])
	ON UPDATE CASCADE
GO


print '' print '** Creating Event AddedBy foreign key'
GO
ALTER TABLE [dbo].[Event] WITH NOCHECK
	ADD CONSTRAINT [fk_EventAddedBy] FOREIGN KEY ([AddedBy])
	REFERENCES [dbo].[Employee]([EmployeeID])
	ON UPDATE CASCADE
GO

print '' print '** Creating EmployeeRole EmployeeID foreign key'
GO
ALTER TABLE [dbo].[EmployeeRole] WITH NOCHECK
	ADD CONSTRAINT [fk_EmployeeRoleEmployeeID] FOREIGN KEY ([EmployeeID])
	REFERENCES [dbo].[Employee]([EmployeeID])
	ON UPDATE CASCADE
GO

print '' print '** Creating EmployeeRole RoleID foreign key'
GO
ALTER TABLE [dbo].[EmployeeRole] WITH NOCHECK
	ADD CONSTRAINT [fk_EmployeeRoleRoleID] FOREIGN KEY ([RoleID])
	REFERENCES [dbo].[Role]([RoleID])
	ON UPDATE CASCADE
GO

print '' print '**Creating sp_authenticate_room'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_room]
	(
	@RoomID			char(3),
	@PINHash	varchar(100)
	)
AS
	BEGIN
		SELECT COUNT(RoomID)
		FROM Room
		WHERE RoomID = @RoomID
		AND  PINHash = @PINHash
	END
GO

print '' print '**Creating sp_get_guest_by_roomID'
GO
CREATE PROCEDURE [dbo].[sp_get_guest_by_roomID]
	(
	@RoomID		char(3)
	)
AS
	BEGIN
		SELECT GuestID, RoomID, FirstName, LastName, Phone, Email, Active
		FROM Guest
		WHERE RoomID = @RoomID
	END
GO

print '' print '**Creating sp_deactivate_old_events'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_old_events]
	(
	@CurrentDate	date
	)
AS
	BEGIN
		UPDATE Event
		SET Active = 0
		WHERE Event.Date < @CurrentDate
		AND EventID IN
			(SELECT EventID
			FROM Event
			WHERE Active = 1)
		
		RETURN @@ROWCOUNT
	END
GO

print '' print '**Creating sp_get_approaching_events'
GO
CREATE PROCEDURE [dbo].[sp_get_approaching_events]
	(
	@DateLimit date,
	@Active		bit
	)
AS
	BEGIN
		SELECT EventID, Name, Description, Date, Time, Location, MaxSeats, Price, AddedBy, Active
		FROM Event
		WHERE Event.Date < @DateLimit
		AND Active = @Active
		ORDER BY Date ASC, Time ASC
	END
GO

print '' print '**Creating sp_get_all_events'
GO
CREATE PROCEDURE [dbo].[sp_get_all_events]
	(
	@Active		bit
	)
AS
	BEGIN
		SELECT EventID, Name, Description, Date, Time, Location, MaxSeats, Price, AddedBy, Active
		FROM Event
		WHERE Active = @Active
		ORDER BY Date ASC, Time ASC
	END
GO

print '' print '**Creating sp_get_ticket_number'
GO
CREATE PROCEDURE [dbo].[sp_get_ticket_number]
	(
	@EventID	int
	)
AS
	BEGIN
		SELECT SUM(NumberReserved)
		FROM RoomEvent
		WHERE EventID = @EventID
	END
GO

print '' print '**Creating sp_purchase_new_tickets'
GO
CREATE PROCEDURE [dbo].[sp_purchase_new_tickets]
	(
	@RoomID				char(3),
	@EventID			int,
	@NumberReserved		int
	)
AS
	BEGIN
		INSERT INTO [dbo].[RoomEvent]
			([RoomID], [EventID], [NumberReserved])
		VALUES
			(@RoomID, @EventID, @NumberReserved)
			
		RETURN @@ROWCOUNT
	END
GO

print '' print '**Creating sp_purchase_more_tickets'
GO
CREATE PROCEDURE [dbo].[sp_purchase_more_tickets]
	(
	@RoomID			char(3),
	@EventID		int,
	@OldAmount		int,
	@NewAmount		int
	)
AS
	BEGIN
		UPDATE RoomEvent
		SET NumberReserved = @NewAmount
		WHERE RoomID = @RoomID
		AND EventID = @EventID
		AND NumberReserved = @OldAmount
		
		RETURN @@ROWCOUNT
	END
GO

print '' print '** Creating sp_lookup_room_event'
GO
CREATE PROCEDURE [dbo].[sp_lookup_room_event]
	(
	@RoomID		char(3),
	@EventID	int
	)
AS
	BEGIN
		SELECT Count(RoomID)
		FROM RoomEvent
		WHERE EventID = @EventID
		AND RoomID = @RoomID
	END
GO

print '' print '**Creating sp_current_number_of_tickets'
GO
CREATE PROCEDURE [dbo].[sp_current_number_of_tickets]
	(
	@RoomID		char(3),
	@EventID	int
	)
AS
	BEGIN
		SELECT SUM(NumberReserved)
		FROM RoomEvent
		WHERE RoomID = @RoomID
		AND EventID = @EventID
	END
GO


print '' print '**Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
	@Username		nvarchar(20),
	@PasswordHash	nvarchar(100)
	)
AS
	BEGIN
		SELECT COUNT(EmployeeID)
		FROM Employee
		WHERE Username = @Username
		AND PasswordHash = @PasswordHash
	END
GO


print '' print '*** Creating sp_retrieve_employee_by_username'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_employee_by_username]
	(
	@Username	nvarchar(20)
	)
AS
	BEGIN
		SELECT EmployeeID, FirstName, LastName, Phone, Email, UserName, Active
		FROM Employee
		WHERE UserName = @Username
	END
GO


print '' print '**Creating sp_retrieve_employee_roles'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_employee_roles]
	(
	@EmployeeID		int
	)
AS
	BEGIN
		SELECT [Role].RoleID, Description
		FROM EmployeeRole, Role
		WHERE [EmployeeRole].[EmployeeID] = @EmployeeID
		AND [EmployeeRole].[RoleID] = [Role].[RoleID]
	END
GO


print '' print'**Creating sp_insert_event'
GO
CREATE PROCEDURE [dbo].[sp_insert_event]
	(
	@Name			nvarchar(100),
	@Description	nvarchar(300),
	@Date			date,
	@Time			int,
	@Location		nvarchar(300),
	@MaxSeats		int,
	@Price			decimal(5,2),
	@AddedBy		int
	)
AS
	BEGIN
		INSERT INTO [dbo].[Event]
			([Name], [Description], [Date], [Time], [Location], [MaxSeats], [Price], [AddedBy])
		VALUES
			(@Name, @Description, @Date, @Time, @Location, @MaxSeats, @Price, @AddedBy)
			
		RETURN @@ROWCOUNT
	END
GO


print '' print '**Creating sp_update_event'
GO
CREATE PROCEDURE [dbo].[sp_update_event]
	(
	@EventID		int,
	
	@OldName		nvarchar(100),
	@OldDescription	nvarchar(300),
	@OldDate		date,
	@OldTime		int,
	@OldLocation	nvarchar(300),
	@OldMaxSeats	int,
	@OldPrice		decimal(5,2),
	@OldAddedBy		int,
	@OldActive		bit,
	
	@NewName		nvarchar(100),
	@NewDescription	nvarchar(300),
	@NewDate		date,
	@NewTime		int,
	@NewLocation	nvarchar(300),
	@NewMaxSeats	int,
	@NewPrice		decimal(5,2),
	@NewAddedBy		int,
	@NewActive		bit	
	)
AS
	BEGIN
		UPDATE Event
		SET Name = @NewName,
			Description = @NewDescription,
			Date = @NewDate,
			Time = @NewTime,
			Location = @NewLocation,
			MaxSeats = @NewMaxSeats,
			Price = @NewPrice,
			AddedBy = @NewAddedBy,
			Active = @NewActive
		WHERE Name = @OldName
			AND Description = @OldDescription
			AND Date = @OldDate
			AND Time = @OldTime
			AND Location = @OldLocation
			AND MaxSeats = @OldMaxSeats
			AND Price = @OldPrice
			AND AddedBy = @OldAddedBy
			AND Active = @OldActive
		AND EventID = @EventID
		
		RETURN @@ROWCOUNT
	END
GO

print '' print '**Creating sp_get_events_by_roomID'
GO
CREATE PROCEDURE [dbo].[sp_get_events_by_roomID]
	(
	@RoomID		char(3)
	)
AS
	BEGIN
		SELECT [Event].[EventID], Name, Date, Price, NumberReserved
		FROM Event, RoomEvent
		WHERE [Event].[EventID] = [RoomEvent].[EventID]
		AND RoomID = @RoomID
		ORDER BY Date
	END
GO

print '' print '**Creating sp_check_if_tickets_purchased'
GO
CREATE PROCEDURE [dbo].[sp_check_if_tickets_purchased]
	(
	@EventID	int
	)
AS
	BEGIN
		SELECT COUNT(EventID)
		FROM RoomEvent
		WHERE EventID = @EventID
	END
GO


print '' print '**Creating sp_delete_roomevent'
GO
CREATE PROCEDURE [dbo].[sp_delete_roomevent]
	(
	@RoomID		char(3),
	@EventID	int
	)
AS
	BEGIN
		DELETE FROM RoomEvent
		WHERE RoomID = @RoomID
		AND EventID = @EventID
		
		RETURN @@ROWCOUNT
	END
GO


print'' print '**Creating sp_retrieve_all_guests'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_all_guests]
AS
	BEGIN
		SELECT GuestID, RoomID, FirstName, LastName, Phone, Email, Active
		FROM Guest
	END
GO


print'' print '**Creating sp_retrieve_events_by_date'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_events_by_date]
	(
	@SearchDate	date,
	@Active		bit
	)
AS
	BEGIN
		SELECT EventID, Name, Description, Date, Time, Location, MaxSeats, Price, AddedBy, Active
		FROM Event
		WHERE Event.Date = @SearchDate
		AND Active = @Active
		ORDER BY Time ASC
	END
GO

print '' print '**Creating sp_update_room_pin'
GO
CREATE PROCEDURE [dbo].[sp_update_room_pin]
	(
	@RoomID		char(3),
	@OldPinHash	nvarchar(100),
	@NewPinHash	nvarchar(100)
	)
AS
	BEGIN
		UPDATE Room
		SET PINHash = @NewPinHash
		WHERE RoomID = @RoomID
		AND PINHash = @OldPinHash
		
		RETURN @@ROWCOUNT
	END
GO

print '' print '**Creating sp_update_user_password'
GO
CREATE PROCEDURE [dbo].[sp_update_user_password]
	(
	@EmployeeID			int,
	@OldPasswordHash	nvarchar(100),
	@NewPasswordHash	nvarchar(100)
	)
AS
	BEGIN
		UPDATE Employee
		SET PasswordHash = @NewPasswordHash
		WHERE EmployeeID = @EmployeeID
		AND PasswordHash = @OldPasswordHash
		
		RETURN @@ROWCOUNT
		
	END
GO

print '' print '**Creating sp_get_event_by_ID'
GO
CREATE PROCEDURE [dbo].[sp_get_event_by_ID]
	(
	@EventID	int
	)
AS
	BEGIN
		SELECT EventID, Name, Description, Date, Time, Location, MaxSeats, Price, AddedBy, Active
		FROM Event
		WHERE EventID = @EventID
	END
GO

print '' print '**Creating sp_get_events_regardless_active'
GO
CREATE PROCEDURE [dbo].[sp_get_events_regardless_active]
AS
	BEGIN
		SELECT EventID, Name, Description, Date, Time, Location, MaxSeats, Price, AddedBy, Active
		FROM Event
		ORDER BY DATE ASC, TIME ASC
	END
GO

print '' print '**Creating sp_retrieve_employee_by_id'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_employee_by_id]
	(
	@EmployeeID	int
	)
AS
	BEGIN
		SELECT EmployeeID, FirstName, LastName, Phone, Email, Username, Active
		FROM Employee
		WHERE EmployeeID = @EmployeeID
	END
GO

print '' print '**Creating procedure sp_deactivate_event_by_id'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_event_by_id]
	(
	@EventID	int
	)
AS
	BEGIN
		UPDATE Event
		SET Active = 0
		WHERE EventID = @EventID
		RETURN @@ROWCOUNT
	END
GO















