--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirPessoaJuridica]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirPessoaJuridica]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirPessoaJuridica]
	@Guid AS UNIQUEIDENTIFIER,
	@GuidPessoa AS UNIQUEIDENTIFIER,
	@Cnpj AS CHAR(14),
	@DataFundacao AS DATE,
	@RazaoSocial AS VARCHAR(100),
	@IdUnidadeNegocio AS INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[PESSOAS_JURIDICAS] ([GUID],
                                       [GUIDPESSOA],
									   [CNPJ],
									   [DATA_FUNDACAO],
									   [RAZAO_SOCIAL],
									   [IDUNIDADE_NEGOCIO])
                               VALUES (@Guid,
							           @GuidPessoa,
									   @Cnpj,
									   @DataFundacao,
									   @RazaoSocial,
									   @IdUnidadeNegocio)

GO