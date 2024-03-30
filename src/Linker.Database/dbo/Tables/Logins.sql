CREATE TABLE [dbo].[Logins] (
    [Id]      NVARCHAR (50) NOT NULL,
    [UserId]  NVARCHAR (50) NOT NULL,
    [LoginAt] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Logins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Logins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

