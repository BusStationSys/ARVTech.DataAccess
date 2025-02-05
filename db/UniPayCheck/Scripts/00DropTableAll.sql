IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PUBLICACOES]') AND type in (N'U'))
DROP TABLE [dbo].[PUBLICACOES]
GO
/****** Object:  Table [dbo].[USUARIOS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USUARIOS]') AND type in (N'U'))
DROP TABLE [dbo].[USUARIOS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PERFIS_USUARIOS]') AND type in (N'U'))
DROP TABLE [dbo].[PERFIS_USUARIOS]
GO
/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_MARCACOES]
GO
/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO_CALCULOS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_ESPELHOS_PONTO_CALCULOS]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO_CALCULOS]
GO
/****** Object:  Table [dbo].[MATRICULAS_ESPELHOS_PONTO]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_ESPELHOS_PONTO]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_ESPELHOS_PONTO]
GO
/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]
GO
/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS]
GO
/****** Object:  Table [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
GO
/****** Object:  Table [dbo].[MATRICULAS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MATRICULAS]') AND type in (N'U'))
DROP TABLE [dbo].[MATRICULAS]
GO
/****** Object:  Table [dbo].[EVENTOS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EVENTOS]') AND type in (N'U'))
DROP TABLE [dbo].[EVENTOS]
GO
/****** Object:  Table [dbo].[CALCULOS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CALCULOS]') AND type in (N'U'))
DROP TABLE [dbo].[CALCULOS]
GO
/****** Object:  Table [dbo].[TOTALIZADORES]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TOTALIZADORES]') AND type in (N'U'))
DROP TABLE [dbo].[TOTALIZADORES]
GO
/****** Object:  Table [dbo].[PESSOAS_JURIDICAS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PESSOAS_JURIDICAS]') AND type in (N'U'))
DROP TABLE [dbo].[PESSOAS_JURIDICAS]
GO
/****** Object:  Table [dbo].[PESSOAS_FISICAS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PESSOAS_FISICAS]') AND type in (N'U'))
DROP TABLE [dbo].[PESSOAS_FISICAS]
GO
/****** Object:  Table [dbo].[PESSOAS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PESSOAS]') AND type in (N'U'))
DROP TABLE [dbo].[PESSOAS]
GO
/****** Object:  Table [dbo].[CEPS]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CEPS]') AND type in (N'U'))
DROP TABLE [dbo].[CEPS]
GO
/****** Object:  Table [dbo].[UNIDADES_NEGOCIO]    Script Date: 20/12/2023 17:33:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UNIDADES_NEGOCIO]') AND type in (N'U'))
DROP TABLE [dbo].[UNIDADES_NEGOCIO]
GO