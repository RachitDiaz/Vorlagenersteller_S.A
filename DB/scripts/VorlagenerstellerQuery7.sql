USE VorlaDB;

DROP TABLE EmpleadosDeEmpresa;
Go

DECLARE @FKConstraint AS varchar(100)

Select @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'Usuario' AND CONSTRAINT_NAME LIKE 'FK%'

DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Usuario DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE Usuario
ADD CONSTRAINT FKPersonaCedula_Usuario 
FOREIGN KEY (Cedula) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;


Select TOP 1 @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'Empleado' AND CONSTRAINT_NAME LIKE 'FK%'

SET @SQL = 'ALTER TABLE Empleado DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE Empleado
ADD CONSTRAINT FKPersonaCedula_Empleado 
FOREIGN KEY (CedulaEmpleado) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;


Select TOP 1 @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'Dueno' AND CONSTRAINT_NAME LIKE 'FK%'

SET @SQL = 'ALTER TABLE Dueno DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE Dueno
ADD CONSTRAINT FKPersonaCedula_Dueno 
FOREIGN KEY (Cedula) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;


Select TOP 1 @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'Administrador' AND CONSTRAINT_NAME LIKE 'FK%'

SET @SQL = 'ALTER TABLE Administrador DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE Administrador
ADD CONSTRAINT FKPersonaCedula_Administrador
FOREIGN KEY (Cedula) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;


Select TOP 1 @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'TelefonosPersona' AND CONSTRAINT_NAME LIKE 'FK%'

SET @SQL = 'ALTER TABLE TelefonosPersona DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE TelefonosPersona
ADD CONSTRAINT FKPersonaCedula_TelefonosPersona
FOREIGN KEY (Cedula) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;


Select TOP 1 @FKConstraint = CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'DireccionesPersona' AND CONSTRAINT_NAME LIKE 'FK%'

SET @SQL = 'ALTER TABLE DireccionesPersona DROP CONSTRAINT |ConstraintName| '
SET @SQL = REPLACE(@SQL, '|ConstraintName|', @FKConstraint)

EXEC (@SQL)

ALTER TABLE DireccionesPersona
ADD CONSTRAINT FKPersonaCedula_DireccionesPersona
FOREIGN KEY (Cedula) 
REFERENCES Persona(Cedula) ON UPDATE CASCADE;

GO

CREATE PROCEDURE EditarEmpleado (@CedulaEmpleado char(12), @NewCedula char(12), @NewNombre varchar(20), @NewApellido1 varchar(20),
				 @NewApellido2 varchar(20), @NewGenero varchar(20), @NewBanco varchar(20), @NewContrato varchar(50), 
				 @NewCorreo char(60), @NewSalario decimal(11,2))
AS
BEGIN
	
	UPDATE Persona
	SET Cedula = @NewCedula, Nombre = @NewNombre, Apellido1 = @NewApellido1, Apellido2 = @NewApellido2, Genero = @NewGenero
	WHERE Cedula = @CedulaEmpleado;

	UPDATE Empleado
	SET Banco = @NewBanco, SalarioBruto = @NewSalario, TipoContrato = @NewContrato
	WHERE CedulaEmpleado = @CedulaEmpleado;

	UPDATE Usuario
	SET Correo = @NewCorreo
	WHERE Cedula = @CedulaEmpleado;

END;
GO

