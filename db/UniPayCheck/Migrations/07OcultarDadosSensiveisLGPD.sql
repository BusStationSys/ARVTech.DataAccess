 UPDATE MATRICULAS
    SET SALARIO_NOMINAL=0

UPDATE MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_EVENTOS
   SET VALOR=0

UPDATE MATRICULAS_DEMONSTRATIVOS_PAGAMENTO_TOTALIZADORES
   SET VALOR=0

   SELECT * FROM PESSOAS_FISICAS

   BEGIN TRAN
   UPDATE PESSOAS_FISICAS SET NOME = CONVERT(VARCHAR(100), NEWID())

   UPDATE PESSOAS_FISICAS SET RG = SUBSTRING(
		CONVERT(
			VARCHAR(20), 
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CONCAT(NEWID(),NEWID()), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
		1,
		20)

   UPDATE PESSOAS_FISICAS SET CPF = SUBSTRING(
		CONVERT(
			CHAR(11), 
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CONCAT(NEWID(),NEWID()), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
		1,
		11)

   UPDATE PESSOAS_FISICAS SET NUMERO_CTPS = SUBSTRING(
		CONVERT(
			VARCHAR(9), 
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(NEWID(), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
		1,
		9)

   UPDATE PESSOAS_FISICAS SET SERIE_CTPS = SUBSTRING(
		CONVERT(
			VARCHAR(4), 
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(NEWID(), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
		1,
		4)
	COMMIT TRAN

   SELECT * FROM PESSOAS

   UPDATE PESSOAS SET EMAIL='carneiro.denis@hotmail.com'
    WHERE GUID <> 'D9F3A622-7CCB-4EAE-98EC-2B40A0721F4E'
      AND GUID <> '566A08BA-8226-4B03-BE5C-F24B6D618E1E'
	  AND GUID <> 'C0C5232D-8139-44AC-939B-D221D8444612'
	  AND GUID <> '222C04A7-23D7-4F7D-BDF4-83797766E1F6'
	  AND GUID <> 'C3398B3B-D331-4A5A-9E58-067F51280B8C'
	  AND GUID <> 'C9F4A972-D2D5-4C4E-99E0-2347F079C2CD'
      AND GUID <> 'FBD88E7B-B9D1-40FA-92DC-64728BECCE2C'
      AND GUID <> 'A2E106D9-C1B9-4F3E-8236-6AB48F27F040'
	  AND GUID <> 'C0C5232D-8139-44AC-939B-D221D8444612'

   UPDATE PESSOAS 
      SET TELEFONE = SUBSTRING(
		CONVERT(
			VARCHAR(4), 
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CONCAT(NEWID(),NEWID()), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
		1,
		30),
		  ENDERECO = NEWID()
    WHERE GUID <> 'D9F3A622-7CCB-4EAE-98EC-2B40A0721F4E'
      AND GUID <> '566A08BA-8226-4B03-BE5C-F24B6D618E1E'
	  AND GUID <> 'C0C5232D-8139-44AC-939B-D221D8444612'
	  AND GUID <> '222C04A7-23D7-4F7D-BDF4-83797766E1F6'
	  AND GUID <> 'C3398B3B-D331-4A5A-9E58-067F51280B8C'
	  AND GUID <> 'C9F4A972-D2D5-4C4E-99E0-2347F079C2CD'
      AND GUID <> 'FBD88E7B-B9D1-40FA-92DC-64728BECCE2C'
      AND GUID <> 'A2E106D9-C1B9-4F3E-8236-6AB48F27F040'
	  AND GUID <> 'C0C5232D-8139-44AC-939B-D221D8444612'

   UPDATE USUARIOS 
      SET USERNAME = CONCAT(
		'denis.carneiro.',
		SUBSTRING(
			CONVERT(
				VARCHAR(4), 
				REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CONCAT(NEWID(),NEWID()), 'A', ''), 'B', ''), 'C', ''), 'D', ''), 'E', ''), 'F', ''), '-', '')),
			1,
			4))
    WHERE DATA_PRIMEIRO_ACESSO IS NULL

--UPDATE USUARIOS SET USERNAME = 'UserMain' WHERE NOT DATA_PRIMEIRO_ACESSO IS NULL

select * from USUARIOS