ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DROP CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MATRICULAS]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DROP CONSTRAINT [DF_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_DATA_INCLUSAO]
GO

/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]    Script Date: 16/01/2024 14:46:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
GO

/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]    Script Date: 16/01/2024 14:46:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO](
	[GUID] [uniqueidentifier] NOT NULL,
	[GUIDMATRICULA] [uniqueidentifier] NOT NULL,
	[COMPETENCIA] [char](8) NOT NULL,
	[DATA_INCLUSAO] [datetime2](7) NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [datetime2](7) NULL,
	[DATA_CONFIRMACAO] [datetime2](7) NULL,
	[IP_CONFIRMACAO] [varbinary](16) NULL,
	[CONTEUDO_ARQUIVO] [varchar](max) NULL,
 CONSTRAINT [PK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] ADD  CONSTRAINT [DF_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_DATA_INCLUSAO]  DEFAULT (getutcdate()) FOR [DATA_INCLUSAO]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MATRICULAS] FOREIGN KEY([GUIDMATRICULA])
REFERENCES [dbo].[MATRICULAS] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] CHECK CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MATRICULAS]
GO

--DROP INDEX [IX_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_COMPETENCIA] ON [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
--GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_COMPETENCIA]    Script Date: 16/01/2024 14:47:14 ******/
CREATE NONCLUSTERED INDEX [IX_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_COMPETENCIA] ON [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
(
	[COMPETENCIA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO