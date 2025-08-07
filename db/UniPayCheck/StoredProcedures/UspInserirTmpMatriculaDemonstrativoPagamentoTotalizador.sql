If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador]
	@GuidDemonstrativoPagamento UNIQUEIDENTIFIER,
	@IdTotalizador INT,
	@Valor VARCHAR(10)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

-- Verifica se o valor não está vazio
IF @Valor <> ''
BEGIN
    -- Substitui a vírgula por ponto
	SET @Valor = REPLACE(@Valor, '.', '')
	SET @Valor = REPLACE(@Valor, ',', '.')

	DECLARE @ValorDecimal   AS DECIMAL(8, 2) = CONVERT(DECIMAL(8, 2), @Valor)
	DECLARE @ValorVarbinary AS VARBINARY(MAX) = CONVERT(VARBINARY(MAX), CONVERT(VARCHAR(20), @ValorDecimal))

    -- Insere o valor na tabela temporária de totalizadores
    INSERT INTO [#TmpMatriculasDemonstrativosPagamentoTotalizadores]
                ([GUID],
				 [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
				 [IDTOTALIZADOR],
				 [VALOR])
         VALUES (NEWID(),
		         @GuidDemonstrativoPagamento,
				 @IdTotalizador,
				 @ValorVarbinary)
END

GO