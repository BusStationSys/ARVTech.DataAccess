--EXEC [dbo].[UspImportarArquivoDemonstrativosPagamento]
--'07718633006896',
--'104/2024   Mensal    0000000001UNIDASUL DIST ALIMENTICIA S.A. FILIAL 68Esteio              6808      Matriz    A4                                                                                        
--2     85666ALISSON DE ANDRADE RAFO SILVA           Aprendiz Serv.Comér,Bens,TurisCAD II - ADMINISTRATIVO F68   000575198-2061/RS   09/10/20230000      6,42033010900000710251231                         
--3  001SALARIO NORMAL                 60,00    385,20V                                                                                                                                                    
--3  008REPOUSO REMUNERADO              6,67     42,80V                                                                                                                                                    
--3  057SALARIO DOENCA                 36,67    235,40V                                                                                                                                                    
--3  855INSS                            7,50     49,75D                                                                                                                                                    
--4    663,40    663,40    663,40     13,26    663,40     49,75    613,65                                                                                                                                  
--5                                                                                                                                                                                                        '

/*
EXEC [dbo].[UspImportarArquivoDemonstrativosPagamento]
'07718633006896',
'104/2024   Mensal    0000000016UNIDASUL DIST ALIMENTICIA S.A. FILIAL 68Esteio              6808      Fil 068   A4                                                                                        
2     70143ISMAEL DE LIMA BOENO                    Conferente de Hortifruti      FILIAL 41 - HORTIFRUTI NOITE  001034126-0050/RS   01/07/20200000  2.143,00033013430000710254424                         
3  001SALARIO NORMAL                220,00  2.143,00V                                                                                                                                                    
3  098INTEG. HE REP REMUN.            1,58     15,40V                                                                                                                                                    
3  403BANCO DE HORAS 50%              6,85    100,09V                                                                                                                                                    
3  513PRA TI COMPRAS                          199,68D                                                                                                                                                    
3  515SACOLA ECONOMICA                          1,50D                                                                                                                                                    
3  666VALE TRANSPORTE               108,00    120,00D                                                                                                                                                    
3  694REFEICAO                                 10,41D                                                                                                                                                    
3  855INSS                            9,00    182,08D                                                                                                                                                    
3  892ADIANTAMENTO QUINZENAL                  621,60D                                                                                                                                                    
4  2.258,49  1.636,89  2.258,49    180,67  2.258,49  1.135,27  1.123,22                                                                                                                                  
5                                        Parabéns! Feliz Aniversário em 6 / 5                                                                                                                            '
*/

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspImportarArquivoDemonstrativosPagamento]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspImportarArquivoDemonstrativosPagamento]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspImportarArquivoDemonstrativosPagamento]
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

DECLARE @GuidDemonstrativoPagamento     UNIQUEIDENTIFIER = NULL

DECLARE @QuantidadeRegistrosInseridos	INT = 0
DECLARE @QuantidadeRegistrosAtualizados	INT = 0
DECLARE @QuantidadeRegistrosInalterados	INT = 0
DECLARE @QuantidadeRegistrosRejeitados	INT = 0

-- Variáveis para controle da extração de linhas
DECLARE @Line							VARCHAR(MAX)
DECLARE @StartIndex						INT = 1
DECLARE @EndIndex						INT

DECLARE @Competencia					VARCHAR(10) = NULL

