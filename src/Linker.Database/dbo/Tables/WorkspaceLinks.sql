CREATE TABLE [dbo].[WorkspaceLinks] (
    [WorkspaceId] NVARCHAR (50) NOT NULL,
    [LinkId]      NVARCHAR (50) NOT NULL,
    [CreatedAt]   DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_WorkspaceLinks] PRIMARY KEY CLUSTERED ([WorkspaceId] ASC, [LinkId] ASC),
    CONSTRAINT [FK_WorkspaceLinks_Links] FOREIGN KEY ([LinkId]) REFERENCES [dbo].[Links] ([Id]),
    CONSTRAINT [FK_WorkspaceLinks_Workspaces] FOREIGN KEY ([WorkspaceId]) REFERENCES [dbo].[Workspaces] ([Id])
);



