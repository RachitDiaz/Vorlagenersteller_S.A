USE VorlaDB
CREATE TABLE Administrador (
	Cedula char(12) PRIMARY KEY NOT NULL FOREIGN KEY REFERENCES Persona(Cedula),
);

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

ALTER TABLE Beneficio
ADD FechaCreacion datetime DEFAULT GETDATE() NOT NULL,
	FechaModificacion datetime DEFAULT GETDATE() NOT NULL,
	UsuarioCrea int FOREIGN KEY REFERENCES Usuario(ID) NOT NULL,
	UsuarioModifica int FOREIGN KEY REFERENCES Usuario(ID) NOT NULL;
