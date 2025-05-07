CREATE DATABASE piTest
GO
USE piTest
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
	Contraseña char(30),
);

CREATE TABLE Dueño (
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

-- Las siguientes líneas son inserciones manuales para probar el login y autenticación del usuario en la página

--INSERT INTO Persona(Cedula, Nombre, Apellido1, Apellido2, Genero) VALUES
--('1-1909-0924', 'Daniel', 'Shih', 'Tang', 'Masculino')

--INSERT INTO Usuario(Cedula, Correo, Contraseña) VALUES
--('1-1909-0924', 'shihtangdaniel@gmail.com', 'ContraseñaTemporal')

--INSERT INTO Dueño(Cedula) VALUES
--('1-1909-0924')

--SELECT * FROM Dueño, Persona, Usuario

--SELECT 
--    u.Correo,
--    CASE WHEN d.Cedula IS NOT NULL THEN 1 ELSE 0 END AS EsDueno
--FROM Usuario u
--LEFT JOIN Dueño d ON u.Cedula = d.Cedula
--WHERE u.Correo = 'shihtangdaniel@gmail.com' AND u.Contraseña = 'ContraseñaTemporal'
