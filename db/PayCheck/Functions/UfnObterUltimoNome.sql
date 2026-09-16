--DECLARE @Nome AS VARCHAR (100) = 'ED WILSON DOS SANTOS MARTINS'

--	DECLARE @FirstName AS VARCHAR(100) = dbo.[UfnObterPrimeiroNome](@Nome)
--	DECLARE @LastName AS VARCHAR(100) = dbo.[UfnObterUltimoNome](@Nome)

--	PRINT @FirstName
--	PRINT @LastName

--	DECLARE @Username AS VARCHAR(75) = CONCAT(@FirstName, '.', @LastName)

--	PRINT @Username

--SELECT dbo.[UfnObterUltimoNome]('ED WILSON DOS SANTOS MARTINS') AS LastName
--SELECT dbo.[UfnObterUltimoNome]('Maria') AS LastName

If OBJECT_ID(N'[dbo].[UfnObterUltimoNome]', N'FN') IS NOT NULL
	DROP FUNCTION [dbo].[UfnObterUltimoNome]
GO

CREATE FUNCTION [dbo].[UfnObterUltimoNome] (
	            @FullName VARCHAR(100))
RETURNS VARCHAR(100)

WITH ENCRYPTION

AS

BEGIN
	DECLARE @Result VARCHAR(100)

    -- Verifica se existe um espaço no nome completo
    IF CHARINDEX(' ', @FullName) = 0
        -- Retorna o nome completo se não houver espaço
        SET @Result = @FullName;
    ELSE
        -- Retorna a última parte depois do último espaço
        SET @Result = REVERSE(LEFT(REVERSE(@FullName), CHARINDEX(' ', REVERSE(@FullName)) - 1));

    -- Retorna o resultado
    RETURN @Result;
END

GO

--GO
--If OBJECT_ID(N'dbo.SIS_UFN_Base64_Encode') IS NOT NULL
--    Drop Function dbo.SIS_UFN_Base64_Encode;
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE FUNCTION [dbo].[SIS_UFN_Base64_Encode] (
--    @string VARCHAR(MAX)
--) 
--RETURNS VARCHAR(MAX)
--AS BEGIN
 
--    DECLARE 
--        @source VARBINARY(MAX), 
--        @encoded VARCHAR(MAX)
        
--    SET @source = CONVERT(VARBINARY(MAX), @string)
--    SET @encoded = CAST('' AS XML).value('xs:base64Binary(sql:variable("@source"))', 'varchar(max)')
 
--    RETURN @encoded
 
--END

--GO

--GO
--If OBJECT_ID(N'dbo.SIS_UFN_Base64_Decode') IS NOT NULL
--    Drop Function dbo.SIS_UFN_Base64_Decode;
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE FUNCTION [dbo].[SIS_UFN_Base64_Decode] (
--    @string VARCHAR(MAX)
--)
--RETURNS VARCHAR(MAX)
--AS BEGIN
 
--    DECLARE @decoded VARCHAR(MAX)
--    SET @decoded = CAST('' AS XML).value('xs:base64Binary(sql:variable("@string"))', 'varbinary(max)')
 
--    RETURN CONVERT(VARCHAR(MAX), @decoded)
    
--END

--GO