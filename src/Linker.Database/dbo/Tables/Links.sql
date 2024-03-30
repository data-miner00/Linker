CREATE TABLE [dbo].[Links] (
    [Id]             NVARCHAR (50)  NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Domain]         NVARCHAR (50)  NOT NULL,
    [Url]            NVARCHAR (100) NOT NULL,
    [Description]    NVARCHAR (200) NULL,
    [AddedBy]        NVARCHAR (50)  NOT NULL,
    [IsSubdomain]    BIT            NOT NULL,
    [IsMultilingual] BIT            NOT NULL,
    [IsResource]     BIT            NOT NULL,
    [Category]       NVARCHAR (50)  NOT NULL,
    [Language]       NVARCHAR (50)  NOT NULL,
    [Rating]         NVARCHAR (50)  NOT NULL,
    [Aesthetics]     NVARCHAR (50)  NOT NULL,
    [Type]           NVARCHAR (50)  NOT NULL,
    [Country]        NVARCHAR (50)  NULL,
    [KeyPersonName]  NVARCHAR (50)  NULL,
    [Grammar]        NVARCHAR (50)  NOT NULL,
    [Visibility]     NVARCHAR (50)  NOT NULL,
    [CreatedAt]      DATETIME2 (7)  NOT NULL,
    [ModifiedAt]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Links] PRIMARY KEY CLUSTERED ([Id] ASC)
);

