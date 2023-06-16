USE [UniPayCheck]
GO

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
	[DATA_ADMISSAO] [datetime] NOT NULL,
	[DATA_DEMISSAO] [datetime] NULL,
	[GUIDCOLABORADOR] [uniqueIdentifier] NOT NULL,
	[GUIDEMPREGADOR] [uniqueIdentifier] NOT NULL,
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