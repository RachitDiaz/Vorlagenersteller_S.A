USE VorlaDB

ALTER TABLE Persona
ADD FechaNacimiento DATE DEFAULT '2000-01-01';

UPDATE Persona
SET FechaNacimiento = '2000-01-01'
WHERE FechaNacimiento IS NULL;
SELECT * FROM Empleado
ALTER TABLE Empleado
ADD CantidadDependientes INT DEFAULT 3;

SELECT * FROM Empleado
UPDATE Empleado
SET CantidadDependientes = 3
WHERE FechaNacimiento IS NOT 3;