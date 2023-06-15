USE [UniPayCheck]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] DROP CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_TOTALIZADORES]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] DROP CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
GO

/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]    Script Date: 28/05/2023 19:32:21 ******/
DROP TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]
GO

/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]    Script Date: 28/05/2023 19:32:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES](
	[GUID] [uniqueidentifier] NOT NULL,
	[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] [uniqueidentifier] NOT NULL,
	[IDTOTALIZADOR] [int] NOT NULL,
	[VALOR] [decimal](25, 10) NULL,
 CONSTRAINT [PK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] FOREIGN KEY([GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO])
REFERENCES [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] ([GUID])
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] CHECK CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]  WITH CHECK ADD  CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_TOTALIZADORES] FOREIGN KEY([IDTOTALIZADOR])
REFERENCES [dbo].[TOTALIZADORES] ([ID])
GO

ALTER TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] CHECK CONSTRAINT [FK_MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES_TOTALIZADORES]
GO