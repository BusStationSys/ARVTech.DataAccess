--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarPessoaFisica]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarPessoaFisica]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarPessoaFisica]
	@Guid AS UNIQUEIDENTIFIER,
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

UPDATE PF
   SET [CPF] = @Cpf,
       [RG] = @Rg,
	   [DATA_NASCIMENTO] = @DataNascimento,
	   [NOME] = @Nome,
	   [NUMERO_CTPS] = @NumeroCtps,
	   [SERIE_CTPS] = @SerieCtps,
	   [UF_CTPS] = @UfCtps
  FROM [dbo].[PESSOAS_FISICAS] AS PF
 WHERE PF.[GUID] = @Guid

GO