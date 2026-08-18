declare @Ano AS TINYINT = NULL
declare @Mes AS TINYINT = 8
declare @Dia AS TINYINT = 18

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

   SELECT * FROM #PalpiteConcurso
   where (@PrimeiraLinha IS NULL OR Linha >= @PrimeiraLinha)
   AND (@UltimaLinha IS NULL OR Linha <= @UltimaLinha)
   order by Linha

   DROP TABLE #PalpiteConcurso
