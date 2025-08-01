--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarPessoa]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarPessoa]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarPessoa]
	@Guid AS UNIQUEIDENTIFIER,
	@Cep AS CHAR(8),
	@Complemento AS VARCHAR(30),
	@Email AS VARCHAR(75),
	@Numero AS VARCHAR(10),
	@Telefone AS VARCHAR(30)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

  UPDATE P
     SET P.[CEP] = @Cep,
	     P.[COMPLEMENTO] = @Complemento,
		 P.[DATA_ULTIMA_ALTERACAO] = GETDATE(),
		 P.[EMAIL] = @Email,
		 P.[NUMERO] = @Numero,
		 P.[TELEFONE] = @Telefone
    FROM [dbo].[PESSOA] AS P
   WHERE P.[GUID] = @Guid

GO