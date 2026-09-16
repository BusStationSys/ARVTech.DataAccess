USE [unipaycheck]
GO

/****** Object:  UserDefinedTableType [dbo].[CredenciaisType]    Script Date: 01/06/2026 16:05:11 ******/
DROP TYPE [dbo].[CredenciaisType]
GO

/****** Object:  UserDefinedTableType [dbo].[CredenciaisType]    Script Date: 01/06/2026 16:05:11 ******/
CREATE TYPE [dbo].[CredenciaisType] AS TABLE(
	[GUIDCOLABORADOR] [uniqueidentifier] NULL,
	[USERNAME] [varchar](75) NULL,
	[PASSWORD_HASH] [varchar](256) NULL
)
GO