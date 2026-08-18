If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspAtualizarModalidade]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspAtualizarModalidade]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspAtualizarModalidade]
	@Id AS INT,
	@Descricao AS VARCHAR(75),
	@UltimoConcursoApurado AS INT = NULL

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

  UPDATE [dbo].[Modalidade]
     SET [Descricao] = @Descricao,
	     [UltimoConcursoApurado] = @UltimoConcursoApurado
   WHERE [ID] = @Id

GO