-------------------------------------------
-- Cria tabela temporária das MATRÍCULAS --
-------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculas] FROM dbo.[MATRICULAS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculas] ON [#TmpMatriculas] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial.
INSERT INTO [#TmpMatriculas]
     SELECT * FROM dbo.[MATRICULAS] WITH (NOLOCK)

--------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS DEMONSTRATIVOS PAGAMENTO --
--------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasDemonstrativosPagamento] FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasDemonstrativosPagamento] ON [#TmpMatriculasDemonstrativosPagamento] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial.
INSERT INTO [#TmpMatriculasDemonstrativosPagamento]
     SELECT * FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] WITH (NOLOCK)

----------------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS DEMONSTRATIVOS PAGAMENTO EVENTOS --
----------------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasDemonstrativosPagamentoEventos] FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasDemonstrativosPagamentoEventos] ON [#TmpMatriculasDemonstrativosPagamentoEventos] ([Guid])

------------------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS DEMONSTRATIVOS PAGAMENTO MENSAGENS --
------------------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasDemonstrativosPagamentoMensagens] FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MENSAGENS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasDemonstrativosPagamentoMensagens] ON [#TmpMatriculasDemonstrativosPagamentoMensagens] ([Guid])

----------------------------------------------------------------------------------
-- Cria tabela temporária das MATRÍCULAS DEMONSTRATIVOS PAGAMENTO TOTALIZADORES --
----------------------------------------------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculasDemonstrativosPagamentoTotalizadores] FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculasDemonstrativosPagamentoTotalizadores] ON [#TmpMatriculasDemonstrativosPagamentoTotalizadores] ([Guid])

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
		SET @GuidDemonstrativoPagamento = NULL

		--	Competência
		SET @Competencia = '01/' + LTRIM(RTRIM(SUBSTRING(@Line, 2, 7)))
		SET @Competencia = CONVERT(VARCHAR(8), CONVERT(DATE, @Competencia, 103), 112)	-- Converter para DATE e depois para VARCHAR(8) no formato yyyymmdd.
		SET @Competencia = REPLACE(@Competencia, '/', '')

		--	Razão Social
		DECLARE @RazaoSocial AS VARCHAR(40) = LTRIM(RTRIM(SUBSTRING(@Line, 32, 40)))
	END
	ELSE IF @Registro = '2'
	BEGIN
		--	Matrícula
		DECLARE @Matricula AS VARCHAR(20) = LTRIM(RTRIM(SUBSTRING(@Line, 2, 10)))

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
	 		SELECT TOP 1 @GuidDemonstrativoPagamento = T.[GUID]
			  FROM [#TmpMatriculasDemonstrativosPagamento] AS T
	         WHERE T.[GUIDMATRICULA] = @GuidMatricula
			   AND T.[COMPETENCIA] = @Competencia

			IF @GuidDemonstrativoPagamento IS NULL
			BEGIN
				SET @GuidDemonstrativoPagamento = NEWID()

				INSERT INTO [#TmpMatriculasDemonstrativosPagamento] ([GUID],
				                                                     [GUIDMATRICULA],
																	 [COMPETENCIA],
																	 [DATA_INCLUSAO],
																	 [DATA_ULTIMA_ALTERACAO])
												             VALUES (@GuidDemonstrativoPagamento,
															         @GuidMatricula,
																	 @Competencia,
																	 @DataAtual,
																	 @DataAtual)
			END
			ELSE
			BEGIN
				UPDATE [#TmpMatriculasDemonstrativosPagamento] 
				   SET [DATA_ULTIMA_ALTERACAO] = @DataAtual
				 WHERE [GUID] = @GuidDemonstrativoPagamento
			END
		END
		
		--	Nome
		DECLARE @Nome AS VARCHAR (40) = LTRIM(RTRIM(SUBSTRING(@Line, 12, 40)))

		--	Descrição do Cargo
		DECLARE @DescricaoCargo AS VARCHAR (30) = LTRIM(RTRIM(SUBSTRING(@Line, 52, 30)))
			
		--	Descrição do Setor
		DECLARE @DescricaoSetor AS VARCHAR (30) = LTRIM(RTRIM(SUBSTRING(@Line, 82, 30)))

		--	Número da CTPS
		DECLARE @NumeroCtps AS VARCHAR (7) = LTRIM(RTRIM(SUBSTRING(@Line, 114, 7)))

		--	Série da CTPS
		DECLARE @SerieCtps AS VARCHAR (7) = LTRIM(RTRIM(SUBSTRING(@Line, 122, 4)))

		--	UF da CTPS
		DECLARE @UfCtps AS VARCHAR (2) = LTRIM(RTRIM(SUBSTRING(@Line, 127, 2)))

		--	Data da Admissão
		DECLARE @DataAdmissao AS DATE = CONVERT(DATE, LTRIM(RTRIM(SUBSTRING(@Line, 132, 10))))

		--	IR
		DECLARE @FaixaIr AS VARCHAR(2) = LTRIM(RTRIM(SUBSTRING(@Line, 142, 2)))

		--	SF
		DECLARE @FaixaSf AS VARCHAR(2) = LTRIM(RTRIM(SUBSTRING(@Line, 144, 2)))
			
		--	Salário Nominal
		--DECLARE @SalarioNominal AS VARBINARY(MAX) = CONVERT(VARBINARY(MAX), LTRIM(RTRIM(SUBSTRING(@Line, 146, 10))))
		--DECLARE @SalarioNominal AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 146, 10)))

		--	Salário Nominal
		DECLARE @SalarioNominalTexto AS VARCHAR(13) = LTRIM(RTRIM(SUBSTRING(@Line, 146, 10)))
		SET @SalarioNominalTexto = REPLACE(@SalarioNominalTexto, '.', '')
		SET @SalarioNominalTexto = REPLACE(@SalarioNominalTexto, ',', '.')

		DECLARE @SalarioNominal AS VARBINARY(MAX) = CONVERT(VARBINARY(MAX), @SalarioNominalTexto)

		--	Banco
		DECLARE @Banco AS VARCHAR(3) = LTRIM(RTRIM(SUBSTRING(@Line, 156, 3)))

		--	Agência
		DECLARE @Agencia AS VARCHAR(5) = LTRIM(RTRIM(SUBSTRING(@Line, 159, 5)))

		--	Conta
		DECLARE @Conta AS VARCHAR(13) = LTRIM(RTRIM(SUBSTRING(@Line, 164, 13)))
	END
	ELSE IF @Registro = '3' AND NOT @GuidDemonstrativoPagamento IS NULL
	BEGIN
		DECLARE @GuidEvento       AS UNIQUEIDENTIFIER = NEWID()

		--	1. Código do Evento.
		DECLARE @IdEvento         AS INT = CONVERT(INT, LTRIM(RTRIM(SUBSTRING(@Line, 2, 5))))
		
		--	2. Descrição do Evento.
		DECLARE @DescricaoEvento  AS VARCHAR(75) = LTRIM(RTRIM(SUBSTRING(@Line, 7, 30)))

		--	3. Referência do Evento.
		DECLARE @ReferenciaEvento      AS DECIMAL(25,10) = NULL

		DECLARE @ReferenciaEventoTexto AS VARCHAR(6) = LTRIM(RTRIM(SUBSTRING(@Line, 37, 6)))

		IF @ReferenciaEventoTexto <> ''
		BEGIN
		    SET @ReferenciaEventoTexto = REPLACE(@ReferenciaEventoTexto, '.', '')
			SET @ReferenciaEventoTexto = REPLACE(@ReferenciaEventoTexto, ',', '.')

			SET @ReferenciaEvento = CONVERT(DECIMAL(25, 10), @ReferenciaEventoTexto)
		END

		--	4. Valor do Evento.
		DECLARE @ValorEventoTexto   AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 43, 10)))
		SET @ValorEventoTexto = REPLACE(@ValorEventoTexto, '.', '')
		SET @ValorEventoTexto = REPLACE(@ValorEventoTexto, ',', '.')

		DECLARE @ValorEventoDecimal AS DECIMAL(8, 2) = CONVERT(DECIMAL(8, 2), @ValorEventoTexto)

		DECLARE @ValorEvento        AS VARBINARY(MAX) = CONVERT(VARBINARY(MAX), CONVERT(VARCHAR(20), @ValorEventoDecimal))

		--	5. Tipo do Evento.
		DECLARE @TipoEvento       AS VARCHAR(1) = LTRIM(RTRIM(SUBSTRING(@Line, 53, 1)))

		--	Se não existir o registro no Cadastro do Evento, faz a inclusão.
		IF NOT EXISTS(SELECT TOP 1 ID
		                FROM EVENTOS
					   WHERE ID = @IdEvento)
		BEGIN
			INSERT INTO [dbo].[EVENTOS]
					   ([ID],
					    [DATA_ULTIMA_ALTERACAO],
						[DESCRICAO],
						[TIPO])
				 VALUES
					   (@IdEvento,
					    GETDATE(),
					    @DescricaoEvento,
						@TipoEvento)
		END

		INSERT INTO [#TmpMatriculasDemonstrativosPagamentoEventos]
				   ([GUID],
				    [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
					[IDEVENTO],
					[REFERENCIA],
					[VALOR])
			 VALUES
				   (@GuidEvento,
				    @GuidDemonstrativoPagamento,
					@IdEvento,
					@ReferenciaEvento,
					@ValorEvento)
	END
	ELSE IF @Registro = '4' AND NOT @GuidDemonstrativoPagamento IS NULL
	BEGIN
		--	1. Base INSS
		DECLARE @IdTotalizadorBaseInss AS INT = 6
		DECLARE @BaseInssTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 2, 10)))
		
		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorBaseInss, @BaseInssTexto

		--	2. Base IRRF
		DECLARE @IdTotalizadorBaseIrrf AS INT = 5
		DECLARE @BaseIrrfTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 12, 10)))
		
		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorBaseIrrf, @BaseIrrfTexto

		--	3. Base FGTS
		DECLARE @IdTotalizadorBaseFgts AS INT = 1
		DECLARE @BaseFgtsTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 22, 10)))

		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorBaseFgts, @BaseFgtsTexto

		--	4. Valor FGTS
		DECLARE @IdTotalizadorValorFgts AS INT = 2
		DECLARE @ValorFgtsTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 32, 10)))
		
		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorValorFgts, @ValorFgtsTexto

		--	5. Total de Vencimentos
		DECLARE @IdTotalizadorTotalVencimentos AS INT = 3
		DECLARE @TotalVencimentosTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 42, 10)))

		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorTotalVencimentos, @TotalVencimentosTexto

		--	6. Total de Descontos
		DECLARE @IdTotalizadorTotalDescontos AS INT = 4
		DECLARE @TotalDescontosTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 52, 10)))

		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorTotalDescontos, @TotalDescontosTexto

		--	7. Total Líquido
		DECLARE @IdTotalizadorTotalLiquido AS INT = 7
		DECLARE @TotalLiquidoTexto         AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 62, 10)))

		-- Chama o procedimento para processar e inserir o totalizador
		EXEC [dbo].[UspInserirTmpMatriculaDemonstrativoPagamentoTotalizador] @GuidDemonstrativoPagamento, @IdTotalizadorTotalLiquido, @TotalLiquidoTexto
	END
	ELSE IF @Registro = '5' AND NOT @GuidDemonstrativoPagamento IS NULL
	BEGIN
		DECLARE @Texto AS VARCHAR(160) = LTRIM(RTRIM(SUBSTRING(@Line, 42, 160)))

		IF @Texto <> ''
		BEGIN
			PRINT @Texto

			INSERT INTO [#TmpMatriculasDemonstrativosPagamentoMensagens]
					   ([GUID],
					    [GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
						[TEXTO])
				VALUES
					   (@GuidEvento,
						@GuidDemonstrativoPagamento,
						@Texto)
		END
	END
END

BEGIN TRANSACTION

--	Atualiza os DEMONSTRATIVOS DE PAGAMENTO que tiveram dados alterados.
    UPDATE DP
       SET DP.[DATA_ULTIMA_ALTERACAO] = T.[DATA_ULTIMA_ALTERACAO]
      FROM [#TmpMatriculasDemonstrativosPagamento] T
INNER JOIN [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DP WITH (NOLOCK)
        ON DP.[COMPETENCIA] = T.[COMPETENCIA]
	   AND DP.[GUIDMATRICULA] = T.[GUIDMATRICULA]
     WHERE DP.[DATA_ULTIMA_ALTERACAO] <> T.[DATA_ULTIMA_ALTERACAO]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Exclui os vínculos (DEMONSTRATIVOS DE PAGAMENTO x DEMONSTRATIVOS DE PAGAMENTO EVENTOS) de Eventos dos Demonstrativos de Pagamento que serão alterados.
    DELETE DPE
      FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS] DPE WITH (NOLOCK)
INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T
        ON DPE.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T.[GUID]
     WHERE T.[DATA_ULTIMA_ALTERACAO] = @DataAtual

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Exclui os vínculos (DEMONSTRATIVOS DE PAGAMENTO x DEMONSTRATIVOS DE PAGAMENTO MENSAGENS) de Mensagens dos Demonstrativos de Pagamento que serão alterados.
    DELETE DPM
      FROM [dbo].[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MENSAGENS] DPM WITH (NOLOCK)
INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T
        ON DPM.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T.[GUID]
     WHERE T.[DATA_ULTIMA_ALTERACAO] = @DataAtual

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Exclui os vínculos (DEMONSTRATIVOS DE PAGAMENTO x DEMONSTRATIVOS DE PAGAMENTO TOTALIZADORES) de Totalizadores dos Demonstrativos de Pagamento que serão alterados.
    DELETE DPT
      FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES] DPT WITH (NOLOCK)
INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T
        ON DPT.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T.[GUID]
     WHERE T.[DATA_ULTIMA_ALTERACAO] = @DataAtual

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os novos registros de MATRÍCULAS DE PAGAMENTO na tabela.
    INSERT INTO dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO]
         SELECT T.[GUID],
				T.[GUIDMATRICULA],
				T.[COMPETENCIA],
				T.[DATA_INCLUSAO],
				T.[DATA_INCLUSAO],
				NULL,
				NULL,
				NULL
           FROM [#TmpMatriculasDemonstrativosPagamento] T
LEFT OUTER JOIN dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DP WITH (NOLOCK)
             ON DP.[COMPETENCIA] = T.[COMPETENCIA]
	        AND DP.[GUIDMATRICULA] = T.[GUIDMATRICULA]
          WHERE DP.[GUID] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os registros de EVENTOS DO DEMONSTRATIVO DE PAGAMENTO na tabela.
INSERT INTO dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS]
     SELECT T1.[GUID],
	        T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
            T1.[IDEVENTO],
			T1.[REFERENCIA],
			T1.[VALOR]
       FROM [#TmpMatriculasDemonstrativosPagamentoEventos] T1
 INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T2
         ON T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T2.[GUID]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os registros de MENSAGENS DO DEMONSTRATIVO DE PAGAMENTO na tabela.
INSERT INTO dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_MENSAGENS]
     SELECT T1.[GUID],
	        T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
            T1.[TEXTO]
       FROM [#TmpMatriculasDemonstrativosPagamentoMensagens] T1
 INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T2
         ON T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T2.[GUID]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os registros de TOTALIZADORES DO DEMONSTRATIVO DE PAGAMENTO na tabela.
INSERT INTO dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES]
     SELECT T1.[GUID],
	        T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO],
            T1.[IDTOTALIZADOR],
			T1.[VALOR]
       FROM [#TmpMatriculasDemonstrativosPagamentoTotalizadores] T1
 INNER JOIN [#TmpMatriculasDemonstrativosPagamento] T2
         ON T1.[GUIDMATRICULA_DEMONSTRATIVO_PAGAMENTO] = T2.[GUID]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

COMMIT TRANSACTION

--	Contabiliza os registros atualizados.
SET @QuantidadeRegistrosAtualizados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DP
                                            WHERE DP.[DATA_INCLUSAO] <> DP.[DATA_ULTIMA_ALTERACAO]
											  AND DP.[DATA_ULTIMA_ALTERACAO] = @DataAtual)

SET @QuantidadeRegistrosInalterados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DP
                                            WHERE DP.[DATA_ULTIMA_ALTERACAO] < @DataAtual )

