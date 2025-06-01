USE VorlaDB;
Go

DECLARE @ConstraintName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);

-- Buscar el nombre del constraint UNIQUE aplicado específicamente a la columna 'correo'
SELECT TOP 1 @ConstraintName = kc.name
FROM sys.key_constraints kc
JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE kc.type = 'UQ'
  AND kc.parent_object_id = OBJECT_ID('Usuario')
  AND c.name = 'correo';

-- Si se encuentra, eliminarlo
IF @ConstraintName IS NOT NULL
BEGIN
    SET @SQL = 'ALTER TABLE Usuario DROP CONSTRAINT [' + @ConstraintName + ']';
    EXEC sp_executesql @SQL;
    PRINT 'Se eliminó el constraint UNIQUE de la columna correo: ' + @ConstraintName;
END
ELSE
BEGIN
    PRINT 'No se encontró constraint UNIQUE en la columna correo.';
END

ALTER TABLE Usuario
ALTER COLUMN Correo CHAR(60) NOT NULL;
Go

ALTER TABLE Usuario
ADD CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo);
Go

INSERT INTO Empleado(CedulaEmpleado, CedulaEmpresa, Banco, SalarioBruto, TipoContrato, UsuarioCreador, UltimoEnModificar)
VALUES ('3-3333-3333',
		'1-222-333444',
		'CR100002882910',
		134000.5,
		'Medio tiempo',
		1,
		6
);
GO

SELECT * FROM Persona;
SELECT * FROM Beneficio;
