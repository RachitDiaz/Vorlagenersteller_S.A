USE VorlaDB
GO
CREATE TABLE API (
	IDBeneficio INT PRIMARY KEY,
	ServicioExterno VARCHAR(300),
	Metodo VARCHAR(5),
	FOREIGN KEY (IDBeneficio) REFERENCES Beneficio(ID),
);

CREATE TABLE ParametrosBeneficio (
	IDParametro INT IDENTITY PRIMARY KEY ,
	IDBeneficio INT,
	Nombre VARCHAR(30) NOT NULL,
	TipoDeDatoParametro VARCHAR (20) NOT NULL,
	ValorDelParametro INT,
	FOREIGN KEY (IDBeneficio) REFERENCES Beneficio(ID),
);