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

insert into Persona
values( '1-6231-7231','Juan','Perez','Torres','Masculino'),
	  ( '1-2345-6789', 'Pepe', 'El', 'Apellido', 'Masculino'),
	  ( '1-2222-3333', 'Manolo', 'Apellido', 'Apellido denuevo', 'Masculino')
;

insert into Usuario (Cedula, Correo, Contrasena)
values ('1-6231-7231', 'juan.perez@plasticos.cr', 'plastico1234'),
	   ('1-2345-6789', 'admin@vorlagener.com', '4321');

insert into Dueno
values ('1-6231-7231');

insert into Administrador
values ('1-2345-6789');

insert into Empresa (CedulaJuridica, CedulaDueno, CedulaAdmin, TipoDePago, RazonSocial, Nombre, Descripcion, BeneficiosMaximos, FechaDeCreacion,
UsuarioCreador, UltimoEnModificar, activo, FechaDeModificacion)
values(
    '1-222-333444', 
	'1-6231-7231',
	'1-2345-6789',
	'Semanal',
    'Producir plasticos sostenibles en CR',
	'PlasTicos',
	'Empresa de plasticos',
	3, 
	'20000423 10:34:09 AM', 
	1,
	1,
	1,
	'20000423 10:34:09 AM'
);

insert into TelefonosEmpresa (CedulaJuridica, Telefono)
values ( '1-222-333444', '8450-8038');

insert into DireccionesEmpresa(CedulaJuridica, Provincia, Canton, Distrito, OtrasSenas)
values ( '1-222-333444', 'San Jose', 'Montes de Oca', 'San Pedro', 'Debajo del centro de informatica UCR');

insert into CorreosElectronicosEmpresa (CedulaJuridica, CorreoElectronico)
values ( '1-222-333444', 'recepcion@plasticos.cr');

insert into Empleado  (CedulaEmpleado, CedulaEmpresa, Banco, SalarioBruto, TipoContrato, UsuarioCreador, UltimoEnModificar)
values ( '1-2222-3333', '1-222-333444', 'BAC', 100000.25, 'medio tiempo', 1, 1)

insert into EmpleadosDeEmpresa
values ('1-222-333444', '1-2222-3333');


