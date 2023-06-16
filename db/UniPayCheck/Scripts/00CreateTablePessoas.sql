USE [UniPayCheck]
GO

/****** Object:  Table [dbo].[PESSOAS]    Script Date: 19/05/2023 17:16:21 ******/
DROP TABLE [dbo].[PESSOAS]
GO

/****** Object:  Table [dbo].[PESSOAS]    Script Date: 19/05/2023 17:16:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PESSOAS](
	[GUID] [uniqueidentifier] NOT NULL,
	[BAIRRO] [varchar](40) NULL,
	[CEP] [char](8) NULL,
	[CIDADE] [varchar](60) NOT NULL,
	[COMPLEMENTO] [varchar](30) NULL,
	[ENDERECO] [varchar](100) NOT NULL,
	[NUMERO] [varchar](10) NULL,
	[UF] [char](2) NOT NULL,
 CONSTRAINT [PK_PESSOAS] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO