If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterConcursoPorIdModalidadeENumeroEDataApuracao]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterConcursoPorIdModalidadeENumeroEDataApuracao]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterConcursoPorIdModalidadeENumeroEDataApuracao]
	@IdModalidade INT,
	@Numero INT,
	@DataApuracao DATE

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

SELECT C.[Id],
       C.[IdModalidade],
       C.[Numero],
       C.[DataApuracao],
	   C.[DataHoraInclusao],
	   C.[DataHoraUltimaAlteracao],
       NULLIF((SELECT CD.[Id],
		              CD.[IdConcurso],
			          CD.[Dezena]
                 FROM [dbo].[ConcursoDezena] AS CD WITH(NOLOCK)
                WHERE CD.IdConcurso = C.[Id]
                  FOR JSON PATH),
			  '[]') AS Dezenas
  FROM [dbo].[Concurso] AS C WITH(NOLOCK)
 WHERE [C].[IdModalidade] = @IdModalidade
   AND [C].[Numero] = @Numero
   AND [C].[DataApuracao] = @DataApuracao

GO