CREATE TABLE [dbo].[Workspaces] (
    [Id]          NVARCHAR (50)  NOT NULL,
    [Handle]      NVARCHAR (50)  NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [OwnerId]     NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    [ModifiedAt]  DATETIME2 (7)  NOT NULL,
    [Visibility] NVARCHAR(50) NOT NULL, 
    [MaxMemberCount] SMALLINT NULL, 
    CONSTRAINT [PK_Workspaces] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Workspaces_Users] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Users] ([Id])
);

