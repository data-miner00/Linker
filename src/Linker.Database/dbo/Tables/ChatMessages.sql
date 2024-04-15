CREATE TABLE [dbo].[ChatMessages] (
    [Id]          NVARCHAR (50)  NOT NULL,
    [AuthorId]    NVARCHAR (50)  NOT NULL,
    [WorkspaceId] NVARCHAR (50)  NOT NULL,
    [Message]     NVARCHAR (MAX) NOT NULL,
    [IsEdited]    BIT            NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    [ModifiedAt]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ChatMessages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_ChatMessages] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Workspaces_ChatMessages] FOREIGN KEY ([WorkspaceId]) REFERENCES [dbo].[Workspaces] ([Id])
);

