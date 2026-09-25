use TabFoot

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspVisualizarPartidas]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspVisualizarPartidas]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

--EXEC [dbo].[UspVisualizarPartidas] '1A37501E-9AF5-41D5-9F49-59173954D64D', '8BAF60F1-744E-4933-B8B9-7C7433016F6D'

CREATE PROCEDURE [dbo].[UspVisualizarPartidas]
    @IdCompeticao AS UNIQUEIDENTIFIER,
	@IdEtapa AS UNIQUEIDENTIFIER = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Ordem AS SMALLINT = NULL

IF NOT @IdEtapa IS NULL
BEGIN
	SELECT TOP 1 @Ordem = Ordem
	  FROM CompeticaoEtapa CTE
	 WHERE CTE.IdCompeticao = @IdCompeticao
	   AND CTE.Id = @IdEtapa
END

CREATE TABLE #Partidas
(
	IdCarneJogo UNIQUEIDENTIFIER NOT NULL,
	IdEtapa UNIQUEIDENTIFIER NOT NULL,
	OrdemEtapa SMALLINT NOT NULL,
	DescricaoEtapa VARCHAR(75) NOT NULL,
	DataHora DATETIMEOFFSET NOT NULL,
	DescricaoLocal VARCHAR(75) NOT NULL,
	Confronto VARCHAR(200) NOT NULL,
)

TRUNCATE TABLE #Partidas

INSERT INTO #Partidas
     select cj.id,
            cte.Id,
	        cte.Ordem,
	        cte.descricao, 
	        cj.DataHora,
	        --CONVERT(VARCHAR(10), CAST(cj.DataHora AS DATE), 103) AS 'Data',
	        l.descricao,
	        --tm.Nome as mandante,cj.placarmandante,cj.placarvisitante,tv.Nome as visitante,
	        concat(tm.Nome, ' ', cj.placarmandante, ' X ', cj.PlacarVisitante, ' ', tv.Nome)
       from CarneJogo CJ
 inner join local l
         on cj.idlocal = l.id
 inner join Competicaoetapa cte
         on cj.IdCompeticaoetapa = cte.id
 inner join CompeticaoTime ctm
         on cj.IdCompeticaoTimeMandante = ctm.Id
 inner join Time tm
         on ctm.IdTime = tm.Id
  inner join CompeticaoTime ctv
          on cj.IdCompeticaoTimeVisitante = ctv.Id
  inner join Time tv on ctv.IdTime = tv.Id
       where cte.idcompeticao = @IdCompeticao
         AND (@Ordem IS NULL OR CTE.Ordem <= @Ordem)
    order by cte.Ordem,
	         DataHora

   select DescricaoEtapa AS 'Rodada',
          CONVERT(VARCHAR(10), CAST(DataHora AS DATE), 103) AS 'Data',
		  DescricaoLocal AS 'Local',
		  Confronto
     from #Partidas
 order by OrdemEtapa,
          DataHora

DROP TABLE #Partidas