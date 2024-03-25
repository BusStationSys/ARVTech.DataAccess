ALTER TABLE [dbo].[PESSOAS_FISICAS] DROP CONSTRAINT [FK_PESSOAS_FISICAS_PESSOAS]
GO

ALTER TABLE [dbo].[PESSOAS_FISICAS] DROP CONSTRAINT [DF_PESSOAS_FISICAS_DATA_INCLUSAO]
GO

/****** Object:  Table [dbo].[PESSOAS_FISICAS]    Script Date: 16/01/2024 14:55:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PESSOAS_FISICAS]') AND type in (N'U'))
DROP TABLE [dbo].[PESSOAS_FISICAS]
GO

/****** Object:  Table [dbo].[PESSOAS_FISICAS]    Script Date: 16/01/2024 14:55:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PESSOAS_FISICAS](
	[GUID] [uniqueidentifier] NOT NULL,
	[GUIDPESSOA] [uniqueidentifier] NOT NULL,
	[CPF] [char](11) NOT NULL,
	[FOTO] [varbinary](max) NULL,
	[RG] [varchar](20) NULL,
	[DATA_NASCIMENTO] [date] NULL,
	[DATA_INCLUSAO] [datetime2](7) NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [datetime2](7) NULL,
	[NOME] [varchar](100) NOT NULL,
	[NUMERO_CTPS] [varchar](9) NOT NULL,
	[SERIE_CTPS] [varchar](5) NOT NULL,
	[UF_CTPS] [varchar](2) NOT NULL,
 CONSTRAINT [PK_PESSOAS_FISICAS] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PESSOAS_FISICAS] ADD  CONSTRAINT [DF_PESSOAS_FISICAS_DATA_INCLUSAO]  DEFAULT (getutcdate()) FOR [DATA_INCLUSAO]
GO

ALTER TABLE [dbo].[PESSOAS_FISICAS]  WITH CHECK ADD  CONSTRAINT [FK_PESSOAS_FISICAS_PESSOAS] FOREIGN KEY([GUIDPESSOA])
REFERENCES [dbo].[PESSOAS] ([GUID])
GO

ALTER TABLE [dbo].[PESSOAS_FISICAS] CHECK CONSTRAINT [FK_PESSOAS_FISICAS_PESSOAS]
GO

--DROP INDEX [IX_CPF] ON [dbo].[PESSOAS_FISICAS]
--GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_CPF]    Script Date: 16/01/2024 14:57:34 ******/
CREATE NONCLUSTERED INDEX [IX_CPF] ON [dbo].[PESSOAS_FISICAS]
(
	[CPF] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

--DROP INDEX [UQ_CPF] ON [dbo].[PESSOAS_FISICAS]
--GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [UQ_CPF]    Script Date: 16/01/2024 14:57:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_CPF] ON [dbo].[PESSOAS_FISICAS]
(
	[CPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO