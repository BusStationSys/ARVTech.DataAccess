--EXEC [uspObterPessoasJuridicas]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterPessoasJuridicas]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterPessoasJuridicas]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

 CREATE PROCEDURE [dbo].[UspObterPessoasJuridicas]

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

    SELECT PJ.[GUID], PJ.[GUIDPESSOA], PJ.[CNPJ], PJ.[DATA_FUNDACAO], PJ.[LOGOTIPO], PJ.[RAZAO_SOCIAL], PJ.[IDUNIDADE_NEGOCIO],
           P.[GUID], P.[CEP], P.[COMPLEMENTO], P.[DATA_INCLUSAO], P.[DATA_ULTIMA_ALTERACAO], P.[EMAIL], P.[NUMERO], P.[TELEFONE], P.[CHAVE_EXPORTACAO_IMPORTACAO],
	       UN.[ID], UN.[DATA_INCLUSAO], UN.[DATA_ULTIMA_ALTERACAO], UN.[DESCRICAO]
      FROM [dbo].[PESSOAS_JURIDICAS] AS PJ WITH(NOLOCK)
INNER JOIN [dbo].[PESSOAS] as P WITH(NOLOCK)
        ON [PJ].[GUIDPESSOA] = [P].[GUID]
INNER JOIN [dbo].[UNIDADES_NEGOCIO] as UN WITH(NOLOCK)
        ON [PJ].[IDUNIDADE_NEGOCIO] = [UN].[ID]

GO