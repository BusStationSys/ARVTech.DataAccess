--EXEC [dbo].[UspObterGraficoEvolucaoSalarial] '3D12DFFE-FD7B-4477-92CC-4249248D8F6C'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterGraficoEvolucaoSalarial]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterGraficoEvolucaoSalarial]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterGraficoEvolucaoSalarial]
	@GuidUsuario AS UNIQUEIDENTIFIER = NULL,
	@QuantidadeMesesRetroativos AS TINYINT = 6

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

-- Tabela temporária para armazenar os dados do Gráfico de Evolução Salarial.
CREATE TABLE #TmpGraficoEvolucaoSalarial (
		TIPO VARCHAR(1),
        GUIDUSUARIO UNIQUEIDENTIFIER,
        COMPETENCIA VARCHAR(8),
        VALOR DECIMAL(11, 2))

DECLARE @IdEventoAdiantamentoQuinzenal AS INT = 892		--	ADIANTAMENTO QUINZENAL
DECLARE @IdTotalizador                 AS INT = 7		--	TOTAL LÍQUIDO

DECLARE @Contador  AS INT  = 0

DECLARE @DataAtual AS DATE

     SELECT TOP 1 @DataAtual = CONVERT(DATE, ISNULL(MAX(CONVERT(CHAR(8), MDP.COMPETENCIA)), CONVERT(CHAR(8), GETDATE(), 112)), 112)
       FROM [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] MDP
 INNER JOIN [MATRICULAS] M
         ON MDP.[GUIDMATRICULA] = M.[GUID]
 INNER JOIN [USUARIOS] U
         ON M.[GUIDCOLABORADOR] = U.[GUIDCOLABORADOR]
      WHERE (@GuidUsuario IS NULL OR U.[GUID] = @GuidUsuario)

WHILE @Contador < @QuantidadeMesesRetroativos
BEGIN
	PRINT (@Contador)

	DECLARE @DataRetroativa AS DATE = DATEADD(
		MONTH,
		@Contador * (-1),
		@DataAtual)

	PRINT (@DataRetroativa)

	DECLARE @Competencia AS VARCHAR (8) = CONCAT(
	    RIGHT('0' + CONVERT(VARCHAR(4), YEAR(@DataRetroativa)), 4),
        RIGHT('0' + CONVERT(VARCHAR(2), MONTH(@DataRetroativa)), 2),
		'01')

	PRINT (@Competencia)

		 INSERT INTO [#TmpGraficoEvolucaoSalarial] (TIPO, GUIDUSUARIO, COMPETENCIA, VALOR)
		 SELECT 'T',
				U.[GUID],
				MDP.[COMPETENCIA],
				CONVERT(DECIMAL(11, 2), CONVERT(VARCHAR(MAX), ISNULL(MDPT.[VALOR], 0))) AS VALOR
		   FROM [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] MDP
	 INNER JOIN [MATRICULAS] M
			 ON MDP.[GUIDMATRICULA] = M.[GUID]
	 INNER JOIN [USUARIOS] U
             ON M.[GUIDCOLABORADOR] = U.[GUIDCOLABORADOR]
	 INNER JOIN [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] MDPT
			 ON MDP.[GUID] = MDPT.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]
		  WHERE MDP.[COMPETENCIA] = @Competencia
		    AND MDPT.[IDTOTALIZADOR] = @IdTotalizador
			AND (@GuidUsuario IS NULL OR U.[GUID] = @GuidUsuario)

		 INSERT INTO [#TmpGraficoEvolucaoSalarial] (TIPO, GUIDUSUARIO, COMPETENCIA, VALOR)
		 SELECT 'E',
				U.[GUID],
				MDP.[COMPETENCIA],
				CONVERT(DECIMAL(11, 2), CONVERT(VARCHAR(MAX), ISNULL(MDPE.[VALOR], 0))) AS VALOR
		   FROM [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] MDP
	 INNER JOIN [MATRICULAS] M
			 ON MDP.[GUIDMATRICULA] = M.[GUID]
	 INNER JOIN [USUARIOS] U
             ON M.[GUIDCOLABORADOR] = U.[GUIDCOLABORADOR]
	 INNER JOIN [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS] MDPE
			 ON MDP.[GUID] = MDPE.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO]
		  WHERE MDP.[COMPETENCIA] = @Competencia
		    AND MDPE.[IDEVENTO] = @IdEventoAdiantamentoQuinzenal
			AND (@GuidUsuario IS NULL OR U.[GUID] = @GuidUsuario)

	-- Verifica se foram inseridos registros para o mês atual.
    IF NOT EXISTS (SELECT TOP 1 1
	                 FROM [#TmpGraficoEvolucaoSalarial]
	                WHERE COMPETENCIA = @Competencia
					  AND (@GuidUsuario IS NULL OR GUIDUSUARIO = @GuidUsuario))
    BEGIN
        -- Se não houver, insere um registro com valor zero tanto para (T)otalizador como para (E)vento.
        INSERT INTO [#TmpGraficoEvolucaoSalarial] (TIPO, GUIDUSUARIO, COMPETENCIA, VALOR)
        SELECT 'T', 
		       U.[GUID],
			   @Competencia,
			   0
		  FROM [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] MDP
	INNER JOIN [MATRICULAS] M
		    ON MDP.[GUIDMATRICULA] = M.[GUID]
	INNER JOIN [USUARIOS] U
            ON M.[GUIDCOLABORADOR] = U.[GUIDCOLABORADOR]
         WHERE (@GuidUsuario IS NULL OR U.[GUID] = @GuidUsuario)

        INSERT INTO [#TmpGraficoEvolucaoSalarial] (TIPO, GUIDUSUARIO, COMPETENCIA, VALOR)
        SELECT 'E', 
		       U.[GUID],
			   @Competencia,
			   0
		  FROM [MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] MDP
	INNER JOIN [MATRICULAS] M
	        ON MDP.[GUIDMATRICULA] = M.[GUID]
	INNER JOIN [USUARIOS] U
            ON M.[GUIDCOLABORADOR] = U.[GUIDCOLABORADOR]
         WHERE (@GuidUsuario IS NULL OR U.[GUID] = @GuidUsuario)
    END

	SET @Contador = @Contador + 1
END

  SELECT [GUIDUSUARIO],
         [COMPETENCIA],
		 SUM([VALOR]) AS [VALOR]
	FROM [#TmpGraficoEvolucaoSalarial]
GROUP BY [GUIDUSUARIO], [COMPETENCIA]
ORDER BY [GUIDUSUARIO], [COMPETENCIA] DESC

DROP TABLE [#TmpGraficoEvolucaoSalarial]

GO