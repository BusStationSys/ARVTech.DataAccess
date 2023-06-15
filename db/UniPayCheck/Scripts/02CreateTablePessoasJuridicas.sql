USE [UniPayCheck]
GO

ALTER TABLE [dbo].[PESSOAS_JURIDICAS] DROP CONSTRAINT [FK_PESSOAS_JURIDICAS_PESSOAS]
GO

/****** Object:  Table [dbo].[PESSOAS_JURIDICAS]    Script Date: 15/06/2023 02:05:41 ******/
DROP TABLE [dbo].[PESSOAS_JURIDICAS]
GO

/****** Object:  Table [dbo].[PESSOAS_JURIDICAS]    Script Date: 15/06/2023 02:05:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PESSOAS_JURIDICAS](
	[GUID] [uniqueidentifier] NOT NULL,
	[GUIDPESSOA] [uniqueidentifier] NOT NULL,
	[CNPJ] [char](14) NOT NULL,
	[DATA_FUNDACAO] [date] NULL,
	[RAZAO_SOCIAL] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PESSOAS_JURIDICAS] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PESSOAS_JURIDICAS]  WITH CHECK ADD  CONSTRAINT [FK_PESSOAS_JURIDICAS_PESSOAS] FOREIGN KEY([GUIDPESSOA])
REFERENCES [dbo].[PESSOAS] ([GUID])
GO

ALTER TABLE [dbo].[PESSOAS_JURIDICAS] CHECK CONSTRAINT [FK_PESSOAS_JURIDICAS_PESSOAS]
GO

