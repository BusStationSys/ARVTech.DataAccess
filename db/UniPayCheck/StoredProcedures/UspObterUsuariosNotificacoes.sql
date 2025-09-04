--EXEC [UspObterUsuariosNotificacoes] null, NULL, NULL, NULL, NULL

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterUsuariosNotificacoes]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterUsuariosNotificacoes]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterUsuariosNotificacoes]
    @Tipo AS VARCHAR(2) = NULL,
	@GuidUsuario AS UNIQUEIDENTIFIER = NULL,
	@GuidMatriculaDemonstrativoPagamento AS UNIQUEIDENTIFIER = NULL,
	@GuidEmpregador AS UNIQUEIDENTIFIER = NULL,
	@GuidColaborador AS UNIQUEIDENTIFIER = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

SELECT *
  FROM vwUsuariosNotificacoes
 WHERE (@Tipo IS NULL OR [TIPO] = @Tipo)
   AND (@GuidUsuario IS NULL OR [GUIDUSUARIO] = @GuidUsuario)
   AND (@GuidMatriculaDemonstrativoPagamento IS NULL OR [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = @GuidMatriculaDemonstrativoPagamento)
   AND (@GuidEmpregador IS NULL OR [GUIDEMPREGADOR] = @GuidEmpregador)
   AND (@GuidColaborador IS NULL OR [GUIDCOLABORADOR] = @GuidColaborador)

GO