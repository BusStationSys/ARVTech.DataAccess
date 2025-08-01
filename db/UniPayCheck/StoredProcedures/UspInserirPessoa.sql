--EXEC [UspObterTotalizadores]

If Exists(Select * From sysobjects Where ID = OBJECT_ID(N'[dbo].[UspInserirPessoa]') And OBJECTPROPERTY(ID, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UspInserirPessoa]
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

CREATE PROCEDURE [dbo].[UspInserirPessoa]
	@Cep AS CHAR(8),
	@Complemento AS VARCHAR(30),
	@Email AS VARCHAR(75),
	@Numero AS VARCHAR(10),
	@Telefone AS VARCHAR(30)

WITH ENCRYPTION
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @NewGuidPessoa AS UNIQUEIDENTIFIER = NEWID()

 INSERT INTO [dbo].[PESSOA] ([GUID],
                            [CEP],
							[COMPLEMENTO],
							[DATA_INCLUSAO],
							[EMAIL],
							[NUMERO],
							[TELEFONE])
                    VALUES (@NewGuidPessoa,
					        @Cep,
							@Complemento,
							GETDATE(),
							@Email,
							@Numero,
							@Telefone)

 SELECT @NewGuidPessoa

GO