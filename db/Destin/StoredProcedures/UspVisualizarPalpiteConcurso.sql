declare @Ano AS TINYINT = NULL
declare @Mes AS TINYINT = 8
declare @Dia AS TINYINT = 19

declare @PrimeiraLinha as tinyint = 6
declare @UltimaLinha as tinyint = 20

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
   DELETE FROM #PalpiteConcurso
   WHERE (@PrimeiraLinha IS NOT NULL AND Linha < @PrimeiraLinha)

   DELETE FROM #PalpiteConcurso
   WHERE (@UltimaLinha IS NOT NULL AND Linha > @UltimaLinha)

   --	Mostra os dados que sobraram.
   SELECT * FROM #PalpiteConcurso
   --order by DEZENA, Linha
   order by Linha

   DROP TABLE #PalpiteConcurso