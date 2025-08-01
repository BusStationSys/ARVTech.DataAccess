--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirPessoaFisica]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirPessoaFisica]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirPessoaFisica]
	@Guid AS UNIQUEIDENTIFIER,
	@GuidPessoa AS UNIQUEIDENTIFIER,
	@Cpf AS CHAR(11),
	@Rg AS VARCHAR(20),
	@DataNascimento AS DATE,
	@Nome AS VARCHAR(100),
	@NumeroCtps AS VARCHAR(9),
	@SerieCtps AS VARCHAR(6),
	@UfCtps AS VARCHAR(2)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

INSERT INTO [dbo].[PESSOAS_FISICAS] ([GUID],
                                     [GUIDPESSOA],
									 [CPF],
									 [RG],
									 [DATA_NASCIMENTO],
									 [NOME],
									 [NUMERO_CTPS],
									 [SERIE_CTPS],
									 [UF_CTPS])
                             VALUES (@Guid,
							         @GuidPessoa,
									 @Cpf,
									 @Rg,
									 @DataNascimento,
									 @Nome,
									 @NumeroCtps,
									 @SerieCtps,
									 @UfCtps)

GO