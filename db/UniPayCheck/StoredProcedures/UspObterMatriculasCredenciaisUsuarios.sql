If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterMatriculasCredenciaisUsuarios]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	 DROP PROCEDURE [dbo].[UspObterMatriculasCredenciaisUsuarios]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspObterMatriculasCredenciaisUsuarios]
	@GuidColaborador UNIQUEIDENTIFIER = NULL,
	@DataInclusao DATETIME2 = NULL,
	@DataUltimaAlteracao DATETIME2 = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

IF @GuidColaborador IS NULL AND @DataInclusao IS NULL AND @DataUltimaAlteracao IS NULL
BEGIN
    RAISERROR ('Nenhum parâmetro foi especificado. É necessário informar pelo menos um.', 16, 1)
    RETURN
END

     SELECT M.[GUID], M.[MATRICULA], M.[DATA_ADMISSAO], M.[DATA_DEMISSAO], M.[DATA_INCLUSAO], M.[DATA_ULTIMA_ALTERACAO], M.[DESCRICAO_CARGO],
            M.[DESCRICAO_SETOR], M.[FAIXA_IR], M.[FAIXA_SF], M.[GUIDCOLABORADOR], M.[GUIDEMPREGADOR], M.[FORMA_PAGAMENTO], M.[BANCO],
            M.[AGENCIA], M.[CONTA], M.[DV_CONTA], M.[CARGA_HORARIA], M.[SALARIO_NOMINAL],
		    PF.[GUID], PF.[GUIDPESSOA], PF.[CPF], PF.[RG], PF.[DATA_NASCIMENTO], PF.[NOME], PF.[NUMERO_CTPS], PF.[SERIE_CTPS], PF.[UF_CTPS],
	        PF.[GUID], PJ.[GUIDPESSOA], PJ.[CNPJ], PJ.[DATA_FUNDACAO], PJ.[RAZAO_SOCIAL], PJ.[IDUNIDADE_NEGOCIO]
       FROM [dbo].[MATRICULAS] AS M WITH(NOLOCK)
 INNER JOIN [dbo].[PESSOAS_FISICAS] AS PF WITH(NOLOCK)
         ON M.[GUIDCOLABORADOR] = PF.[GUID]
 INNER JOIN [dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
         ON M.[GUIDEMPREGADOR] = PJ.[GUID]
      WHERE (@GuidColaborador IS NULL OR PF.[GUID] = @GuidColaborador)
       --AND (@DataInclusao IS NULL OR M.[DATA_INCLUSAO] >= @DataInclusao)
       --AND (@DataUltimaAlteracao IS NULL OR M.[DATA_ULTIMA_ALTERACAO] >= @DataUltimaAlteracao)
   --   AND (@DataInclusao IS NULL OR M.[DATA_INCLUSAO] >= @DataInclusao AND M.[DATA_INCLUSAO] < DATEADD(SECOND, 1, @DataInclusao))
	  --AND (@DataUltimaAlteracao IS NULL OR M.[DATA_ULTIMA_ALTERACAO] >= @DataUltimaAlteracao AND M.[DATA_ULTIMA_ALTERACAO] < DATEADD(SECOND, 1, @DataUltimaAlteracao))
 	    AND (@DataInclusao IS NULL OR M.[DATA_INCLUSAO] = @DataInclusao)
	    AND (@DataUltimaAlteracao IS NULL OR M.[DATA_ULTIMA_ALTERACAO] = @DataUltimaAlteracao)