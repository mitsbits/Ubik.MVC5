CREATE TABLE [dbo].[Contents]
(
	[ComponentId] INT NOT NULL, 
	[TextualId] INT NOT NULL ,
    [MetasInfo] XML NULL, 
    [CanonicalURL] VARCHAR(1024) NULL
)
