USE [UniPayCheck]
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES] DROP CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MARCACOES_MATRICULAS_ESPELHOS_PONTO]
GO

/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]    Script Date: 23/08/2023 02:14:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]
GO

/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]    Script Date: 23/08/2023 02:14:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES](
	[GUID] [uniqueidentifier] NOT NULL,
	[GUIDMATRICULA_ESPELHO_PONTO] [uniqueidentifier] NOT NULL,
	[DATA] [date] NOT NULL,
	[MARCACAO] [varchar](max) NOT NULL,
	[HORAS_EXTRAS_50] [time](7) NULL,
	[HORAS_EXTRAS_70] [time](7) NULL,
	[HORAS_EXTRAS_100] [time](7) NULL,
	[HORAS_CREDITO_BH] [time](7) NULL,
	[HORAS_DEBITO_BH] [time](7) NULL,
	[HORAS_FALTAS] [time](7) NULL,
 CONSTRAINT [PK_MATRICULAS_ESPELHOS_PONTO_MARCACOES] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MARCACOES_MATRICULAS_ESPELHOS_PONTO] FOREIGN KEY([GUIDMATRICULA_ESPELHO_PONTO])
REFERENCES [dbo].[MATRICULAS_ESPELHOS_PONTO] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES] CHECK CONSTRAINT [FK_MATRICULAS_ESPELHOS_PONTO_MARCACOES_MATRICULAS_ESPELHOS_PONTO]
GO

