If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterTimes]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterTimes]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[UspObterTimes]

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 SELECT T.[Id],
        T.[Nome],
		T.[IdTecnico],
		T.[DataHoraInclusao],
		T.[DataHoraUltimaAlteracao]
   FROM [dbo].[Time] AS T WITH(NOLOCK)

GO