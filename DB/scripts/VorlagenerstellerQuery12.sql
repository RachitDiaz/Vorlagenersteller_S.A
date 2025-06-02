USE VorlaDB


CREATE TABLE EligeBeneficio (
	CedulaEmpleado char(12) FOREIGN KEY REFERENCES Empleado(CedulaEmpleado) NOT NULL,
	IDBeneficio int FOREIGN KEY REFERENCES Beneficio(ID) NOT NULL,
	PRIMARY KEY (IDBeneficio, CedulaEmpleado)
);


CREATE OR ALTER PROCEDURE ObtenerBeneficiosEmpleado
    @CorreoUsuario CHAR(60)
AS
BEGIN
    DECLARE @CedulaEmpleado CHAR(12);
    DECLARE @CedulaEmpresa CHAR(12);

    SELECT TOP 1 @CedulaEmpleado = Cedula
    FROM Usuario
    WHERE RTRIM(Correo) = RTRIM(@CorreoUsuario);

    IF @CedulaEmpleado IS NULL
    BEGIN
        RAISERROR('No se encontró la cédula del usuario.', 16, 1);
        RETURN;
    END

    SELECT TOP 1 @CedulaEmpresa = CedulaEmpresa
    FROM Empleado
    WHERE RTRIM(CedulaEmpleado) = RTRIM(@CedulaEmpleado);

    IF @CedulaEmpresa IS NULL
    BEGIN
        RAISERROR('No se encontró la empresa del empleado.', 16, 1);
        RETURN;
    END

    SELECT B.ID, B.Nombre
    FROM Beneficio B
    WHERE RTRIM(B.CedulaEmpresa) = RTRIM(@CedulaEmpresa);
END;
GO

-- maybe this is not needed, but it is a good practice to have the procedure in the same file
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

SELECT * FROM Empleado
SELECT *FROM Persona
SELECT * FROM Dueno
SELECT * FROM Usuario
SELECT * FROM Beneficio

UPDATE Beneficio
SET CedulaEmpresa = '3-234-231233'
WHERE ID IN (1005, 1006, 1007);

DECLARE @IDPiscina INT, @IDGimnasio INT, @IDSeguro INT;

SELECT @IDPiscina = ID FROM Beneficio WHERE Nombre = 'Piscina';
SELECT @IDGimnasio = ID FROM Beneficio WHERE Nombre = 'Gimnasio';
SELECT @IDSeguro = ID FROM Beneficio WHERE Nombre = 'Seguro';

CREATE PROCEDURE ObtenerBeneficiosSeleccionados
    @CorreoUsuario CHAR(60)
AS
BEGIN
    DECLARE @CedulaEmpleado CHAR(12)

    SELECT TOP 1 @CedulaEmpleado = Cedula
    FROM Usuario
    WHERE Correo = @CorreoUsuario;

    SELECT B.ID, B.Nombre
    FROM EligeBeneficio EB
    JOIN Beneficio B ON EB.IDBeneficio = B.ID
    WHERE EB.CedulaEmpleado = @CedulaEmpleado;
END;