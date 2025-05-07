USE piTest
CREATE TABLE Administrador (
	Cedula char(12) PRIMARY KEY NOT NULL FOREIGN KEY REFERENCES Persona(Cedula),
);

--INSERT INTO Administrador(Cedula) VALUES
--('1-1909-0924')

CREATE TABLE Empresa (
	CedulaJuridica char(12) PRIMARY KEY NOT NULL
		CHECK (
		PATINDEX('[1-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]', CedulaJuridica) = 1
		),
	CedulaAdmin char(12) FOREIGN KEY REFERENCES Administrador(Cedula),
	CedulaDueno char(12) NOT NULL FOREIGN KEY REFERENCES Dueno(Cedula),
	TipoDePago varchar(10) NOT NULL CHECK (TipoDePago IN ('Semanal', 'Quincenal', 'Mensual')),
	RazonSocial varchar(100) NOT NULL,
	Nombre varchar(100) NOT NULL,
	Descripcion varchar(300) NOT NULL,
	BeneficiosMaximos int NOT NULL,
	FechaDeModificacion datetime DEFAULT GETDATE() NOT NULL,
	FechaDeCreacion datetime DEFAULT GETDATE() NOT NULL,
	UsuarioCreador int FOREIGN KEY REFERENCES usuario(id) NOT NULL,
	UltimoEnModificar int FOREIGN KEY REFERENCES usuario(id) NOT NULL,
	activo bit NOT NULL
);

CREATE TABLE Empleado (
	CedulaEmpleado char(12) PRIMARY KEY FOREIGN KEY REFERENCES Persona(Cedula) NOT NULL,
	CedulaEmpresa char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	Banco varchar(22),
	SalarioBruto Decimal(11,2),
	FechaIngreso datetime DEFAULT GETDATE() NOT NULL,
	TipoContrato varchar(50) NOT NULL,
	FechaDeModificacion datetime DEFAULT GETDATE() NOT NULL,
	FechaDeCreacion datetime DEFAULT GETDATE() NOT NULL,
	UsuarioCreador int FOREIGN KEY REFERENCES usuario(id) NOT NULL,
	UltimoEnModificar int FOREIGN KEY REFERENCES usuario(id) NOT NULL,
);

CREATE TABLE EmpresaOfreceBeneficio (
	IDBeneficio int FOREIGN KEY REFERENCES Beneficio(ID) NOT NULL,
	CedulaEmpresa char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	PRIMARY KEY (IDBeneficio, CedulaEmpresa)
);
-- Hasta ahora se a�aden estas cosas porque no hab�an usuarios antes
--ALTER TABLE Beneficio
--ADD FechaCreacion datetime DEFAULT GETDATE() NOT NULL,
--	FechaModificacion datetime DEFAULT GETDATE() NOT NULL,
--	UsuarioCrea int FOREIGN KEY REFERENCES Usuario(ID) NOT NULL,
--	UsuarioModifica int FOREIGN KEY REFERENCES Usuario(ID) NOT NULL;

 -- Inserci�n manual para probar algunas funcionalidades en backend
--INSERT INTO Empresa(CedulaJuridica, CedulaAdmin, CedulaDueno, TipoDePago, RazonSocial,
--			Nombre, Descripcion, BeneficiosMaximos, UsuarioCreador, UltimoEnModificar, activo) VALUES
--('3-101-771169', '1-1909-0924', '1-1909-0924', 'Mensual', 'Razon social inventada', 'Nombre inventado', 'Descripcion inventada', 3, 1, 1, 0) 

--SELECT * FROM Empresa

-- Probando comando para recibir la c�dula de una empresa apartir de un correo de due�o
--SELECT e.CedulaJuridica
--FROM Usuario u
--JOIN Dueno d ON u.Cedula = d.Cedula
--JOIN Empresa e ON d.Cedula = e.CedulaDueno
--WHERE u.Correo = 'shihtangdaniel@gmail.com';