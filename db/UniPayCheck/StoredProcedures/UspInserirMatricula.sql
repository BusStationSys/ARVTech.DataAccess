--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirMatricula]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirMatricula]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirMatricula]
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
	@CargaHoraria AS DECIMAL(6, 2)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @NewGuidMatricula UniqueIdentifier = NEWID()

INSERT INTO [dbo].[MATRICULAS] ([GUID],
                                [MATRICULA],
								[DATA_ADMISSAO],
								[DATA_DEMISSAO],
								[DESCRICAO_CARGO],
								[DESCRICAO_SETOR],
								[GUIDCOLABORADOR],
								[GUIDEMPREGADOR],
								[FORMA_PAGAMENTO],
								[BANCO],
								[AGENCIA],
								[CONTA],
								[DV_CONTA],
								[CARGA_HORARIA])
                        VALUES (@NewGuidMatricula,
						        @Matricula,
								@DataAdmissao,
								@DataDemissao,
								@DescricaoCargo,
								@DescricaoSetor,
								@GuidColaborador,
								@GuidEmpregador,
								@FormaPagamento,
								@Banco,
								@Agencia,
								@Conta,
								@DvConta,
								@CargaHoraria)

     SELECT @NewGuidMatricula

GO