use destin

--EXEC [dbo].[UspVisualizarPalpiteConcurso] NULL, 9, 1

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspVisualizarPalpiteConcurso]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspVisualizarPalpiteConcurso]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspVisualizarPalpiteConcurso]
	@Ano AS TINYINT = NULL,
	@Mes AS TINYINT = NULL,
	@Dia AS TINYINT = NULL,
	@PrimeiraLinha AS TINYINT = NULL,
	@UltimaLinha AS TINYINT = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

create table #PalpiteConcurso
(
	Dezena TINYINT NOT NULL,
	Quantidade INT NOT NULL,
	Linha TINYINT NOT NULL,
)

INSERT INTO #PalpiteConcurso (Dezena,
                              Quantidade,
							  Linha)
     select dezena,
            COUNT(dezena),
			ROW_NUMBER() OVER (ORDER BY COUNT(dezena) DESC, dezena ASC)
       from ConcursoDezena CD
 inner join Concurso c
         on cd.IdConcurso=c.Id
      where c.IdModalidade = 5
        and (@Ano IS NULL OR year(c.DataApuracao) = @Ano)
        and (@Mes IS NULL OR Month(c.DataApuracao) = @mes)
        and (@Dia IS NULL OR day(c.DataApuracao) = @dia)
   group by dezena

   --	Saneamento dos dados através de um filtro por Delete.
 DELETE
   FROM #PalpiteConcurso
  WHERE (@PrimeiraLinha IS NOT NULL AND Linha < @PrimeiraLinha)

 DELETE
   FROM #PalpiteConcurso
  WHERE (@UltimaLinha IS NOT NULL AND Linha > @UltimaLinha)

   --	Mostra os dados que sobraram.
   SELECT *
     FROM #PalpiteConcurso
   --order by DEZENA, Linha
 ORDER BY Linha

  DROP TABLE #PalpiteConcurso