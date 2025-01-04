--exec uspImportarArquivoMatriculas '07718633006896',
--'      15 JOSE VALOIR DA SILVEIRA             17/04/1951   93.265-140                              (51) 98357177               185.622.800-20 00/00/0000 08/07/2002                        1541  TAMANDARE            CASTRO ALVES                   Esteio             RS 000086591 645    RS 1004653588        07.718.633/0068-96 Encarregado Prevenção e Perdas CAD II - SEPARAÇÃO NOITE F6 6808       R 0033 01090 000001014479 8         3.223
--      38 SAMUEL MARCOS DORNELES              02/10/1974   93.222-090                              (51) 34525595               662.655.250-34 00/00/0000 04/03/2002 CASA                   213   VILA VARGAS          THALES ALVES DE SOUZA          Sapucaia do Sul    RS 000018031 33     RS 1058459429        07.718.633/0068-96 Conferente Operação Logística  DEC SUL - DIA F68           6858       R 0033 01090 000001014384 5         2.566
--   23018 JEFERSON FAGUNDES NASCIMENTO        30/03/1970   93.265-140 jef.guaiba@gmail.com         (51) 999598173              682.458.150-04 00/00/0000 04/02/2010                        201   JARDIM PLANALTO      SAO LEOPOLDO                   Esteio             RS 007355985 0010   RS 5050314243        07.718.633/0068-96 Gerente Operacional II         CAD II - ADMINISTRATIVO F68 6808       R 0033 01090 000001014329 2         8.513'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspImportarArquivoMatriculas]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	 DROP PROCEDURE [dbo].[UspImportarArquivoMatriculas]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspImportarArquivoMatriculas]
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

DECLARE @QuantidadeRegistrosInseridos	INT = 0
DECLARE @QuantidadeRegistrosAtualizados	INT = 0
DECLARE @QuantidadeRegistrosInalterados	INT = 0

-- Variáveis para controle da extração de linhas
DECLARE @Line							VARCHAR(MAX)
DECLARE @StartIndex						INT = 1
DECLARE @EndIndex						INT

