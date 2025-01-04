--EXEC [uspObterUsuarioPorCpfEmailUsername] '07718633006977'

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterUsuarioPorCpfEmailUsername]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterUsuarioPorCpfEmailUsername]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterUsuarioPorCpfEmailUsername]
	@Filtro VARCHAR(75)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

         SELECT U.[GUID], U.[GUIDCOLABORADOR], U.[IDPERFIL_USUARIO], U.[DATA_INCLUSAO], U.[DATA_ULTIMA_ALTERACAO], U.[EMAIL], U.[USERNAME], U.[PASSWORD_HASH], U.[SALT], U.[IDASPNETUSER], U.[DATA_PRIMEIRO_ACESSO],
		        PF.[GUID], PF.[GUIDPESSOA], PF.[CPF], PF.[FOTO], PF.[RG], PF.[DATA_NASCIMENTO], PF.[NOME], PF.[NUMERO_CTPS], PF.[SERIE_CTPS], PF.[UF_CTPS],
			    P.[GUID], P.[CEP], P.[COMPLEMENTO], P.[DATA_INCLUSAO], P.[DATA_ULTIMA_ALTERACAO], P.[EMAIL], P.[NUMERO], P.[TELEFONE], P.[CHAVE_EXPORTACAO_IMPORTACAO]
	       FROM [dbo].[USUARIOS] AS U WITH(NOLOCK)                       
LEFT OUTER JOIN [dbo].[PESSOAS_FISICAS] as PF WITH(NOLOCK)
			 ON [U].[GUIDCOLABORADOR] = [PF].[GUID]
LEFT OUTER JOIN [dbo].[PESSOAS] as [P] WITH(NOLOCK)
		     ON [PF].[GUIDPESSOA] = [P].[GUID]
		  WHERE ( LOWER([PF].[CPF]) = @Filtro OR LOWER([P].[Email]) = @Filtro OR LOWER([U].[UserName]) = @Filtro )

GO