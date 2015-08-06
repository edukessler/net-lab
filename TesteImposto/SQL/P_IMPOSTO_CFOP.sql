USE [Teste]
GO
IF OBJECT_ID('dbo.P_IMPOSTO_CFOP') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.P_IMPOSTO_CFOP
    IF OBJECT_ID('dbo.P_IMPOSTO_CFOP') IS NOT NULL
        PRINT '<<< FALHA APAGANDO A PROCEDURE dbo.P_IMPOSTO_CFOP >>>'
    ELSE
        PRINT '<<< PROCEDURE dbo.P_IMPOSTO_CFOP APAGADA >>>'
END
go
SET QUOTED_IDENTIFIER ON
GO
SET NOCOUNT ON 
GO 
CREATE PROCEDURE P_IMPOSTO_CFOP AS
BEGIN
	SELECT
		i.Cfop,
		SUM(i.BaseIcms) As BaseIcms,
		SUM(i.ValorIcms) As ValorIcms,
		SUM(i.BaseIpi) As BaseIpi,
		SUM(i.ValorIpi) As ValorIpi
	FROM	
		NotaFiscalItem	i
	GROUP BY
		i.Cfop	
END
GO
GRANT EXECUTE ON dbo.P_IMPOSTO_CFOP TO [public]
go
IF OBJECT_ID('dbo.P_IMPOSTO_CFOP') IS NOT NULL
    PRINT '<<< PROCEDURE dbo.P_IMPOSTO_CFOP CRIADA >>>'
ELSE
    PRINT '<<< FALHA NA CRIACAO DA PROCEDURE dbo.P_IMPOSTO_CFOP >>>'
GO