CREATE TABLE [dbo].[WorkspaceMemberships] (
    [WorkspaceId]   NVARCHAR (50) NOT NULL,
    [UserId]        NVARCHAR (50) NOT NULL,
    [WorkspaceRole] NVARCHAR (50) NOT NULL,
    [CreatedAt]     DATETIME2 (7) NOT NULL,
    [ModifiedAt]    DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_WorkspaceMemberships] PRIMARY KEY CLUSTERED ([WorkspaceId] ASC, [UserId] ASC),
    CONSTRAINT [FK_WorkspaceMemberships_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_WorkspaceMemberships_Workspace] FOREIGN KEY ([WorkspaceId]) REFERENCES [dbo].[Workspaces] ([Id])
);

