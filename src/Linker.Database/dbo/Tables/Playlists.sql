CREATE TABLE [dbo].[Playlists] (
    [Id]          NVARCHAR (50)  NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (255) NOT NULL,
    [Visibility]  NVARCHAR (50)  NOT NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    [ModifiedAt]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED ([Id] ASC)
);

