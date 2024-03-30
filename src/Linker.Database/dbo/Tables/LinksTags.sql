CREATE TABLE [dbo].[LinksTags] (
    [LinkId] NVARCHAR (50) NOT NULL,
    [TagId]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LinksTags] PRIMARY KEY CLUSTERED ([LinkId] ASC, [TagId] ASC),
    CONSTRAINT [FK_LinksTags_Links] FOREIGN KEY ([LinkId]) REFERENCES [dbo].[Links] ([Id]),
    CONSTRAINT [FK_LinksTags_Tags] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id])
);

