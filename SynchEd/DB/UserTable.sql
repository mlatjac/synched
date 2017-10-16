CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL,
	[UserKey] CHAR(4) UNIQUE NOT NULL
)

INSERT INTO [dbo].[User] (Name,UserKey) VALUES 
('Alex','UUU1'),
('Arturo','UUU2'),
('Azam','UUU3'),
('Derek','UUU4'),
('Giorgio','UUU5'),
('Gregory','UUU6'),
('Hana','UUU7'),
('Hossein','UUU8'),
('Lucas','UUU9'),
('Miguel','UU10'),
('Miho','UU11'),
('Mohamed','UU12'),
('Mohammadjavad','UU13'),
('Mojgan','UU14'),
('Olesksii','UU15'),
('Roberto','UU18'),
('Vajihehalsadat','UU16'),
('Winoto','UU17'),
('Default User','UUUZ')
;

SELECT * FROM [dbo].[User] WHERE UserKey='UUUZ'
;

INSERT INTO [dbo].[User] (Name,UserKey) VALUES 
('Default User','UUUZ')
;
