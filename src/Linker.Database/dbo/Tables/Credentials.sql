CREATE TABLE [dbo].[Credentials] (
    [UserId]               NVARCHAR (50)  NOT NULL,
    [PasswordHash]         NVARCHAR (200) NOT NULL,
    [PasswordSalt]         NVARCHAR (50)  NOT NULL,
    [HashAlgorithmType]    NVARCHAR (50)  NOT NULL,
    [PreviousPasswordHash] NVARCHAR (50)  NULL,
    [CreatedAt]            DATETIME2 (7)  NOT NULL,
    [ModifiedAt]           DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Credentials] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_Credentials_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);



