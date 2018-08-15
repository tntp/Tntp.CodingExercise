CREATE TABLE [dbo].[Comments]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(15) NOT NULL, 
    [Content] NVARCHAR(140) NOT NULL, 
    [CreationTimestamp] DATETIME2 NOT NULL DEFAULT sysdatetime()
)

GO

CREATE INDEX [IX_Comments_CreationTimestamp] ON [dbo].[Comments] ([CreationTimestamp] DESC)
