<<<<<<< HEAD
USE [unipaycheck]
GO

/****** Object:  Table [dbo].[CEPS]    Script Date: 22/12/2024 02:16:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CEPS]') AND type in (N'U'))
DROP TABLE [dbo].[CEPS]
GO

/****** Object:  Table [dbo].[CEPS]    Script Date: 22/12/2024 02:16:38 ******/
=======
/****** Object:  Table [dbo].[CEPS]    Script Date: 22/12/2024 03:19:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CEPS]') AND type in (N'U'))
	DROP TABLE [dbo].[CEPS]
GO

/****** Object:  Table [dbo].[CEPS]    Script Date: 22/12/2024 03:19:42 ******/
>>>>>>> e69203f0a0669091cb760e2c777d3120431d0890
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CEPS](
	[CEP] [char](8) NOT NULL,
	[LOGRADOURO] [varchar](100) NOT NULL,
	[BAIRRO] [varchar](100) NULL,
	[CIDADE] [varchar](100) NOT NULL,
	[UF] [varchar](2) NOT NULL,
	[DATA_INCLUSAO] [datetime2](7) NOT NULL,
<<<<<<< HEAD
	[DATA_ULTIMA_ALTERACAO] [datetime2](7) NULL,
=======
	[DATA_ULTIMA_ALTERACAO] [datetime2](7) NOT NULL,
>>>>>>> e69203f0a0669091cb760e2c777d3120431d0890
 CONSTRAINT [PK_CEPS] PRIMARY KEY CLUSTERED 
(
	[CEP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
<<<<<<< HEAD
GO

=======
GO
>>>>>>> e69203f0a0669091cb760e2c777d3120431d0890
