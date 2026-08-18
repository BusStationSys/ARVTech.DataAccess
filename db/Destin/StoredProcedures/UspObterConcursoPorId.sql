If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterConcursoPorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterConcursoPorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterConcursoPorId]
	@Id UniqueIdentifier

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

SELECT C.[Id],
       C.[IdModalidade],
       C.[Numero],
       C.[DataApuracao],
       NULLIF((SELECT CD.[Id],
		              CD.[IdConcurso],
			          CD.[Dezena]
                 FROM [dbo].[ConcursoDezena] AS CD WITH(NOLOCK)
                WHERE CD.IdConcurso = C.[Id]
                  FOR JSON PATH),
			  '[]') AS Dezenas
  FROM [dbo].[Concurso] AS C WITH(NOLOCK)
 WHERE [C].[Id] = @Id 

GO