SET @QuantidadeRegistrosInseridos = (    SELECT COUNT(*)
                                           FROM dbo.[MATRICULAS_DEMONSTRATIVOS_PAGAMENTO] DP
                                          WHERE DP.[DATA_INCLUSAO] = DP.[DATA_ULTIMA_ALTERACAO]
									        AND DP.[DATA_INCLUSAO] = @DataAtual )

SELECT @DataAtual AS 'DATA_INICIO',
       GETDATE() AS 'DATA_FIM',
       @QuantidadeRegistrosAtualizados AS 'QUANTIDADE_REGISTROS_ATUALIZADOS',
       @QuantidadeRegistrosInalterados AS 'QUANTIDADE_REGISTROS_INALTERADOS',
	   @QuantidadeRegistrosInseridos   AS 'QUANTIDADE_REGISTROS_INSERIDOS',
	   @QuantidadeRegistrosRejeitados  AS 'QUANTIDADE_REGISTROS_REJEITADOS'

FINALIZA:

DROP TABLE [#TmpMatriculasDemonstrativosPagamentoTotalizadores]
DROP TABLE [#TmpMatriculasDemonstrativosPagamentoMensagens]
DROP TABLE [#TmpMatriculasDemonstrativosPagamentoEventos]
DROP TABLE [#TmpMatriculasDemonstrativosPagamento]
DROP TABLE [#TmpMatriculas]

GO