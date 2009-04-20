CREATE TABLE [lookup].[RollsheetType]
(
[Id] [int] NOT NULL,
[Code] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [lookup].[RollsheetType] ADD CONSTRAINT [PK_RollsheetType] PRIMARY KEY CLUSTERED ([Id])
GO
