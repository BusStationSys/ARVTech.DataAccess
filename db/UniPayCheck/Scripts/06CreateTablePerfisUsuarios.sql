/****** Object:  Table [dbo].[EVENTOS]    Script Date: 28/05/2023 20:19:24 ******/
DROP TABLE [dbo].[PERFIS_USUARIOS]
GO

/****** Object:  Table [dbo].[EVENTOS]    Script Date: 28/05/2023 20:19:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PERFIS_USUARIOS](
	[ID] [int] NOT NULL,
	[DATA_INCLUSAO] [Datetime2] NOT NULL,
	[DATA_ULTIMA_ALTERACAO] [Datetime2] NULL,
	[DESCRICAO] [varchar](75) NOT NULL
 CONSTRAINT [PK_PERFIS_USUARIOS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PERFIS_USUARIOS] ADD CONSTRAINT DF_PERFIS_USUARIOS_DATA_INCLUSAO DEFAULT GETUTCDATE() FOR [DATA_INCLUSAO]
GO