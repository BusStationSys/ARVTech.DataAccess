 DELETE FROM USUARIOS
 DELETE FROM PERFIS_USUARIOS

 DELETE FROM MATRICULAS_ESPELHOS_PONTO_MARCACOES
 DELETE FROM MATRICULAS_ESPELHOS_PONTO_CALCULOS
 DELETE FROM MATRICULAS_ESPELHOS_PONTO

 DELETE FROM MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS
 DELETE FROM MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES
 DELETE FROM MATRICULAS_DEMONSTRATIVOS_PAGAMENTO

 DELETE FROM MATRICULAS

 DELETE FROM PESSOAS_FISICAS

 DELETE FROM PESSOAS_JURIDICAS
  WHERE GUID <> 'DDBD63D4-95F8-4F87-A3D8-3F98E2338EDC'
    AND GUID <> '5530BAD4-826C-4CBF-B817-70A87F12AF66'
	AND GUID <> 'C80F71E2-521C-427B-B9E5-8DEBDE41CC16'
	AND GUID <> 'CF286BC1-7E2E-4C04-B22C-A907E5FACE90'
	AND GUID <> '78C30594-0091-48BC-A3D9-B6CE5890D309'
	AND GUID <> 'D7E189DC-CFD8-4504-A829-BBE433BA455F'
	AND GUID <> '6E600C2B-BB2A-41B8-BA2F-F91A8874A450'

 DELETE FROM PESSOAS
  WHERE GUID <> '29063E26-5486-4586-AC77-99B859FEA875'
    AND GUID <> '7E312DF9-0955-439E-856B-3F40219A934A'
	AND GUID <> '64F29925-2751-4E72-B4E3-6CC2A60B04AC'
	AND GUID <> '9902F285-8880-4383-B216-36C68B32E3AE'
    AND GUID <> 'F74849FB-B5FA-489E-A5BE-AEB3F3154A7C'
	AND GUID <> '1576FD78-E4EB-42C3-A2D9-EB6974B66D6B'
	AND GUID <> 'F4659464-9C8C-40E7-9B39-4BA5899888B5'