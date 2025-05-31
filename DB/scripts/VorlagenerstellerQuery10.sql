USE VorlaDB


CREATE TABLE EligeBeneficio (
	CedulaEmpleado char(12) FOREIGN KEY REFERENCES Empleado(CedulaEmpleado) NOT NULL,
	IDBeneficio int FOREIGN KEY REFERENCES Beneficio(ID) NOT NULL,
	PRIMARY KEY (IDBeneficio, CedulaEmpleado)
);

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

CREATE PROCEDURE ObtenerBeneficiosEmpleado
    @CorreoUsuario CHAR(60)
AS
BEGIN
    DECLARE @CedulaEmpleado CHAR(12);
    DECLARE @CedulaEmpresa CHAR(12);
    DECLARE @CedulaDueno CHAR(12);

    SELECT TOP 1 @CedulaEmpleado = Cedula
    FROM Usuario
    WHERE Correo = @CorreoUsuario;

    SELECT TOP 1
        @CedulaEmpresa = CedulaEmpresa
    FROM Empleado
    WHERE CedulaEmpleado = @CedulaEmpleado;

    SELECT TOP 1
        @CedulaDueno = CedulaDueno
    FROM Empresa
    WHERE CedulaJuridica = @CedulaEmpresa;

    SELECT B.ID, B.Nombre
    FROM EmpresaOfreceBeneficio EB
    JOIN Beneficio B ON EB.IDBeneficio = B.ID
    WHERE EB.CedulaEmpresa = @CedulaEmpresa
    AND B.UsuarioCrea IN (
        SELECT ID FROM Usuario WHERE Cedula = @CedulaDueno
    );
END;
GO

SELECT * FROM Empleado
SELECT *FROM Persona
SELECT * FROM Dueno
SELECT * FROM Usuario
SELECT * FROM Beneficio

INSERT INTO Beneficio (Nombre, Tipo, Descripcion, ServicioExterno, MesesMinimos, CantidadParametros, FechaCreacion, FechaModificacion, UsuarioCrea, UsuarioModifica)
VALUES
('Piscina', 'Recreativo', 'Acceso a piscina de la empresa', NULL, 0, 0, GETDATE(), GETDATE(), 6, 6),
('Gimnasio', 'Salud', 'Gimnasio corporativo disponible', NULL, 0, 0, GETDATE(), GETDATE(), 6, 6),
('Seguro', 'Salud', 'Seguro medico complementario', NULL, 6, 1, GETDATE(), GETDATE(), 6, 6);

DECLARE @IDPiscina INT, @IDGimnasio INT, @IDSeguro INT;

SELECT @IDPiscina = ID FROM Beneficio WHERE Nombre = 'Piscina';
SELECT @IDGimnasio = ID FROM Beneficio WHERE Nombre = 'Gimnasio';
SELECT @IDSeguro = ID FROM Beneficio WHERE Nombre = 'Seguro';

INSERT INTO EmpresaOfreceBeneficio (IDBeneficio, CedulaEmpresa)
VALUES
(@IDPiscina, '1-222-333444'),
(@IDGimnasio, '1-222-333444'),
(@IDSeguro, '1-222-333444');

EXEC ObtenerBeneficiosEmpleado @CorreoUsuario = 'pedro.martinez@gmail.com';
SELECT ID, Nombre FROM Beneficio
WHERE Nombre IN ('Piscina', 'Gimnasio', 'Seguro');

INSERT INTO EligeBeneficio (CedulaEmpleado, IDBeneficio)
VALUES
('2-2222-2222', 1002), 
('2-2222-2222', 1003);

SELECT EB.CedulaEmpleado, B.Nombre
FROM EligeBeneficio EB
JOIN Beneficio B ON EB.IDBeneficio = B.ID
WHERE CedulaEmpleado = '2-2222-2222';

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