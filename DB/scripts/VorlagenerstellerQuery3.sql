USE piTest
GO

CREATE TABLE TelefonosEmpresa (
	ID int Identity PRIMARY KEY NOT NULL,
	CedulaJuridica char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	Telefono varchar(15),
);

CREATE TABLE DireccionesEmpresa (
	ID int Identity PRIMARY KEY NOT NULL,
	CedulaJuridica char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	Provincia varchar(20),
	Canton varchar(20),
	Distrito varchar(20),
	OtrasSenas varchar(300),
);

CREATE TABLE CorreosElectronicosEmpresa(
	ID int Identity PRIMARY KEY NOT NULL,
	CedulaJuridica char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	CorreoElectronico varchar(300),
);

CREATE TABLE EmpleadosDeEmpresa(
	CedulaJuridica char(12) FOREIGN KEY REFERENCES Empresa(CedulaJuridica) NOT NULL,
	CedulaEmpleado char(12) FOREIGN KEY REFERENCES Empleado(CedulaEmpleado) NOT NULL,
	PRIMARY KEY (CedulaJuridica, CedulaEmpleado)
);



