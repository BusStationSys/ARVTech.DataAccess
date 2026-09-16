--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarMatricula]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarMatricula]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarMatricula]
	@Guid AS UNIQUEIDENTIFIER,
	@Matricula AS VARCHAR(20),
	@DataAdmissao AS DATE,
	@DataDemissao AS DATE,
	@DescricaoCargo AS VARCHAR(75),
	@DescricaoSetor AS VARCHAR(75),
	@GuidColaborador AS UNIQUEIDENTIFIER,
	@GuidEmpregador AS UNIQUEIDENTIFIER,
	@FormaPagamento AS CHAR(1),
	@Banco AS VARCHAR(6),
	@Agencia AS VARCHAR(9),
	@Conta AS VARCHAR(15),
	@DvConta AS CHAR(1),
	@CargaHoraria AS DECIMAL(6, 2),
	@SalarioNominal AS VARBINARY(MAX)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 UPDATE M
    SET M.[MATRICULA] = @Matricula,
	    M.[DATA_ADMISSAO] = @DataAdmissao,
		M.[DATA_DEMISSAO] = @DataDemissao,
		M.[DESCRICAO_CARGO] = @DescricaoCargo,
		M.[DESCRICAO_SETOR] = @DescricaoSetor,
		M.[GUIDCOLABORADOR] = @GuidColaborador,
		M.[GUIDEMPREGADOR] = @GuidEmpregador,
		M.[BANCO] = @Banco,
		M.[AGENCIA] = @Agencia,
	    M.[CONTA] = @Conta,
		M.[DV_CONTA] = @DvConta,
		M.[FORMA_PAGAMENTO] = @FormaPagamento,
		M.[SALARIO_NOMINAL] = @SalarioNominal
   FROM [dbo].[MATRICULAS] AS M
  WHERE M.[GUID] = @Guid

GO