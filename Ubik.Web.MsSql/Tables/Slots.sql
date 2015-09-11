CREATE TABLE [dbo].[Slots]
(
	[SectionId] INT NOT NULL, 
    [Enabled] BIT NOT NULL, 
    [Ordinal] INT NOT NULL, 
    [Module] XML NULL 
)
