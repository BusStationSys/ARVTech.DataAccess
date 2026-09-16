If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirCompeticaoTime]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirCompeticaoTime]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirCompeticaoTime]
	@Id AS UNIQUEIDENTIFIER = NULL,
	@IdCompeticao AS UNIQUEIDENTIFIER,
	@IdTime AS UNIQUEIDENTIFIER

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @DataAtual AS DATETIMEOFFSET = SYSDATETIMEOFFSET() AT TIME ZONE 'E. South America Standard Time'

SET @Id = COALESCE(@Id, NEWID())

DELETE FROM [dbo].[CompeticaoTime]
 WHERE [IdCompeticao] = @IdCompeticao
   AND [IdTime] = @IdTime

INSERT INTO [dbo].[CompeticaoTime] ([Id],
                                    [IdCompeticao],
									[IdTime])
	 VALUES (@Id,
			 @IdCompeticao,
			 @IdTime)

GO