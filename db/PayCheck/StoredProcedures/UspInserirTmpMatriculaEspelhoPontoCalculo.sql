If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo]
	@GuidEspelhoPonto UNIQUEIDENTIFIER,
	@IdCalculo INT,
	@Valor VARCHAR(10)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

-- Verifica se o valor não está vazio
IF @Valor <> ''
BEGIN
    -- Substitui a vírgula por ponto
    SET @Valor = REPLACE(@Valor, ',', '.')

    -- Insere o valor na tabela temporária de cálculos
    INSERT INTO [#TmpMatriculasEspelhosPontoCalculos]
                ([GUID],
				 [GUIDMATRICULA_ESPELHO_PONTO],
				 [IDCALCULO],
				 [VALOR])
         VALUES (NEWID(),
		         @GuidEspelhoPonto,
				 @IdCalculo,
				 CONVERT(DECIMAL(25, 10), @Valor))
END

GO