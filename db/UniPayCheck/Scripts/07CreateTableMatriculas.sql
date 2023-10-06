ALTER TABLE [dbo].[MATRICULAS] DROP CONSTRAINT [FK_MATRICULAS_PESSOAS_JURIDICAS]
GO

ALTER TABLE [dbo].[MATRICULAS] DROP CONSTRAINT [FK_MATRICULAS_PESSOAS_FISICAS]
GO

/****** Object:  Table [dbo].[MATRICULAS]    Script Date: 19/05/2023 17:15:41 ******/
DROP TABLE [dbo].[MATRICULAS]
GO

/****** Object:  Table [dbo].[MATRICULAS]    Script Date: 19/05/2023 17:15:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MATRICULAS](
	[GUID] [uniqueIdentifier] NOT NULL,
	[MATRICULA] [varchar](20) NOT NULL,
	[DATA_ADMISSAO] [date] NOT NULL,
	[DATA_DEMISSAO] [date] NULL,
	[DATA_INCLUSAO] [Datetime2] NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [Datetime2] NULL,
	[DESCRICAO_CARGO] [varchar] (75) NOT NULL,
	[DESCRICAO_SETOR] [varchar] (75) NOT NULL,
	[FAIXA_IR] [SMALLINT] NOT NULL DEFAULT(0),
	[FAIXA_SF] [SMALLINT] NOT NULL DEFAULT(0),
	[GUIDCOLABORADOR] [uniqueIdentifier] NOT NULL,
	[GUIDEMPREGADOR] [uniqueIdentifier] NOT NULL,
	[AGENCIA] [varchar](9) NOT NULL,
	[BANCO] [varchar](3) NOT NULL,
	[CARGA_HORARIA] [decimal](6, 2) NOT NULL,
	[CONTA] [varchar](15) NOT NULL,
    [SALARIO_NOMINAL] [VARCHAR](128) NOT NULL
 CONSTRAINT [PK_MATRICULAS] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MATRICULAS]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_PESSOAS_FISICAS] FOREIGN KEY([GUIDCOLABORADOR])
REFERENCES [dbo].[PESSOAS_FISICAS] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS] CHECK CONSTRAINT [FK_MATRICULAS_PESSOAS_FISICAS]
GO

ALTER TABLE [dbo].[MATRICULAS]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_PESSOAS_JURIDICAS] FOREIGN KEY([GUIDEMPREGADOR])
REFERENCES [dbo].[PESSOAS_JURIDICAS] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS] CHECK CONSTRAINT [FK_MATRICULAS_PESSOAS_JURIDICAS]
GO

ALTER TABLE [dbo].[MATRICULAS] ADD CONSTRAINT DF_MATRICULAS_DATA_INCLUSAO DEFAULT GETUTCDATE() FOR [DATA_INCLUSAO]
GO

ALTER TABLE [dbo].[MATRICULAS] ADD CONSTRAINT DF_MATRICULAS_SALARIO_NOMINAL DEFAULT '0.01' FOR [SALARIO_NOMINAL]
GO