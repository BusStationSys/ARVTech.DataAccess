use TabFoot

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspVisualizarClassificacaoGeral]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspVisualizarClassificacaoGeral]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

--EXEC [dbo].[UspVisualizarClassificacaoGeral] '1A37501E-9AF5-41D5-9F49-59173954D64D', '8BAF60F1-744E-4933-B8B9-7C7433016F6D'

CREATE PROCEDURE [dbo].[UspVisualizarClassificacaoGeral]
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

;WITH Jogos AS
    (
        SELECT
            IdCompeticaoTimeMandante AS IdTime,
			tm.Nome AS NomeTime,
            CAST(PlacarMandante AS INT) AS GolsPro,
            CAST(PlacarVisitante AS INT) AS GolsContra,
            CASE
                WHEN PlacarMandante > PlacarVisitante THEN c.PontoVitoria
                WHEN PlacarMandante = PlacarVisitante THEN c.PontoEmpate
                ELSE 0
            END AS Pontos,
            CASE
                WHEN PlacarMandante > PlacarVisitante THEN 1
                ELSE 0
            END AS Vitoria,
            CASE
                WHEN PlacarMandante = PlacarVisitante THEN 1
                ELSE 0
            END AS Empate,
            CASE
                WHEN PlacarMandante < PlacarVisitante THEN 1
                ELSE 0
            END AS Derrota
        FROM dbo.CarneJogo CJ
		inner join Competicaoetapa cte
		on cj.IdCompeticaoetapa = cte.id
		inner join Competicao c
		on cte.IdCompeticao = c.id
 inner join CompeticaoTime ctm
         on cj.IdCompeticaoTimeMandante = ctm.Id
 inner join Time tm
         on ctm.IdTime = tm.Id
where cte.idcompeticao = @IdCompeticao
AND (@Ordem IS NULL OR CTE.Ordem <= @Ordem)


        UNION ALL

        SELECT
            IdCompeticaoTimeVisitante AS IdTime,
			tm.Nome AS NomeTime,
            CAST(PlacarVisitante AS INT) AS GolsPro,
            CAST(PlacarMandante AS INT) AS GolsContra,
            CASE
                WHEN PlacarVisitante > PlacarMandante THEN c.PontoVitoria
                WHEN PlacarVisitante = PlacarMandante THEN c.PontoEmpate
                ELSE 0
            END AS Pontos,
            CASE
                WHEN PlacarVisitante > PlacarMandante THEN 1
                ELSE 0
            END AS Vitoria,
            CASE
                WHEN PlacarVisitante = PlacarMandante THEN 1
                ELSE 0
            END AS Empate,
            CASE
                WHEN PlacarVisitante < PlacarMandante THEN 1
                ELSE 0
            END AS Derrota
        FROM dbo.CarneJogo CJ
		inner join Competicaoetapa cte
		on cj.IdCompeticaoetapa = cte.id
		inner join Competicao c
		on cte.IdCompeticao = c.id
 inner join CompeticaoTime ctm
         on cj.IdCompeticaoTimeVisitante = ctm.Id
 inner join Time tm
         on ctm.IdTime = tm.Id
where cte.idcompeticao = @IdCompeticao
AND (@Ordem IS NULL OR CTE.Ordem <= @Ordem)
    ),
    ClassificacaoGeral AS
    (
        SELECT
            IdTime,
			NomeTime,
            SUM(Pontos) AS P,
            COUNT(*) AS PJ,
            SUM(Vitoria) AS V,
            SUM(Empate) AS E,
            SUM(Derrota) AS D,
            SUM(GolsPro) AS GP,
            SUM(GolsContra) AS GC,
            SUM(GolsPro - GolsContra) AS SG
        FROM Jogos
        GROUP BY IdTime, NomeTime
    )
    SELECT
        ROW_NUMBER() OVER
        (
            ORDER BY
                P DESC,
                V DESC,
                SG DESC,
                GP DESC,
                IdTime
        ) AS Posicao,
		NomeTime,
        P,
        PJ,
        V,
        E,
        D,
        GP,
        GC,
        SG
    FROM ClassificacaoGeral
    ORDER BY
        Posicao
