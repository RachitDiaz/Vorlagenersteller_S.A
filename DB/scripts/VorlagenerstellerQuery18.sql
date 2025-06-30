USE VorlaDB

ALTER TABLE Persona
ADD FechaNacimiento DATE DEFAULT '2000-01-01';
GO

UPDATE Persona
SET FechaNacimiento = '2000-01-01'
WHERE FechaNacimiento IS NULL;
GO
SELECT * FROM Empleado

ALTER TABLE Empleado
ADD CantidadDependientes INT DEFAULT 3;
GO

SELECT * FROM Empleado
UPDATE Empleado
SET CantidadDependientes = 3
WHERE CantidadDependientes IS NULL;
GO