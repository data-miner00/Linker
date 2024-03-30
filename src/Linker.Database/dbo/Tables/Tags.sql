CREATE TABLE [dbo].[Tags] (
    [Id]         NVARCHAR (50) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [CreatedAt]  DATETIME2 (7) NOT NULL,
    [ModifiedAt] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC)
);

