CREATE TABLE [dbo].[Devices]
(
	[Id] INT NOT NULL  IDENTITY, 
    [FriendlyName] NVARCHAR(512) NOT NULL, 
    [Path] NVARCHAR(1024) NOT NULL
)
