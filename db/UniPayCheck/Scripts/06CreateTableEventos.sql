EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EVENTOS', @level2type=N'COLUMN',@level2name=N'TIPO'

GO

ALTER TABLE [dbo].[EVENTOS] DROP CONSTRAINT [DF_EVENTOS_TIPO]
GO

/****** Object:  Table [dbo].[EVENTOS]    Script Date: 28/05/2023 20:19:24 ******/
DROP TABLE [dbo].[EVENTOS]
GO

/****** Object:  Table [dbo].[EVENTOS]    Script Date: 28/05/2023 20:19:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EVENTOS](
	[ID] [int] NOT NULL,
	[DATA_INCLUSAO] [Datetime2] NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [Datetime2] NULL,
	[DESCRICAO] [varchar](75) NOT NULL,
	[TIPO] [char](1) NOT NULL,
	[OBSERVACOES] [varchar](max) NULL,
 CONSTRAINT [PK_EVENTOS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[EVENTOS] ADD  CONSTRAINT [DF_EVENTOS_TIPO]  DEFAULT ('I') FOR [TIPO]
GO

ALTER TABLE [dbo].[EVENTOS] ADD CONSTRAINT DF_EVENTOS_DATA_INCLUSAO DEFAULT GETUTCDATE() FOR [DATA_INCLUSAO]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'(I)nformativo; (D)esconto; (V)encimento.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EVENTOS', @level2type=N'COLUMN',@level2name=N'TIPO'
GO