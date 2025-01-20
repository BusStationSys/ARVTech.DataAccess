If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspImportarArquivoEspelhosPonto]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspImportarArquivoEspelhosPonto]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspImportarArquivoEspelhosPonto]
	@Cnpj CHAR(14),
    @Conteudo VARCHAR(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

--	Faz a verificação se o CNPJ que está sendo importadas as matrículas existe.
DECLARE @GuidEmpregador AS UNIQUEIDENTIFIER = (SELECT TOP 1 [GUID]
                                                     FROM dbo.[PESSOAS_JURIDICAS]
				                                    WHERE [CNPJ] = @Cnpj)

IF @GuidEmpregador = NULL
BEGIN
	RAISERROR ('Pessoa Jurídica não encontrada para o CNPJ %s. O registro deve ser cadastrado/importado préviamente.', 16, 1, @Cnpj)
	RETURN
END

DECLARE @DataAtual DATETIME2 = GETDATE()

DECLARE @GuidEspelhoPonto AS UNIQUEIDENTIFIER = NULL

DECLARE @QuantidadeRegistrosInseridos	INT = 0
DECLARE @QuantidadeRegistrosAtualizados	INT = 0
DECLARE @QuantidadeRegistrosInalterados	INT = 0
DECLARE @QuantidadeRegistrosRejeitados	INT = 0

-- Variáveis para controle da extração de linhas
DECLARE @Line							VARCHAR(MAX)
DECLARE @StartIndex						INT = 1
DECLARE @EndIndex						INT

-------------------------------------------
-- Cria tabela temporária das MATRÍCULAS --
-------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculas] FROM dbo.[MATRICULAS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculas] ON [#TmpMatriculas] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial.
INSERT INTO [#TmpMatriculas]
     SELECT * FROM dbo.[MATRICULAS] WITH (NOLOCK)

----------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS ESPELHOS PONTO --
----------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasEspelhosPonto] FROM dbo.[MATRICULAS_ESPELHOS_PONTO] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasEspelhosPonto] ON [#TmpMatriculasEspelhosPonto] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial.
INSERT INTO [#TmpMatriculasEspelhosPonto]
     SELECT * FROM dbo.[MATRICULAS_ESPELHOS_PONTO] WITH (NOLOCK)

--------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS ESPELHOS PONTO MARCAÇÕES --
--------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasEspelhosPontoMarcacoes] FROM dbo.[MATRICULAS_ESPELHOS_PONTO_MARCACOES] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasEspelhosPontoMarcacoes] ON [#TmpMatriculasEspelhosPontoMarcacoes] ([Guid])

-------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS ESPELHOS PONTO CÁLCULOS --
-------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasEspelhosPontoCalculos] FROM dbo.[MATRICULAS_ESPELHOS_PONTO_CALCULOS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasEspelhosPontoCalculos] ON [#TmpMatriculasEspelhosPontoCalculos] ([Guid])

-- Loop para processar cada linha da string.
WHILE @StartIndex > 0
BEGIN
    -- Encontra o índice da próxima quebra de linha (CRLF).
    SET @EndIndex = CHARINDEX(CHAR(13) + CHAR(10), @Conteudo, @StartIndex);

    -- Se não encontrar uma quebra de linha, usa o restante da string.
    IF @EndIndex = 0
    BEGIN
        SET @Line = SUBSTRING(@Conteudo, @StartIndex, LEN(@Conteudo) - @StartIndex + 1);

        SET @StartIndex = 0;  -- Finaliza o loop
    END
    ELSE
    BEGIN
        -- Extrai a linha com base nos índices encontrados.
        SET @Line = SUBSTRING(@Conteudo, @StartIndex, @EndIndex - @StartIndex);

        -- Move o índice de início para o próximo caractere após a quebra de linha.
        SET @StartIndex = @EndIndex + 2;
    END

	--	Tipo de Registro
	DECLARE @Registro AS CHAR(1) = LTRIM(RTRIM(SUBSTRING(@Line, 1, 1)))

	IF @Registro = '1'
	BEGIN
		SET @GuidEspelhoPonto = NULL

		--	Matrícula
		DECLARE @Matricula AS VARCHAR(20) = LTRIM(RTRIM(SUBSTRING(@Line, 2, 10)))

		--	Competência
		DECLARE @Competencia AS VARCHAR(7) = LTRIM(RTRIM(SUBSTRING(@Line, 135, 7)))
		SET @Competencia = REPLACE(@Competencia, '/', '')

		--	Verifica se existe a Matrícula, se não existir, tem que pular para o próximo registro
		DECLARE @GuidMatricula AS UNIQUEIDENTIFIER = (SELECT TOP 1 M.[GUID]
			                                            FROM [dbo].[MATRICULAS] as M WITH(NOLOCK)
	                                                   WHERE M.[MATRICULA] = @Matricula)

		IF @GuidMatricula IS NULL
		BEGIN
			PRINT 'Matrícula ' + @Matricula + ' não encontrada. O registro deve ser cadastrado/importado préviamente.'

			SET @QuantidadeRegistrosRejeitados = @QuantidadeRegistrosRejeitados + 1
		END
		ELSE
		BEGIN
	 	    SELECT TOP 1 @GuidEspelhoPonto = T.[GUID]
			  FROM [#TmpMatriculasEspelhosPonto] AS T
	         WHERE T.[GUIDMATRICULA] = @GuidMatricula
			   AND T.[COMPETENCIA] = @Competencia

			--PRINT @GuidMatricula
			--PRINT @GuidEspelhoPonto
			--PRINT '-'

			IF @GuidEspelhoPonto IS NULL
			BEGIN
				SET @GuidEspelhoPonto = NEWID()

				INSERT INTO [#TmpMatriculasEspelhosPonto] ([GUID],
				                                           [DATA_INCLUSAO],
														   [DATA_ULTIMA_ALTERACAO],
														   [COMPETENCIA],
														   [GUIDMATRICULA])
												   VALUES (@GuidEspelhoPonto,
												           @DataAtual,
														   @DataAtual,
														   @Competencia,
														   @GuidMatricula)
			END
			ELSE
			BEGIN
				UPDATE [#TmpMatriculasEspelhosPonto] 
				   SET [DATA_ULTIMA_ALTERACAO] = @DataAtual
				 WHERE [GUID] = @GuidEspelhoPonto
			END
		END
	END
	ELSE IF @Registro = '2' AND NOT @GuidEspelhoPonto IS NULL
	BEGIN
		DECLARE @GuidMarcacao AS UNIQUEIDENTIFIER = NEWID()

		--	Dia
		DECLARE @DiaMarcacao AS VARCHAR(2) = LTRIM(RTRIM(SUBSTRING(@Line, 2, 2)))

		--	Data da Marcação
		DECLARE @DataMarcacao AS VARCHAR(10) = CONCAT(
											      @DiaMarcacao,
												  @Competencia)

        SET @DataMarcacao = CONCAT(
		                       SUBSTRING(@DataMarcacao, 5, 4),
							   '-',
							   SUBSTRING(@DataMarcacao, 3, 2),
							   '-',
							   SUBSTRING(@DataMarcacao, 1, 2))

		--	Marcação
        DECLARE @Marcacao AS VARCHAR(MAX) = LTRIM(RTRIM(SUBSTRING(@Line, 9, 82)))

		--	Horas Trabalhadas
        DECLARE @HorasTrabalhadas AS VARCHAR(5) = LTRIM(RTRIM(SUBSTRING(@Line, 91, 5)))

		--	Horas Faltas
        DECLARE @HorasFaltas AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 97, 5))))

		--	Horas Extras 50%
        DECLARE @HorasExtras050 AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 103, 5))))
	
		--	Horas Extras 70%
        DECLARE @HorasExtras070 AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 109, 5))))

		--	Horas Extras 100%
		DECLARE @HorasExtras100 AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 115, 5))))

		--	Horas Crédito Banco de Horas
		DECLARE @HorasCreditoBH AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 121, 5))))

		--	Horas Débito Banco de Horas
		DECLARE @HorasDebitoBH AS VARCHAR(5) = CONVERT(TIME, LTRIM(RTRIM(SUBSTRING(@Line, 127, 5))))

        INSERT INTO [#TmpMatriculasEspelhosPontoMarcacoes]
		            ([GUID],
					 [GUIDMATRICULA_ESPELHO_PONTO],
					 [DATA],
					 [MARCACAO],
					 [HORAS_EXTRAS_050],
					 [HORAS_EXTRAS_070],
					 [HORAS_EXTRAS_100],
					 [HORAS_CREDITO_BH],
					 [HORAS_DEBITO_BH],
					 [HORAS_FALTAS],
					 [HORAS_TRABALHADAS])
			 VALUES (@GuidMarcacao,
			         @GuidEspelhoPonto,
					 CONVERT(DATE, @DataMarcacao),
					 @Marcacao,
					 CONVERT(TIME, @HorasExtras050),
					 CONVERT(TIME, @HorasExtras070),
					 CONVERT(TIME, @HorasExtras100),
					 CONVERT(TIME, @HorasCreditoBH),
					 CONVERT(TIME, @HorasDebitoBH),
					 CONVERT(TIME, @HorasFaltas),
					 CONVERT(TIME, @HorasTrabalhadas))
	END
	ELSE IF @Registro = '3' AND NOT @GuidEspelhoPonto IS NULL
	BEGIN
		-- 1. Total Horas Extras 50%
		DECLARE @TotalHorasExtras050 AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 2, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 1, @TotalHorasExtras050

		-- 2. Total Horas Extras 70%
		DECLARE @TotalHorasExtras070 AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 12, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 2, @TotalHorasExtras070

		-- 3. Total Horas Extras 100%
		DECLARE @TotalHorasExtras100 AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 22, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 3, @TotalHorasExtras100

		-- 4. Total Adicional Noturno
		DECLARE @TotalAdicionalNoturno AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 32, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 4, @TotalAdicionalNoturno

		-- 5. Total Atestado
		DECLARE @TotalAtestado AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 42, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 5, @TotalAtestado

		--	6. Paternidade
		DECLARE @TotalPaternidade AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 52, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 6, @TotalPaternidade

		--	7. Seguro
		DECLARE @TotalSeguro AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 62, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 7, @TotalSeguro

		--	8. Faltas
		DECLARE @TotalFaltas AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 72, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 8, @TotalFaltas

		--	9. Faltas Justificadas
		DECLARE @TotalFaltasJustificadas AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 82, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 9, @TotalFaltasJustificadas

		--	10. Atraso
		DECLARE @TotalAtraso AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 92, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 10, @TotalAtraso

		--	11. Crédito BH
		DECLARE @TotalCreditoBH AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 102, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 11, @TotalCreditoBH

		--	12. Débito BH
		DECLARE @TotalDebitoBH AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 112, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 12, @TotalDebitoBH

		--	13. Saldo BH
		DECLARE @TotalSaldoBH AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 122, 10)))
		SET @TotalSaldoBH = REPLACE(@TotalSaldoBH, '+', '')

		IF CHARINDEX('-', @TotalSaldoBH) > 0
		BEGIN
			SET @TotalSaldoBH = LTRIM(RTRIM(REPLACE(@TotalSaldoBH, '-', '')))

			SET @TotalSaldoBH = CONCAT('-', @TotalSaldoBH)
		END

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 13, @TotalSaldoBH

		--	14. Dispensa Não Remunerada
		DECLARE @TotalDispensaNaoRemunerada AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 132, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 14, @TotalDispensaNaoRemunerada

		--	15. Grat. Ad. Fech
		DECLARE @TotalGratAdFech AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 142, 10)))

		-- Chama o procedimento para processar e inserir o cálculo
		EXEC [dbo].[UspInserirTmpMatriculaEspelhoPontoCalculo] @GuidEspelhoPonto, 15, @TotalGratAdFech
	END
