CREATE TABLE [dbo].[HealthCheckResults] (
    [Url]           NVARCHAR (200) NOT NULL,
    [Status]        NVARCHAR (50)  NOT NULL,
    [LastCheckedAt] DATETIME2 (7)  NOT NULL,
    [ErrorMessage]  NVARCHAR (MAX) NULL,
    [DeadAt]        DATETIME2 (7)  NULL,
    CONSTRAINT [PK_HealthCheckResults] PRIMARY KEY CLUSTERED ([Url] ASC)
);

