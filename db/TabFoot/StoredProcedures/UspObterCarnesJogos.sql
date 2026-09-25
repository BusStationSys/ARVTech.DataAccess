If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterCarnesJogos]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterCarnesJogos]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[UspObterCarnesJogos]

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 SELECT CJ.[Id],
        CJ.[DataHora],
		CJ.[IdLocal],
		CJ.[IdSituacao],
		CJ.[IdCompeticaoEtapa],
		CJ.[IdCompeticaoTimeMandante],
		CJ.[PlacarMandante],
		CJ.[IdCompeticaoTimeVisitante],
		CJ.[PlacarVisitante],
		CJ.[DataHoraInclusao],
		CJ.[DataHoraUltimaAlteracao]
   FROM [dbo].[CarneJogo] AS CJ WITH(NOLOCK)
GO