END

--select * from [#TmpMatriculasEspelhosPonto]
--select * from [#TmpMatriculasEspelhosPontoCalculos]
--select * from [#TmpMatriculasEspelhosPontoMarcacoes]

--DROP TABLE [#TmpMatriculasEspelhosPontoMarcacoes]
--DROP TABLE [#TmpMatriculasEspelhosPontoCalculos]
--DROP TABLE [#TmpMatriculasEspelhosPonto]
--DROP TABLE [#TmpMatriculas]

--RETURN

--	Atualiza os campos VARCHAR da tabela PESSOAS com NULL se estiverem não preenchidos.
--UPDATE T
--   SET T.[CEP] = NULL
--  FROM [#TmpPessoas] T
-- WHERE COALESCE(T.[CEP], '') = ''

--UPDATE T
--   SET T.[COMPLEMENTO] = NULL
--  FROM [#TmpPessoas] T
-- WHERE COALESCE(T.[COMPLEMENTO], '') = ''

--UPDATE T
--   SET T.[EMAIL] = NULL
--  FROM [#TmpPessoas] T
-- WHERE COALESCE(T.[EMAIL], '') = ''

--UPDATE T
--   SET T.[NUMERO] = NULL
--  FROM [#TmpPessoas] T
-- WHERE COALESCE(T.[NUMERO], '') = ''

--UPDATE T
--   SET T.[TELEFONE] = NULL
--  FROM [#TmpPessoas] T
-- WHERE COALESCE(T.[TELEFONE], '') = ''

BEGIN TRANSACTION

--	Atualiza os ESPELHOS DE PONTO que tiveram dados alterados.
    UPDATE EP
       SET EP.[DATA_ULTIMA_ALTERACAO] = T.[DATA_ULTIMA_ALTERACAO]
      FROM [#TmpMatriculasEspelhosPonto] T
INNER JOIN [dbo].[MATRICULAS_ESPELHOS_PONTO] EP WITH (NOLOCK)
        ON EP.[COMPETENCIA] = T.[COMPETENCIA]
	   AND EP.[GUIDMATRICULA] = T.[GUIDMATRICULA]
     WHERE EP.[DATA_ULTIMA_ALTERACAO] <> T.[DATA_ULTIMA_ALTERACAO]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Exclui os vínculos (ESPELHOS DE PONTO x ESPELHOS DE PONTO MARCAÇÕES) de Marcações dos Espelhos de Ponto que serão alterados.
    DELETE EPM
      FROM dbo.[MATRICULAS_ESPELHOS_PONTO_MARCACOES] EPM WITH (NOLOCK)
INNER JOIN [#TmpMatriculasEspelhosPonto] T
        ON EPM.[GUIDMATRICULA_ESPELHO_PONTO] = T.[GUID]
     WHERE T.[DATA_ULTIMA_ALTERACAO] = @DataAtual

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Exclui os vínculos (ESPELHOS DE PONTO x ESPELHOS DE PONTO CÁLCULOS) de Cálculos dos Espelhos de Ponto que serão alterados.
    DELETE EPC
      FROM dbo.[MATRICULAS_ESPELHOS_PONTO_CALCULOS] EPC WITH (NOLOCK)
INNER JOIN [#TmpMatriculasEspelhosPonto] T
        ON EPC.[GUIDMATRICULA_ESPELHO_PONTO] = T.[GUID]
     WHERE T.[DATA_ULTIMA_ALTERACAO] = @DataAtual

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os novos registros de ESPELHOS DE PONTO na tabela.
    INSERT INTO dbo.[MATRICULAS_ESPELHOS_PONTO]
         SELECT T.[GUID],
				T.[DATA_INCLUSAO],
				T.[DATA_INCLUSAO],
				T.[COMPETENCIA],
				T.[GUIDMATRICULA],
				NULL,
				NULL,
				NULL
           FROM [#TmpMatriculasEspelhosPonto] T
LEFT OUTER JOIN dbo.[MATRICULAS_ESPELHOS_PONTO] EP WITH (NOLOCK)
             ON EP.[COMPETENCIA] = T.[COMPETENCIA]
	        AND EP.[GUIDMATRICULA] = T.[GUIDMATRICULA]
          WHERE EP.[GUID] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os registros de MARCAÇÕES DO ESPELHOS DE PONTO na tabela.
INSERT INTO dbo.[MATRICULAS_ESPELHOS_PONTO_MARCACOES]
     SELECT T1.[GUID],
	        T1.[GUIDMATRICULA_ESPELHO_PONTO],
            T1.[DATA],
			T1.[MARCACAO],
			T1.[HORAS_EXTRAS_050],
			T1.[HORAS_EXTRAS_070],
			T1.[HORAS_EXTRAS_100],
			T1.[HORAS_CREDITO_BH],
			T1.[HORAS_DEBITO_BH],
			T1.[HORAS_FALTAS],
			T1.[HORAS_TRABALHADAS]
       FROM [#TmpMatriculasEspelhosPontoMarcacoes] T1
 INNER JOIN [#TmpMatriculasEspelhosPonto] T2
         ON T1.[GUIDMATRICULA_ESPELHO_PONTO] = T2.[GUID]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os registros de CÁLCULOS DO ESPELHOS DE PONTO na tabela.
INSERT INTO dbo.[MATRICULAS_ESPELHOS_PONTO_CALCULOS]
     SELECT T1.[GUID],
	        T1.[GUIDMATRICULA_ESPELHO_PONTO],
            T1.[IDCALCULO],
			T1.[VALOR]
       FROM [#TmpMatriculasEspelhosPontoCalculos] T1
 INNER JOIN [#TmpMatriculasEspelhosPonto] T2
         ON T1.[GUIDMATRICULA_ESPELHO_PONTO] = T2.[GUID]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

COMMIT TRANSACTION

--	Contabiliza os registros atualizados.
SET @QuantidadeRegistrosAtualizados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS_ESPELHOS_PONTO] EP
                                            WHERE EP.[DATA_INCLUSAO] <> EP.[DATA_ULTIMA_ALTERACAO]
											  AND EP.[DATA_ULTIMA_ALTERACAO] = @DataAtual)

SET @QuantidadeRegistrosInalterados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS_ESPELHOS_PONTO] EP
                                            WHERE EP.[DATA_ULTIMA_ALTERACAO] < @DataAtual )

SET @QuantidadeRegistrosInseridos = (    SELECT COUNT(*)
                                           FROM dbo.[MATRICULAS_ESPELHOS_PONTO] EP
                                          WHERE EP.[DATA_INCLUSAO] = EP.[DATA_ULTIMA_ALTERACAO]
									        AND EP.[DATA_INCLUSAO] = @DataAtual )

SELECT @DataAtual AS 'DATA_INICIO',
       GETDATE() AS 'DATA_FIM',
       @QuantidadeRegistrosAtualizados AS 'QUANTIDADE_REGISTROS_ATUALIZADOS',
       @QuantidadeRegistrosInalterados AS 'QUANTIDADE_REGISTROS_INALTERADOS',
	   @QuantidadeRegistrosInseridos   AS 'QUANTIDADE_REGISTROS_INSERIDOS',
	   @QuantidadeRegistrosRejeitados  AS 'QUANTIDADE_REGISTROS_REJEITADOS'

FINALIZA:

DROP TABLE [#TmpMatriculasEspelhosPontoMarcacoes]
DROP TABLE [#TmpMatriculasEspelhosPonto]
DROP TABLE [#TmpMatriculas]

GO

--DELETE FROM MATRICULAS
--DELETE FROM USUARIOS
--DELETE FROM PESSOAS_FISICAS
--DELETE FROM PESSOAS WHERE CHAVE_EXPORTACAO_IMPORTACAO LIKE '%PF%'