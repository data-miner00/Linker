CREATE TABLE [dbo].[Users] (
    [Id]          NVARCHAR (50)  NOT NULL,
    [Email]       NVARCHAR (50)  NOT NULL,
    [Username]    NVARCHAR (50)  NOT NULL,
    [Role]        NVARCHAR (50)  NOT NULL,
    [Status]      NVARCHAR (50)  NOT NULL,
    [DateOfBirth] DATE           NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [PhotoUrl]    NVARCHAR (200) NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    [ModifiedAt]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

