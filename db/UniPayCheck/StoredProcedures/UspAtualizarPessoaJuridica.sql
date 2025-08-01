--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarPessoaJuridica]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarPessoaJuridica]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarPessoaJuridica]
	@Guid AS UNIQUEIDENTIFIER,
	@Cnpj AS CHAR(14),
	@DataFundacao AS DATE,
	@RazaoSocial AS VARCHAR(100),
	@IdUnidadeNegocio AS INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

UPDATE PJ
   SET [CNPJ] = @Cnpj,
       [DATA_FUNDACAO] = @DataFundacao,
	   [RAZAO_SOCIAL] = @RazaoSocial,
	   [IDUNIDADE_NEGOCIO] = @IdUnidadeNegocio
  FROM [dbo].[PESSOAS_JURIDICAS] AS PJ
 WHERE PJ.[GUID] = @Guid

GO