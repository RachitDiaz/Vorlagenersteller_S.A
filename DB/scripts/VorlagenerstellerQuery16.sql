USE VorlaDB

UPDATE Empleado
SET Editable = 0
WHERE Editable IS NULL;


INSERT INTO PlanillaDeduccionesEmpresa (CedulaEmpresa)
VALUES (
	'1-222-333444'
)

INSERT INTO PlanillaMensualEmpleado (IDPlanilla, CedulaEmpleado, SalarioBruto)
VALUES (
	1,
	'2-2222-2222',
	123000
)

INSERT INTO PlanillaDeduccionesEmpresa (CedulaEmpresa)
VALUES (
	'1-222-333444'
)

INSERT INTO PlanillaMensualEmpleado (IDPlanilla, CedulaEmpleado, SalarioBruto)
VALUES (
	2,
	'2-2222-2222',
	124000
)


