If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspExcluirModalidadePorId]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspExcluirModalidadePorId]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspExcluirModalidadePorId]
	@Id INT

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

 DELETE M
   FROM [dbo].[Modalidade] AS m
  WHERE M.[Id] = @Id

GO