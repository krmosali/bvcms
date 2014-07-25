CREATE TABLE [dbo].[Transaction]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[TransactionDate] [datetime] NULL,
[TransactionGateway] [nvarchar] (50) NULL,
[DatumId] [int] NULL,
[testing] [bit] NULL,
[amt] [money] NULL,
[ApprovalCode] [nvarchar] (150) NULL,
[Approved] [bit] NULL,
[TransactionId] [nvarchar] (50) NULL,
[Message] [nvarchar] (150) NULL,
[AuthCode] [nvarchar] (150) NULL,
[amtdue] [money] NULL,
[URL] [nvarchar] (300) NULL,
[Description] [nvarchar] (180) NULL,
[Name] [nvarchar] (100) NULL,
[Address] [nvarchar] (50) NULL,
[City] [nvarchar] (50) NULL,
[State] [nvarchar] (20) NULL,
[Zip] [nvarchar] (15) NULL,
[Phone] [nvarchar] (20) NULL,
[Emails] [nvarchar] (max) NULL,
[Participants] [nvarchar] (max) NULL,
[OrgId] [int] NULL,
[OriginalId] [int] NULL,
[regfees] [money] NULL,
[donate] [money] NULL,
[fund] [nvarchar] (50) NULL,
[financeonly] [bit] NULL,
[voided] [bit] NULL,
[credited] [bit] NULL,
[coupon] [bit] NULL,
[moneytran] AS (CONVERT([bit],case when [amt]<>(0) AND [approved]=(1) then (1) else (0) end,(0))),
[settled] [datetime] NULL,
[batch] [datetime] NULL,
[batchref] [nvarchar] (50) NULL,
[batchtyp] [nvarchar] (50) NULL,
[fromsage] [bit] NULL,
[LoginPeopleId] [int] NULL,
[First] [nvarchar] (50) NULL,
[MiddleInitial] [nvarchar] (1) NULL,
[Last] [nvarchar] (50) NULL,
[Suffix] [nvarchar] (10) NULL,
[AdjustFee] [bit] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
