USE VorlaDB;

ALTER TABLE Usuario
ALTER COLUMN Correo CHAR(60) NOT NULL;

ALTER TABLE Usuario
ADD CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo);



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
