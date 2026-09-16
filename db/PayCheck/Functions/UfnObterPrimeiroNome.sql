--SELECT dbo.[UfnObterPrimeiroNome]('ED WILSON DOS SANTOS MARTINS') AS FirstName
--SELECT dbo.[UfnObterPrimeiroNome]('Maria') AS FirstName

If OBJECT_ID(N'[dbo].[UfnObterPrimeiroNome]', N'FN') IS NOT NULL
	DROP FUNCTION [dbo].[UfnObterPrimeiroNome]
GO

CREATE FUNCTION [dbo].[UfnObterPrimeiroNome] (
	            @FullName VARCHAR(100))
RETURNS VARCHAR(100)

WITH ENCRYPTION

AS

BEGIN
	DECLARE @Result VARCHAR(100)

    -- Verifica se existe um espaço no nome completo.
    IF CHARINDEX(' ', @FullName) = 0
        -- Retorna o nome completo se não houver espaço.
        SET @Result = @FullName
    ELSE
        -- Retorna a parte antes do primeiro espaço.
        SET @Result = LEFT(@FullName, CHARINDEX(' ', @FullName) - 1)

	RETURN @Result
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