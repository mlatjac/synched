CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL,
	[UserKey] CHAR(4) UNIQUE NOT NULL
)

INSERT INTO [dbo].[User] (Name,UserKey) VALUES 
('Suzan','UUU1'),
('Bert','UUU2')
;

SELECT * FROM [dbo].[User] WHERE UserKey='UUUZ'
;

INSERT INTO [dbo].[User] (Name,UserKey) VALUES 
('Default User','UUUZ')
;