ALTER TABLE [lookup].[ContactReason] ADD CONSTRAINT [PK_ContactReasons] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
