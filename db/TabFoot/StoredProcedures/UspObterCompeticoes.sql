If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterCompeticoes]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterCompeticoes]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterCompeticoes]

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 SELECT C.[Id],
        C.[Descricao],
		C.[DataInicio],
		C.[DataTermino],
		C.[PontoVitoria],
		C.[PontoEmpate],
		C.[DataHoraInclusao],
		C.[DataHoraUltimaAlteracao]
   FROM [dbo].[Competicao] AS C WITH(NOLOCK)

GO