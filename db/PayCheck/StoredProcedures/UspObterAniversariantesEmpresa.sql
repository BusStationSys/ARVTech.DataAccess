--EXEC [UspObterEventos]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterAniversariantesEmpresa]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterAniversariantesEmpresa]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterAniversariantesEmpresa]
	@Mes AS INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

     SELECT M.[GUID],
            M.[MATRICULA],
            M.[DATA_ADMISSAO],
            PF.[GUID],
            PF.[NOME],
            PJ.[GUID],
            PJ.[RAZAO_SOCIAL]
       FROM MATRICULAS as M
 INNER JOIN PESSOAS_FISICAS as PF
         ON M.GUIDCOLABORADOR = PF.GUID
 INNER JOIN PESSOAS_JURIDICAS as PJ
         ON M.GUIDEMPREGADOR = PJ.GUID
      WHERE DATA_DEMISSAO IS NULL
        AND MONTH(M.[DATA_ADMISSAO]) = @Mes

GO