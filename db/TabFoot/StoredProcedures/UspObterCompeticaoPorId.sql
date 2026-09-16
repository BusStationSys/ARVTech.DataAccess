If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspObterModalidadePorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspObterModalidadePorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspObterModalidadePorId]
	@Id INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 SELECT M.[Id],
        M.[Descricao],
        M.[UltimoConcursoApurado]
   FROM [dbo].[Modalidade] AS M WITH(NOLOCK)
  WHERE [M].[Id] = @Id 

GO