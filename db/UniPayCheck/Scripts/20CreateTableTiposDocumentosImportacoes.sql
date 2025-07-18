/****** Object:  Table [dbo].[CALCULOS]    Script Date: 18/06/2023 14:50:00 ******/
DROP TABLE [dbo].[TIPOS_DOCUMENTOS_IMPORTACOES]
GO

/****** Object:  Table [dbo].[CALCULOS]    Script Date: 18/06/2023 14:50:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TIPOS_DOCUMENTOS_IMPORTACOES](
	[ID] [int] NOT NULL,
	[DATA_INCLUSAO] [Datetime2] NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [Datetime2] NOT NULL,
	[DESCRICAO] [varchar](75) NOT NULL,
 CONSTRAINT [PK_TIPOS_DOCUMENTOS_IMPORTACOES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TIPOS_DOCUMENTOS_IMPORTACOES] ADD CONSTRAINT DF_TIPOS_DOCUMENTOS_IMPORTACOES_DATA_INCLUSAO DEFAULT GETUTCDATE() FOR [DATA_INCLUSAO]
GO