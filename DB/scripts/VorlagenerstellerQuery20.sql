USE VorlaDB
GO 

-- Cambios foreign key Beneficio
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Beneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Empresa'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CedulaEmpresa';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Beneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Beneficio
ADD CONSTRAINT FK_Beneficio_Empresa
FOREIGN KEY (CedulaEmpresa) REFERENCES Empresa(CedulaJuridica)
ON DELETE CASCADE;
GO

-- Cambios foreign key TelefonosEmpresa
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'TelefonosEmpresa'
  AND OBJECT_NAME(f.referenced_object_id) = 'Empresa'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CedulaJuridica';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE TelefonosEmpresa DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE TelefonosEmpresa
ADD CONSTRAINT FK_Telefonos_Empresa
FOREIGN KEY (CedulaJuridica) REFERENCES Empresa(CedulaJuridica)
ON DELETE CASCADE;
GO

-- Cambios foreign key DireccionesEmpresa
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'DireccionesEmpresa'
  AND OBJECT_NAME(f.referenced_object_id) = 'Empresa'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CedulaJuridica';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE DireccionesEmpresa DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE DireccionesEmpresa
ADD CONSTRAINT FK_Direcciones_Empresa
FOREIGN KEY (CedulaJuridica) REFERENCES Empresa(CedulaJuridica)
ON DELETE CASCADE;
GO

-- Cambios foreign key CorreosElectronicosEmpresa
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'CorreosElectronicosEmpresa'
  AND OBJECT_NAME(f.referenced_object_id) = 'Empresa'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CedulaJuridica';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE CorreosElectronicosEmpresa DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE CorreosElectronicosEmpresa
ADD CONSTRAINT FK_Correos_Empresa
FOREIGN KEY (CedulaJuridica) REFERENCES Empresa(CedulaJuridica)
ON DELETE CASCADE;
GO

-- Cambios foreign key EligeBeneficio
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'EligeBeneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Beneficio'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ID';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE EligeBeneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE EligeBeneficio
ADD CONSTRAINT FK_Empleado_Elige_Beneficio
FOREIGN KEY (IDBeneficio) REFERENCES Beneficio(ID)
ON DELETE CASCADE;
GO

-- Cambios foreign key API
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'API'
  AND OBJECT_NAME(f.referenced_object_id) = 'Beneficio'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ID';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE API DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE API
ADD CONSTRAINT FK_API_Beneficio
FOREIGN KEY (IDBeneficio) REFERENCES Beneficio(ID)
ON DELETE CASCADE;
GO

-- Cambios foreign key ParametrosBeneficio
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'ParametrosBeneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Beneficio'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ID';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE ParametrosBeneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE ParametrosBeneficio
ADD CONSTRAINT FK_Parametros_Beneficio
FOREIGN KEY (IDBeneficio) REFERENCES Beneficio(ID)
ON DELETE CASCADE;
GO

ALTER TABLE Dueno ADD Borrado BIT NOT NULL DEFAULT 0;
GO

SELECT 
    f.name AS ForeignKeyName,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(f.referenced_object_id) AS ReferencedTable,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Dueno';

-- Cambios foreign key Dueno
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Dueno'
  AND OBJECT_NAME(f.referenced_object_id) = 'Persona'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'Cedula';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Dueno DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Dueno
ADD CONSTRAINT FK_Dueno_Persona
FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
ON DELETE CASCADE;
GO

ALTER TABLE Empresa ADD Borrado BIT NOT NULL DEFAULT 0;
GO

UPDATE e
SET CedulaAdmin = NULL
FROM Empresa e
WHERE e.CedulaJuridica = '1-222-333444';
GO

DELETE FROM Administrador
WHERE Cedula = '1-1909-0924';
GO

DROP TRIGGER IF EXISTS trg_Empresa_Delete;
GO

-- Trigger para la eliminación de empresa
CREATE TRIGGER trg_Empresa_Delete
ON Empresa
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE e
    SET Borrado = 1
    FROM Empresa e
    INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
    WHERE EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );

	UPDATE e
	SET CedulaAdmin = NULL
	FROM Empresa e
	INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
	WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );

	DELETE e
	FROM Empleado e
	INNER JOIN DELETED d ON e.CedulaEmpresa = d.CedulaJuridica
	WHERE CedulaEmpresa = d.CedulaJuridica

	DELETE e
    FROM Empresa e
    INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
    WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );

	IF NOT EXISTS (
		SELECT 1 FROM Empresa
		WHERE CedulaDueno IN (SELECT CedulaDueno FROM DELETED)
	)
	BEGIN
		DELETE p
		FROM Persona p
		INNER JOIN DELETED d ON p.Cedula = d.CedulaDueno;
	END
END;
GO

-- Cambios foreign key Beneficio
DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Beneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Usuario'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ID';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Beneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

DECLARE @constraintName NVARCHAR(255);
SELECT @constraintName = f.name
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Beneficio'
  AND OBJECT_NAME(f.referenced_object_id) = 'Usuario'
  AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'ID';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Beneficio DROP CONSTRAINT [' + @constraintName + ']');
END
GO

ALTER TABLE Beneficio
ALTER COLUMN UsuarioCrea INT NULL;
GO

ALTER TABLE Beneficio
ALTER COLUMN UsuarioModifica INT NULL;
GO

ALTER TABLE Beneficio
ADD CONSTRAINT FK_Beneficio_UsuarioCrea
FOREIGN KEY (UsuarioCrea) REFERENCES Usuario(ID)
ON DELETE NO ACTION;
GO

ALTER TABLE Beneficio
ADD CONSTRAINT FK_Beneficio_UsuarioModifica
FOREIGN KEY (UsuarioModifica) REFERENCES Usuario(ID)
ON DELETE NO ACTION;
GO

CREATE TRIGGER trg_Persona_Delete
ON Persona
INSTEAD OF DELETE
AS
BEGIN
	DELETE Usuario
	WHERE Cedula IN (SELECT Cedula FROM DELETED);

	DELETE FROM Persona
    WHERE Cedula IN (SELECT Cedula FROM DELETED);
END;
GO

ALTER TABLE Usuario
ADD CONSTRAINT FK_Usuario_Persona
FOREIGN KEY (Cedula) REFERENCES Persona(Cedula)
ON UPDATE CASCADE
ON DELETE NO ACTION;
GO


CREATE TRIGGER trg_Usuario_Delete
ON Usuario
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Beneficio
    SET UsuarioCrea = NULL
    WHERE UsuarioCrea IN (SELECT ID FROM DELETED);
    UPDATE Beneficio
    SET UsuarioModifica = NULL
    WHERE UsuarioModifica IN (SELECT ID FROM DELETED);
    DELETE FROM Usuario
    WHERE ID IN (SELECT ID FROM DELETED);
END;
GO
