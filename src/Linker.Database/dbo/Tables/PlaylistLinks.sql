CREATE TABLE [dbo].[PlaylistLinks] (
    [PlaylistId] NVARCHAR (50) NOT NULL,
    [LinkId]     NVARCHAR (50) NOT NULL,
    [CreatedAt]  DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_PlaylistLinks] PRIMARY KEY CLUSTERED ([PlaylistId] ASC, [LinkId] ASC),
    CONSTRAINT [FK_PlaylistLinks_Links] FOREIGN KEY ([LinkId]) REFERENCES [dbo].[Links] ([Id]),
    CONSTRAINT [FK_PlaylistLinks_Playlists] FOREIGN KEY ([PlaylistId]) REFERENCES [dbo].[Playlists] ([Id])
);

