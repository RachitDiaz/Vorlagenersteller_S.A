USE VorlaDB
CREATE TABLE Persona (
	Cedula char(12) PRIMARY KEY NOT NULL
		CHECK (
		PATINDEX('[1-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]', Cedula) = 1
		),
	Nombre varchar(20),
	Apellido1 varchar(20),
	Apellido2 varchar(20),
	Genero varchar(20)
);

INSERT INTO Persona(Cedula, Nombre, Apellido1, Apellido2, Genero)
VALUES
('1-1111-1111', 'Ernesto', 'Campos' , 'Torres', 'Masculino'),
('2-2222-2222', 'Pedro', 'Martinez', 'Rojas', 'Femenino'),
('3-3333-3333', 'Joaquin', 'Feliciano', 'Ross', 'Mascullino'),
('4-4444-4444', 'Enrique', 'Campos', 'Guevara', 'Masculino'),
('5-5555-5555', 'Monica', 'Fonseca', 'Hidalgo', 'Femenino'),
('1-1909-0924', 'Daniel', 'Shih', 'Tang', 'Masculino');
GO

SELECT * FROM Persona
GO

CREATE TABLE Usuario (
	ID int Identity PRIMARY KEY NOT NULL,
	Cedula char(12) FOREIGN KEY REFERENCES Persona(Cedula),
	Correo char(60) UNIQUE,
	Contrasena char(200),
);

INSERT INTO Usuario(Cedula, Correo, Contrasena)
VALUES
('1-1909-0924', 'shihtangdaniel@gmail.com', 'AQAAAAIAAYagAAAAEJWirEN3cxCJjWvig9K5obrN05JKMm//jTaHXj7kR9bZn/UVyu0W6dHoXT9AYiufAg=='),
('1-1111-1111', 'ernesto.campos@gmail.com', 'AQAAAAIAAYagAAAAECRb8Vi9iTYEdZhihkRMV+YvMv91LJ64Y9gx9X3O0Ga3RakN8v9q7/f2JbS0ZTH9ew=='),
('2-2222-2222', 'pedro.martinez@gmail.com', 'AQAAAAIAAYagAAAAEJzIcApn5MI/ZaDmgOxGtLbcnucHAa7oB0BNj+XR+EOBp+/KGzq+hIIJ4AYEhCEdDA=='),
('3-3333-3333', 'joaquin.feliciano@gmail.com', 'AQAAAAIAAYagAAAAEDq6y/qoYBTwwTe4JkTC/VhCATasldyhbN+V0UkGaR4ejM4m0OG4/KfiUT5Kx93Pzw=='),
('4-4444-4444', 'enrique.campos@gmail.com', 'AQAAAAIAAYagAAAAELHm8OI7DtmAR6GbQFFxv60PMmUOYVF8eyycw/PBjIIV1LVdjadAlgcFgCkYfmaMJg=='),
('5-5555-5555', 'monica.fonseca@hotmail.com', 'AQAAAAIAAYagAAAAEGewkPNT2xt7n5EjMOD1LiUu6A73S/VEoBqXAqfIJ/4vA0FmLJ+zudt32VkO4zc6Gw==');
GO
SELECT * FROM Usuario
GO

INSERT INTO Dueno(Cedula)
VALUES
('1-1909-0924');
GO

INSERT INTO Administrador (Cedula)
values('1-1909-0924');
GO

INSERT INTO Empresa (CedulaJuridica, CedulaAdmin, CedulaDueno, TipoDePago, Nombre, RazonSocial, Descripcion, BeneficiosMaximos, UsuarioCreador, UltimoEnModificar, activo)
VALUES( '1-222-333444',
		'1-1909-0924',
		'1-1909-0924',
		'Semanal',
		'PlasTico',
		'Empresa dedicada a crear plasticos biodegradables',
		'Manufactura de productos de plastico biodegradable',
		3,
		6,
		6,
		1
);	
GO

Select * FROM Empresa


INSERT INTO Empleado(CedulaEmpleado, CedulaEmpresa, Banco, SalarioBruto, TipoContrato, UsuarioCreador, UltimoEnModificar)
VALUES ('2-2222-2222',
		'1-222-333444',
		'CR100002882910',
		134000.5,
		'Medio tiempo',
		6,
		6
);
GO

SELECT * FROM Beneficio