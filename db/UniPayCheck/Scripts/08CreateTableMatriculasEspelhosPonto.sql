USE [UniPayCheck]
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO] DROP CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MATRICULAS]
GO

/****** Object:  Table [dbo].[MATRICULAS_ESPELHO_PONTO]    Script Date: 19/05/2023 17:15:28 ******/
DROP TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO]
GO

/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO]    Script Date: 19/05/2023 17:15:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO](
	[GUID] [uniqueIdentifier] NOT NULL,
	[DATA_MARCACAO] [date] NOT NULL,
	[DATA_INCLUSAO] [Datetime2] NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [Datetime2] NULL,
	[MARCACAO_EXTENSO] [VARCHAR](MAX) NOT NULL,
	[COMPETENCIA] [char](6) NULL,
	[GUIDMATRICULA] [uniqueIdentifier] NOT NULL,
 CONSTRAINT [PK_MATRICULAS_ESPELHO_PONTO] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MATRICULAS] FOREIGN KEY([GUIDMATRICULA])
REFERENCES [dbo].[MATRICULAS] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO] CHECK CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MATRICULAS]
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO] ADD CONSTRAINT DF_MATRICULAS_ESPELHOS_PONTO_DATA_INCLUSAO DEFAULT GETUTCDATE() FOR [DATA_INCLUSAO]
GO