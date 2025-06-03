USE VorlaDB


CREATE TABLE EligeBeneficio (
	CedulaEmpleado char(12) FOREIGN KEY REFERENCES Empleado(CedulaEmpleado) NOT NULL,
	IDBeneficio int FOREIGN KEY REFERENCES Beneficio(ID) NOT NULL,
	PRIMARY KEY (IDBeneficio, CedulaEmpleado)
);


CREATE OR ALTER FUNCTION dbo.FnObtenerCedulaEmpresaDesdeCorreo
(
    @CorreoUsuario CHAR(60)
)
RETURNS CHAR(12)
AS
BEGIN
    DECLARE @CedulaEmpleado CHAR(12);
    DECLARE @CedulaEmpresa CHAR(12);

    SELECT TOP 1 @CedulaEmpleado = Cedula
    FROM Usuario
    WHERE RTRIM(Correo) = RTRIM(@CorreoUsuario);

    SELECT TOP 1 @CedulaEmpresa = CedulaEmpresa
    FROM Empleado
    WHERE CedulaEmpleado = @CedulaEmpleado;

    RETURN @CedulaEmpresa;
END;
GO

CREATE OR ALTER FUNCTION dbo.FnObtenerBeneficiosParaEmpleado
(
    @CorreoUsuario CHAR(60)
)
RETURNS TABLE
AS
RETURN
(
    SELECT B.ID, B.Nombre
    FROM Beneficio B
    WHERE RTRIM(B.CedulaEmpresa) = RTRIM(dbo.FnObtenerCedulaEmpresaDesdeCorreo(@CorreoUsuario))
);
GO

CREATE OR ALTER FUNCTION dbo.FnObtenerBeneficiosSeleccionados
(
    @CorreoUsuario CHAR(60)
)
RETURNS TABLE
AS
RETURN
(
    SELECT B.ID, B.Nombre
    FROM EligeBeneficio EB
    JOIN Beneficio B ON EB.IDBeneficio = B.ID
    WHERE EB.CedulaEmpleado = (
        SELECT TOP 1 Cedula
        FROM Usuario
        WHERE RTRIM(Correo) = RTRIM(@CorreoUsuario)
    )
);
GO

CREATE PROCEDURE ActualizarBeneficiosEmpleado
    @CedulaEmpleado CHAR(12),
    @ListaBeneficios NVARCHAR(MAX)
AS
BEGIN
    DELETE FROM EligeBeneficio
    WHERE CedulaEmpleado = @CedulaEmpleado;

    DECLARE @json NVARCHAR(MAX) = @ListaBeneficios;

    INSERT INTO EligeBeneficio (CedulaEmpleado, IDBeneficio)
    SELECT 
        @CedulaEmpleado,
        JSON_VALUE(value, '$.id') AS IDBeneficio
    FROM OPENJSON(@json);
END;
GO