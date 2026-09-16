select 
cj.id, 
cte.descricao as 'Rodada', 
cj.DataHora,
--CONVERT(VARCHAR(10), CAST(cj.DataHora AS DATE), 103) AS 'Data',
l.descricao as 'Local',
--tm.Nome as mandante,cj.placarmandante,cj.placarvisitante,tv.Nome as visitante,
concat(tm.Nome, ' ', cj.placarmandante, ' X ', cj.PlacarVisitante, ' ', tv.Nome) as 'Confronto'
from CarneJogo CJ
inner join local l
on cj.idlocal = l.id
inner join Competicaoetapa cte
on cj.IdCompeticaoetapa = cte.id
inner join CompeticaoTime ctm
on cj.IdCompeticaoTimeMandante = ctm.Id
inner join Time tm on ctm.IdTime = tm.Id
inner join CompeticaoTime ctv
on cj.IdCompeticaoTimeVisitante = ctv.Id
inner join Time tv on ctv.IdTime = tv.Id
where cte.idcompeticao = '1A37501E-9AF5-41D5-9F49-59173954D64D'
order by cte.Ordem, DataHora

--REAL MADRID - A70F8F26-A847-4C2E-9F60-C4C943585252
--MADUREIRA - BA5602F2-4F1A-4D4F-8C5E-A4C1CD66304E
--RIVER PLATE - 3E1F672B-82BD-4DD5-ACE1-C87EDCD351AD
--SANTA CRUZ - AA83A63D-C0A1-4743-BA07-F0B109DD9691
--ORIENTE - 0EF940DF-C0F3-4ECA-9917-4791848BFFEB
--JUVENTUS - CDD76DD5-EED9-45F1-A887-3D58B014475D

--select * from CompeticaoTime
--select * from time

--update CarneJogo
--set DataHora='2026-09-12 17:15:00.0000001 -03:00'
----idlocal = 'B51E1C8C-9CBE-4A13-B110-1AFDACD95AD6'
--where id='29CC0984-00FA-4DB3-A237-B1670B1D2676'
 

--update CarneJogo
--set idlocal = '72340412-F246-4FB7-AB79-DCE36EA8D2B5'
--where id='4F47CED5-42A5-4993-9FD6-8A0022A707AE'

--select * from Local

--select * from CompeticaoEtapa