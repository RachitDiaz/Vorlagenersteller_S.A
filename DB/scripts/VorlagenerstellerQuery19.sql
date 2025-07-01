USE VorlaDB
GO

ALTER TABLE Empleado ADD Borrado BIT NOT NULL DEFAULT 0;
GO

-- Cambios foreign key EligeBeneficio
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'EligeBeneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Empleado'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CedulaEmpleado';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE EligeBeneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE EligeBeneficio
ADD CONSTRAINT FK_Beneficio_Empleado
FOREIGN KEY (CedulaEmpleado) REFERENCES Empleado(CedulaEmpleado)
ON DELETE CASCADE;
GO

-- Cambios foreign key Empleado
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Empleado'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Empleado DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Empleado
ADD CONSTRAINT FK_Empleado_Persona
FOREIGN KEY (CedulaEmpleado) REFERENCES Persona(Cedula)
ON DELETE CASCADE;
GO

-- Cambios foreign key TelefonosPersona
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'TelefonosPersona'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE TelefonosPersona DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE TelefonosPersona
ADD CONSTRAINT FK_Telefonos_Persona
FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
ON UPDATE CASCADE
ON DELETE CASCADE;
GO

-- Cambios foreign key DireccionesPersona
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'DireccionesPersona'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE DireccionesPersona DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE DireccionesPersona
ADD CONSTRAINT FK_Direcciones_Persona
FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
ON UPDATE CASCADE
ON DELETE CASCADE;
GO

-- Cambios foreign key Usuario
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Usuario'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Usuario DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Usuario
ADD CONSTRAINT FK_Usuario_Persona
FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
ON UPDATE CASCADE
ON DELETE CASCADE;
GO

-- Cambios foreign key Empleado
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Empleado'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Empleado DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Empleado
ADD CONSTRAINT FK_Empleado_Persona
FOREIGN KEY (CedulaEmpleado) REFERENCES Persona(Cedula)
GO

-- Trigger para la eliminación de los empleados
CREATE TRIGGER trg_Empleado_Delete
ON Empleado
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE e
    SET Borrado = 1
    FROM Empleado e
    INNER JOIN DELETED d ON e.CedulaEmpleado = d.CedulaEmpleado
    WHERE EXISTS (
        SELECT 1 FROM PlanillaMensualEmpleado pme
        WHERE pme.CedulaEmpleado = d.CedulaEmpleado
    );

	DELETE e
    FROM Empleado e
    INNER JOIN DELETED d ON e.CedulaEmpleado = d.CedulaEmpleado
    WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaMensualEmpleado pme
        WHERE pme.CedulaEmpleado = d.CedulaEmpleado
    );

    DELETE p
    FROM Persona p
    INNER JOIN DELETED d ON p.Cedula = d.CedulaEmpleado
    WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaMensualEmpleado pme
        WHERE pme.CedulaEmpleado = d.CedulaEmpleado
    );
END;
GO