USE VorlaDB
GO 

DELETE FROM EmpresaOfreceBeneficio
GO
DELETE FROM Beneficio

ALTER TABLE Beneficio
DROP COLUMN ServicioExterno

ALTER TABLE Beneficio
ADD CedulaEmpresa char(12)

ALTER TABLE Beneficio
ADD CONSTRAINT FK_Empresa_Beneficio
FOREIGN KEY (CedulaEmpresa) REFERENCES Empresa(CedulaJuridica)

DROP TABLE EmpresaOfreceBeneficio