-------------------------------------
-- Cria tabela temporária dos CEPS --
-------------------------------------
SELECT TOP 0 * INTO [#TmpCeps] FROM dbo.[CEPS] WITH (NOLOCK)

CREATE INDEX [#TmpCeps] ON [#TmpCeps] ([Cep])

----------------------------------------
-- Cria tabela temporária das PESSOAS --
----------------------------------------
SELECT TOP 0 * INTO [#TmpPessoas] FROM dbo.[PESSOAS] WITH (NOLOCK)

CREATE INDEX [#TmpPessoas] ON [#TmpPessoas] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial com a tabela MATRÍCULAS ao atualizar as Pessoas.
INSERT INTO [#TmpPessoas]
     SELECT * FROM dbo.[PESSOAS] WITH (NOLOCK)

------------------------------------------------
-- Cria tabela temporária das PESSOAS FÍSICAS --
------------------------------------------------
SELECT TOP 0 * INTO [#TmpPessoasFisicas] FROM dbo.[PESSOAS_FISICAS] WITH (NOLOCK)

CREATE INDEX [#TmpPessoasFisicas] ON [#TmpPessoasFisicas] ([Guid])

--	Foi necessário popular a Temporária para que não aconteça o erro de integridade referencial com a tabela MATRÍCULAS ao atualizar as Pessoas Físicas.
INSERT INTO [#TmpPessoasFisicas]
     SELECT * FROM dbo.[PESSOAS_FISICAS] WITH (NOLOCK)

--------------------------------------------
-- Cria tabela temporária das MATRÍCULAS  --
--------------------------------------------
SELECT TOP 0 * INTO [#TmpMatriculas] FROM dbo.[MATRICULAS] WITH (NOLOCK)

CREATE INDEX [#TmpMatriculas] ON [#TmpMatriculas] ([Guid])

-----------------------------------------
-- Cria tabela temporária dos USUÁRIOS --
-----------------------------------------
--SELECT TOP 0 * INTO [#TmpUsuarios] FROM dbo.[USUARIOS] WITH (NOLOCK)

--CREATE INDEX [#TmpUsuarios] ON [#TmpUsuarios] ([Guid])

-- Loop para processar cada linha da string
WHILE @StartIndex > 0
BEGIN
    -- Encontra o índice da próxima quebra de linha (CRLF)
    SET @EndIndex = CHARINDEX(CHAR(13) + CHAR(10), @Conteudo, @StartIndex);

    -- Se não encontrar uma quebra de linha, usa o restante da string
    IF @EndIndex = 0
    BEGIN
        SET @Line = SUBSTRING(@Conteudo, @StartIndex, LEN(@Conteudo) - @StartIndex + 1);

        SET @StartIndex = 0;  -- Finaliza o loop
    END
    ELSE
    BEGIN
        -- Extrai a linha com base nos índices encontrados
        SET @Line = SUBSTRING(@Conteudo, @StartIndex, @EndIndex - @StartIndex);

        -- Move o índice de início para o próximo caractere após a quebra de linha
        SET @StartIndex = @EndIndex + 2;
    END

	--	Matrícula
	DECLARE @Matricula AS VARCHAR(20) = LTRIM(RTRIM(SUBSTRING(@Line, 1, 8)))

	--	Data de Admissão
	DECLARE @DataAdmissao AS DATE = CONVERT(DATE, CAST(SUBSTRING(@Line, 153, 10) AS VARCHAR(10)))

	--	Data de Demissão
	DECLARE @DataDemissao AS DATE = NULL

	IF ISDATE(SUBSTRING(@Line, 142, 10)) = 1
		SET @DataDemissao = CONVERT(DATE, CAST(SUBSTRING(@Line, 142, 10) AS VARCHAR(10)))

	--	Descrição do Cargo
	DECLARE @DescricaoCargo AS VARCHAR(75) = LTRIM(RTRIM(SUBSTRING(@Line, 324, 30)))

	--	Descrição do Setor
    DECLARE @DescricaoSetor AS VARCHAR(75) = LTRIM(RTRIM(SUBSTRING(@Line, 355, 27)))

	--	Forma de Pagamento
	DECLARE @FormaPagamento AS CHAR(1) = LTRIM(RTRIM(SUBSTRING(@Line, 394, 1)))

	--	Banco
	DECLARE @Banco AS VARCHAR(6) = LTRIM(RTRIM(SUBSTRING(@Line, 396, 4)))

	--	Agência
	DECLARE @Agencia AS VARCHAR(9) = LTRIM(RTRIM(SUBSTRING(@Line, 401, 5)))

	--	Conta
	DECLARE @Conta AS VARCHAR(15) = LTRIM(RTRIM(SUBSTRING(@Line, 407, 12)))

	--	Dígito Verificador Conta
	DECLARE @DvConta AS CHAR(1) = LTRIM(RTRIM(SUBSTRING(@Line, 420, 1)))

	--	Salário Nominal
	DECLARE @SalarioNominal AS VARBINARY(MAX) = CONVERT(VARBINARY(MAX), LTRIM(RTRIM(SUBSTRING(@Line, 422, 13))))

	--	Conteúdo DEFAULT para Faixa IR, Faixa SF e Carga Horária.
	DECLARE @FaixaIr AS SMALLINT = 0
	DECLARE @FaixaSf AS SMALLINT = 0
	DECLARE @CargaHoraria AS DECIMAL(6, 2) = 220

	--	CPF
	DECLARE @Cpf AS VARCHAR(18) = SUBSTRING(@Line, 127, 14)
	SET @Cpf = REPLACE(@Cpf, '.', '')
	SET @Cpf = REPLACE(@Cpf, '-', '')

	-- Chave de Exportação e de Importação
	DECLARE @ChaveExportacaoImportacao AS VARCHAR(13) = CONCAT('PF', @Cpf)

	--	Nome
	DECLARE @Nome AS VARCHAR(100) = LTRIM(RTRIM(SUBSTRING(@Line, 10, 35)))

	--	RG
	DECLARE @Rg AS VARCHAR(20) = LTRIM(RTRIM(SUBSTRING(@Line, 287, 15)))

	--	Data Nascimento
	DECLARE @DataNascimento AS DATE = NULL

	IF ISDATE(SUBSTRING(@Line, 46, 10)) = 1
		SET @DataNascimento = CONVERT(DATE, CAST(SUBSTRING(@Line, 46, 10) AS VARCHAR(10)))

	--	CEP
	DECLARE @Cep AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 59, 10)))
	SET @Cep = REPLACE(@Cep, '.', '')
	SET @Cep = REPLACE(@Cep, '-', '')

	--	Logradouro
	DECLARE @Logradouro AS VARCHAR(100) = LTRIM(RTRIM(SUBSTRING(@Line, 214, 30)))

	--	Número
	DECLARE @Numero AS VARCHAR(10) = LTRIM(RTRIM(SUBSTRING(@Line, 184, 6)))

	--	Complemento
	DECLARE @Complemento AS VARCHAR(30) = LTRIM(RTRIM(SUBSTRING(@Line, 164, 23)))

	--	Bairro
	DECLARE @Bairro AS VARCHAR(100) = LTRIM(RTRIM(SUBSTRING(@Line, 193, 20)))

	--	Cidade
	DECLARE @Cidade AS VARCHAR(100) = LTRIM(RTRIM(SUBSTRING(@Line, 245, 18)))

	--	UF
	DECLARE @Uf AS VARCHAR(2) = SUBSTRING(@Line, 264, 2)

	--	Email
	DECLARE @Email AS VARCHAR(100) = LOWER(LTRIM(RTRIM(SUBSTRING(@Line, 70, 28))))

	--	Telefone
	DECLARE @Telefone AS VARCHAR(30) = LTRIM(RTRIM(SUBSTRING(@Line, 99, 27)))

	--	Número CTPS
	DECLARE @NumeroCtps AS VARCHAR(9) = LTRIM(RTRIM(SUBSTRING(@Line, 267, 9)))

	--	Série CTPS
	DECLARE @SerieCtps AS VARCHAR(5) = LTRIM(RTRIM(SUBSTRING(@Line, 277, 6)))

	--	UF CTPS
	DECLARE @UfCtps AS VARCHAR(2) = SUBSTRING(@Line, 284, 2)

	--	Montagem do Username que será utilizado no registro do Usuário.
	DECLARE @FirstName AS VARCHAR(100) = dbo.[UfnObterPrimeiroNome](@Nome)
	DECLARE @LastName AS VARCHAR(100) = dbo.[UfnObterUltimoNome](@Nome)

	DECLARE @Username AS VARCHAR(75) = LOWER(CONCAT(@FirstName, '.', @LastName))

    --	Insere o registro na tabela de CEP, se não existir.
	IF NOT EXISTS(SELECT TOP 1 1 FROM [#TmpCeps] WHERE [CEP] = @Cep)
	BEGIN
		INSERT INTO [#TmpCeps] ([CEP],
								[LOGRADOURO],
								[BAIRRO],
								[CIDADE],
								[UF],
								[DATA_INCLUSAO],
								[DATA_ULTIMA_ALTERACAO])
						VALUES (@Cep,
								@Logradouro,
								@Bairro,
								@Cidade,
								@Uf,
								@DataAtual,
								@DataAtual)
	END

	--	Insere o registro na tabela de PESSOA, se não existir.
	DECLARE @GuidPessoa AS UNIQUEIDENTIFIER

	SET @GuidPessoa = (SELECT TOP 1 [GUID]
	                     FROM [#TmpPessoas]
	                    WHERE [CHAVE_EXPORTACAO_IMPORTACAO] = @ChaveExportacaoImportacao)
	IF @GuidPessoa IS NULL
	BEGIN
		SET @GuidPessoa = NEWID()

		INSERT INTO [#TmpPessoas] ([GUID],
								   [CEP],
								   [COMPLEMENTO],
								   [DATA_INCLUSAO],
								   [DATA_ULTIMA_ALTERACAO],
								   [EMAIL],
								   [NUMERO],
								   [TELEFONE],
								   [CHAVE_EXPORTACAO_IMPORTACAO])
						   VALUES (@GuidPessoa,
						           @Cep,
							       @Complemento,
								   @DataAtual,
								   @DataAtual,
								   @Email,
								   @Numero,
								   @Telefone,
								   @ChaveExportacaoImportacao)
	END

    --	Insere o registro na tabela de PESSOAS FÍSICAS, se não existir.
	DECLARE @GuidColaborador AS UNIQUEIDENTIFIER

	SET @GuidColaborador = (SELECT TOP 1 [GUID]
	                              FROM [#TmpPessoasFisicas]
								 WHERE [CPF] = @Cpf)

	IF @GuidColaborador IS NULL
	BEGIN
		SET @GuidColaborador = NEWID()

		INSERT INTO [#TmpPessoasFisicas] ([GUID],
			                              [GUIDPESSOA],
				                          [CPF],
										  [RG],
										  [DATA_NASCIMENTO],
										  [NOME],
										  [NUMERO_CTPS],
										  [SERIE_CTPS],
										  [UF_CTPS])
                                  VALUES (@GuidColaborador,
								          @GuidPessoa,
										  @Cpf,
										  @Rg,
										  @DataNascimento,
										  @Nome,
										  @NumeroCtps,
										  @SerieCtps,
										  @UfCtps)
	END

	--	Insere o registro na tabela de MATRÍCULAS, se não existir.
	IF NOT EXISTS(SELECT TOP 1 [GUID] FROM [#TmpMatriculas] WHERE [MATRICULA] = @Matricula)
	BEGIN
		INSERT INTO [#TmpMatriculas] ([GUID],
									  [MATRICULA],
									  [DATA_ADMISSAO],
									  [DATA_DEMISSAO],
									  [DATA_INCLUSAO],
									  [DATA_ULTIMA_ALTERACAO],
									  [DESCRICAO_CARGO],
									  [DESCRICAO_SETOR],
									  [FAIXA_IR],
									  [FAIXA_SF],
									  [GUIDCOLABORADOR],
									  [GUIDEMPREGADOR],
									  [FORMA_PAGAMENTO],
									  [BANCO],
									  [AGENCIA],
									  [CONTA],
									  [DV_CONTA],
									  [CARGA_HORARIA],
									  [SALARIO_NOMINAL])
							  VALUES (NEWID(),
								      @Matricula,
								      @DataAdmissao,
									  @DataDemissao,
									  @DataAtual,
									  @DataAtual,
									  @DescricaoCargo,
									  @DescricaoSetor,
									  @FaixaIr,
									  @FaixaSf,
									  @GuidColaborador,
									  @GuidEmpregador,
									  @FormaPagamento,
									  @Banco,
									  @Agencia,
									  @Conta,
									  @DvConta,
									  @CargaHoraria,
									  @SalarioNominal)
	END
END

--	Atualiza os campos VARCHAR da tabela PESSOAS com NULL se estiverem não preenchidos.
UPDATE T
   SET T.[CEP] = NULL
  FROM [#TmpPessoas] T
 WHERE COALESCE(T.[CEP], '') = ''

UPDATE T
   SET T.[COMPLEMENTO] = NULL
  FROM [#TmpPessoas] T
 WHERE COALESCE(T.[COMPLEMENTO], '') = ''

UPDATE T
   SET T.[EMAIL] = NULL
  FROM [#TmpPessoas] T
 WHERE COALESCE(T.[EMAIL], '') = ''

UPDATE T
   SET T.[NUMERO] = NULL
  FROM [#TmpPessoas] T
 WHERE COALESCE(T.[NUMERO], '') = ''

UPDATE T
   SET T.[TELEFONE] = NULL
  FROM [#TmpPessoas] T
 WHERE COALESCE(T.[TELEFONE], '') = ''

--select * from [#TmpMatriculas]
--select * from [#TmpPessoasFisicas]
--select * from [#TmpPessoas]
--select * from [#TmpCeps]

--DROP TABLE [#TmpMatriculas]
--DROP TABLE [#TmpPessoasFisicas]
--DROP TABLE [#TmpPessoas]
--DROP TABLE [#TmpCeps]

--return

BEGIN TRANSACTION

--	Atualiza os CEPS que tiveram dados alterados.
    UPDATE C
       SET C.[LOGRADOURO] = T.[LOGRADOURO],
	       C.[BAIRRO] = T.[BAIRRO],
	       C.[CIDADE] = T.[CIDADE],
	       C.[UF] = T.[UF],
		   C.[DATA_ULTIMA_ALTERACAO] = @DataAtual
      FROM [#TmpCeps] T
INNER JOIN [CEPS] C
        ON C.[CEP] = T.[CEP]
	 WHERE C.[LOGRADOURO] <> T.[LOGRADOURO]
	    OR COALESCE(C.[BAIRRO], '') <> COALESCE(T.[BAIRRO], '')
	    OR C.[CIDADE] <> T.[CIDADE]
	    OR C.[UF] <> T.[UF]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui os novos CEPS na tabela.
    INSERT INTO dbo.[CEPS]
         SELECT T.[CEP],
                T.[LOGRADOURO],
			    T.[BAIRRO],
			    T.[CIDADE],
			    T.[UF],
			    T.[DATA_INCLUSAO],
			    T.[DATA_INCLUSAO]
           FROM [#TmpCeps] T
LEFT OUTER JOIN dbo.[CEPS] C WITH (NOLOCK)
             ON C.[CEP] = T.[CEP]
          WHERE C.[CEP] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

-- Atualiza a DATA_ULTIMA_ALTERACAO na tabela PESSOAS conforme alterações nos registros da tabela PESSOAS_FISICAS.
    UPDATE P
       SET P.[DATA_ULTIMA_ALTERACAO] = @DataAtual
      FROM dbo.[PESSOAS] P
INNER JOIN dbo.[PESSOAS_FISICAS] PF
        ON P.[GUID] = PF.[GUIDPESSOA]
INNER JOIN [#TmpPessoasFisicas] T
        ON PF.[CPF] = T.[CPF]
     WHERE COALESCE(PF.[RG], '') <> COALESCE(T.[RG], '')
	    OR COALESCE(PF.[DATA_NASCIMENTO], '') <> COALESCE(T.[DATA_NASCIMENTO], '')
	    OR PF.[NOME] <> T.[NOME]
		OR PF.[NUMERO_CTPS] <> T.[NUMERO_CTPS]
		OR PF.[SERIE_CTPS] <> T.[SERIE_CTPS]
		OR PF.[UF_CTPS] <> T.[UF_CTPS]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Atualiza as PESSOAS que tiveram dados alterados.
    UPDATE P
       SET P.[CEP] = T.[CEP],
           P.[NUMERO] = T.[NUMERO],
		   P.[COMPLEMENTO] = T.[COMPLEMENTO],
	       P.[EMAIL] = T.[EMAIL],
	       P.[TELEFONE] = T.[TELEFONE],
		   P.[DATA_ULTIMA_ALTERACAO] = @DataAtual
      FROM [#TmpPessoas] T
INNER JOIN dbo.[PESSOAS] P
		ON P.[CHAVE_EXPORTACAO_IMPORTACAO] = T.[CHAVE_EXPORTACAO_IMPORTACAO]
     WHERE COALESCE(P.[CEP], '') <> COALESCE(T.[CEP], '')
        OR COALESCE(P.[NUMERO], '') <> COALESCE(T.[NUMERO], '')
	    OR COALESCE(P.[COMPLEMENTO], '') <> COALESCE(T.[COMPLEMENTO], '')
	    OR COALESCE(P.[EMAIL], '') <> COALESCE(T.[EMAIL], '')
	    OR COALESCE(P.[TELEFONE], '') <> COALESCE(T.[TELEFONE], '')

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui as novas PESSOAS na tabela.
    INSERT INTO dbo.[PESSOAS]
         SELECT T.[GUID],
		        T.[CEP],
                T.[COMPLEMENTO],
			    T.[DATA_INCLUSAO],
			    T.[DATA_INCLUSAO],
			    T.[EMAIL],
				T.[NUMERO],
			    T.[TELEFONE],
				T.[CHAVE_EXPORTACAO_IMPORTACAO]
           FROM [#TmpPessoas] T
LEFT OUTER JOIN dbo.[PESSOAS] P WITH (NOLOCK)
             ON P.[CHAVE_EXPORTACAO_IMPORTACAO] = T.[CHAVE_EXPORTACAO_IMPORTACAO]
          WHERE P.[GUID] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Atualiza as PESSOAS FÍSICAS que tiveram dados alterados.
    UPDATE P
       SET P.[RG] = T.[RG],
	       P.[DATA_NASCIMENTO] = T.[DATA_NASCIMENTO],
	       P.[NOME] = T.[NOME],
		   P.[NUMERO_CTPS] = T.[NUMERO_CTPS],
		   P.[SERIE_CTPS] = T.[SERIE_CTPS],
		   P.[UF_CTPS] = T.[UF_CTPS]
      FROM [#TmpPessoasFisicas] T
INNER JOIN dbo.[PESSOAS_FISICAS] P
        ON P.[CPF] = T.[CPF]
     WHERE COALESCE(P.[RG], '') <> COALESCE(T.[RG], '')
	    OR COALESCE(P.[DATA_NASCIMENTO], '') <> COALESCE(T.[DATA_NASCIMENTO], '')
	    OR P.[NOME] <> T.[NOME]
		OR P.[NUMERO_CTPS] <> T.[NUMERO_CTPS]
		OR P.[SERIE_CTPS] <> T.[SERIE_CTPS]
		OR P.[UF_CTPS] <> T.[UF_CTPS]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui as novas PESSOAS FISICAS na tabela.
    INSERT INTO dbo.[PESSOAS_FISICAS]
         SELECT T.[GUID],
		        T.[GUIDPESSOA],
		        T.[CPF],
				T.[FOTO],
				T.[RG],
			    T.[DATA_NASCIMENTO],
				T.[NOME],
			    T.[NUMERO_CTPS],
				T.[SERIE_CTPS],
				T.[UF_CTPS]
           FROM [#TmpPessoasFisicas] T
LEFT OUTER JOIN dbo.[PESSOAS_FISICAS] P WITH (NOLOCK)
             ON P.[CPF] = T.[CPF]
          WHERE P.[GUID] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Atualiza as MATRÍCULAS que tiveram dados alterados.
    UPDATE M
       SET M.[DATA_ADMISSAO] = T.[DATA_ADMISSAO],
	       M.[DATA_DEMISSAO] = T.[DATA_DEMISSAO],
	       M.[DESCRICAO_CARGO] = T.[DESCRICAO_CARGO],
		   M.[DESCRICAO_SETOR] = T.[DESCRICAO_SETOR],
		   M.[FAIXA_IR] = T.[FAIXA_IR],
		   M.[FAIXA_SF] = T.[FAIXA_SF],
		   M.[GUIDCOLABORADOR] = T.[GUIDCOLABORADOR],
		   M.[GUIDEMPREGADOR] = T.[GUIDEMPREGADOR],
		   M.[FORMA_PAGAMENTO] = T.[FORMA_PAGAMENTO],
		   M.[BANCO] = T.[BANCO],
		   M.[AGENCIA] = T.[AGENCIA],
		   M.[CONTA] = T.[CONTA],
		   M.[DV_CONTA] = T.[DV_CONTA],
		   M.[CARGA_HORARIA] = T.[CARGA_HORARIA],
		   M.[SALARIO_NOMINAL] = T.[SALARIO_NOMINAL],
		   M.[DATA_ULTIMA_ALTERACAO] = @DataAtual
      FROM [#TmpMatriculas] T
INNER JOIN dbo.[MATRICULAS] M
        ON M.[MATRICULA] = T.[MATRICULA]
     WHERE M.[DATA_ADMISSAO] <> M.[DATA_ADMISSAO]
	    OR COALESCE(M.[DATA_DEMISSAO], '') <> COALESCE(T.[DATA_DEMISSAO], '')
	    OR M.[DESCRICAO_CARGO] <> T.[DESCRICAO_CARGO]
		OR M.[DESCRICAO_SETOR] <> T.[DESCRICAO_SETOR]
		OR M.[FAIXA_IR] <> T.[FAIXA_IR]
		OR M.[FAIXA_SF] <> T.[FAIXA_SF]
		OR M.[GUIDCOLABORADOR] <> T.[GUIDCOLABORADOR]
		OR M.[GUIDEMPREGADOR] <> T.[GUIDEMPREGADOR]
		OR M.[FORMA_PAGAMENTO] <> T.[FORMA_PAGAMENTO]
		OR M.[BANCO] <> T.[BANCO]
		OR M.[AGENCIA] <> T.[AGENCIA]
		OR M.[CONTA] <> T.[CONTA]
		OR M.[DV_CONTA] <> T.[DV_CONTA]
		OR M.[CARGA_HORARIA] <> T.[CARGA_HORARIA]
		OR M.[SALARIO_NOMINAL] <> T.[SALARIO_NOMINAL]

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

--	Inclui as novas MATRÍCULAS na tabela.
    INSERT INTO dbo.[MATRICULAS]
         SELECT T.[GUID],
		        T.[MATRICULA],
		        T.[DATA_ADMISSAO],
				T.[DATA_DEMISSAO],
				T.[DATA_INCLUSAO],
				T.[DATA_INCLUSAO],
				T.[DESCRICAO_CARGO],
				T.[DESCRICAO_SETOR],
				T.[FAIXA_IR],
				T.[FAIXA_SF],
				T.[GUIDCOLABORADOR],
				T.[GUIDEMPREGADOR],
				T.[FORMA_PAGAMENTO],
				T.[BANCO],
				T.[AGENCIA],
				T.[CONTA],
				T.[DV_CONTA],
				T.[CARGA_HORARIA],
				T.[SALARIO_NOMINAL]
           FROM [#TmpMatriculas] T
LEFT OUTER JOIN dbo.[MATRICULAS] M WITH (NOLOCK)
             ON M.[MATRICULA] = T.[MATRICULA]
          WHERE M.[GUID] IS NULL

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION

	GOTO FINALIZA
END

COMMIT TRANSACTION

--	Contabiliza os registros atualizados.
--SET @QuantidadeRegistrosAtualizados = (    SELECT COUNT(*)
--                                             FROM dbo.[PESSOAS_FISICAS] PF
--                                       INNER JOIN dbo.[PESSOAS] P
--									           ON PF.[GUIDPESSOA] = P.[GUID]
--                                            WHERE P.[DATA_INCLUSAO] <> P.[DATA_ULTIMA_ALTERACAO]
--											  AND P.[DATA_ULTIMA_ALTERACAO] = @DataAtual )

SET @QuantidadeRegistrosAtualizados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS] M
                                            WHERE M.[DATA_INCLUSAO] <> M.[DATA_ULTIMA_ALTERACAO]
											  AND M.[DATA_ULTIMA_ALTERACAO] = @DataAtual )

--	Contabiliza os registros que não sofreram nenhum tipo de modificação.
--SET @QuantidadeRegistrosInalterados = (    SELECT COUNT(*)
--                                             FROM dbo.[PESSOAS_FISICAS] PF
--                                       INNER JOIN dbo.[PESSOAS] P
--								               ON PF.[GUIDPESSOA] = P.[GUID]
--                                            WHERE P.[DATA_ULTIMA_ALTERACAO] < @DataAtual )

SET @QuantidadeRegistrosInalterados = (    SELECT COUNT(*)
                                             FROM dbo.[MATRICULAS] M
                                            WHERE M.[DATA_ULTIMA_ALTERACAO] < @DataAtual )

--	Contabiliza os registros inseridos.
--SET @QuantidadeRegistrosInseridos = (    SELECT COUNT(*)
--                                           FROM dbo.[PESSOAS_FISICAS] PF
--                                     INNER JOIN dbo.[PESSOAS] P
--								             ON PF.[GUIDPESSOA] = P.[GUID]
--                                          WHERE P.[DATA_INCLUSAO] = P.[DATA_ULTIMA_ALTERACAO]
--									        AND P.[DATA_INCLUSAO] = @DataAtual )

SET @QuantidadeRegistrosInseridos = (    SELECT COUNT(*)
                                           FROM dbo.[MATRICULAS] M
                                          WHERE M.[DATA_INCLUSAO] = M.[DATA_ULTIMA_ALTERACAO]
									        AND M.[DATA_INCLUSAO] = @DataAtual )

SELECT @DataAtual AS 'DATA_INICIO',
       GETDATE() AS 'DATA_FIM',
       @QuantidadeRegistrosAtualizados AS 'QUANTIDADE_REGISTROS_ATUALIZADOS',
       @QuantidadeRegistrosInalterados AS 'QUANTIDADE_REGISTROS_INALTERADOS',
	   @QuantidadeRegistrosInseridos   AS 'QUANTIDADE_REGISTROS_INSERIDOS'

FINALIZA:

DROP TABLE [#TmpMatriculas]

DROP TABLE [#TmpPessoasFisicas]

DROP TABLE [#TmpPessoas]

DROP TABLE [#TmpCeps]

GO

--DELETE FROM MATRICULAS
--DELETE FROM PESSOAS_FISICAS
--DELETE FROM PESSOAS WHERE CHAVE_EXPORTACAO_IMPORTACAO LIKE '%PF%'