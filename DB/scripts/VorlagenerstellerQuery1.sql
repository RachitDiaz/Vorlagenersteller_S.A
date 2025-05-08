CREATE DATABASE VorlaDB
GO
USE VorlaDB
GO

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

CREATE TABLE TelefonosPersona (
	ID int Identity PRIMARY KEY NOT NULL,
	Cedula char(12) FOREIGN KEY REFERENCES Persona(Cedula),
	Telefono varchar(15),
);

CREATE TABLE DireccionesPersona (
	ID int Identity PRIMARY KEY NOT NULL,
	Cedula char(12) FOREIGN KEY REFERENCES Persona(Cedula),
	Provincia varchar(20),
	Canton varchar(20),
	Distrito varchar(20),
	OtrasSenas varchar(300),
);

CREATE TABLE Usuario (
	ID int Identity PRIMARY KEY NOT NULL,
	Cedula char(12) FOREIGN KEY REFERENCES Persona(Cedula),
	Correo char(60) UNIQUE,
	Contrasena char(200),
);

CREATE TABLE Dueno (
	Cedula char(12) PRIMARY KEY NOT NULL FOREIGN KEY REFERENCES Persona(Cedula),
);

CREATE TABLE Beneficio (
	Nombre varchar(30) NOT NULL,
	ID int Identity PRIMARY KEY NOT NULL,
	Tipo varchar(20) NOT NULL,
	Descripcion varchar(300) NOT NULL,
	ServicioExterno varchar(300),
	MesesMinimos int NOT NULL,
	CantidadParametros int NOT NULL,
);


