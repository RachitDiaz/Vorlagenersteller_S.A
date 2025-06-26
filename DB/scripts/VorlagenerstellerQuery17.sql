USE VorlaDB;
GO

-- Asegurar que todos los empleados tienen campo Editable inicializado
UPDATE Empleado
SET Editable = 1
WHERE Editable = 0;
GO

-- Eliminar tablas si existen
DROP TABLE IF EXISTS PlanillaMensualEmpleado;
DROP TABLE IF EXISTS PlanillaDeduccionesEmpresa;
GO

-- Crear tabla PlanillaDeduccionesEmpresa con IDPlanilla como uniqueidentifier
CREATE TABLE PlanillaDeduccionesEmpresa (
	IDPlanilla uniqueidentifier NOT NULL DEFAULT NEWID(),
	CONSTRAINT PK_PlanillaDeduccionesEmpresa PRIMARY KEY NONCLUSTERED (IDPlanilla),
	CedulaEmpresa char(12) NOT NULL,
	CONSTRAINT FK_PlanillaDeduccionesEmpresa FOREIGN KEY (CedulaEmpresa) REFERENCES Empresa(CedulaJuridica),
	Periodo char(15), -- Ejemplo: '2025-06'
	FechaDeCreacion datetime DEFAULT GETDATE(),
	FechaDeModificacion datetime DEFAULT GETDATE(),
	TotalSEMPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalIVMPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalBPOPPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalAsignacionesFamiliaresPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalIMASPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalINAPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalFCLPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalOPCPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalINSPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalBeneficiosPagar decimal(18, 2) NOT NULL DEFAULT 0,
	TotalSalariosPagar decimal(18, 2) NOT NULL DEFAULT 0
);
GO

-- Crear tabla PlanillaMensualEmpleado con IDPlanilla como uniqueidentifier
CREATE TABLE PlanillaMensualEmpleado (
	IDPlanilla uniqueidentifier NOT NULL,
	CedulaEmpleado char(12) NOT NULL,
	CONSTRAINT PK_PlanillaMensualEmpleado PRIMARY KEY NONCLUSTERED (IDPlanilla, CedulaEmpleado),
	CONSTRAINT FK_PlanillaMensualEmpleado FOREIGN KEY (CedulaEmpleado) REFERENCES Empleado(CedulaEmpleado),
	SalarioBruto decimal(18, 2) NOT NULL DEFAULT 0,
	SEMEmpleado decimal(18, 2) NOT NULL DEFAULT 0,
	IVEMEmpleado decimal(18, 2) NOT NULL DEFAULT 0,
	BPPOEmpleado decimal(18, 2) NOT NULL DEFAULT 0,
	ImpuestoRenta decimal(18, 2) NOT NULL DEFAULT 0,
	BeneficioMonto1 decimal(18, 2) DEFAULT 0,
	BeneficioMonto2 decimal(18, 2) DEFAULT 0,
	BeneficioMonto3 decimal(18, 2) DEFAULT 0,
	BeneficioNombre1 varchar(30),
	BeneficioNombre2 varchar(30),
	BeneficioNombre3 varchar(30),
	TotalDeduccionesEmpleado decimal(18, 2) NOT NULL DEFAULT 0,
	TotalDeduccionesPatrono decimal(18, 2) NOT NULL DEFAULT 0,
	TotalDeduccionesBeneficios decimal(18, 2) NOT NULL DEFAULT 0,
	FechaDeCreacion datetime DEFAULT GETDATE()
);
GO

-- Crear índices
CREATE CLUSTERED INDEX IX_PlanillaDeduccionesEmpresa_FechaDeCreacion
ON PlanillaDeduccionesEmpresa(FechaDeCreacion DESC);

CREATE CLUSTERED INDEX IX_PlanillaMensualEmpleado_FechaDeCreacion
ON PlanillaMensualEmpleado(FechaDeCreacion DESC);
GO

-- Trigger que actualiza la editabilidad de un empleado
CREATE TRIGGER trg_UpdateEditableEmpleado
ON PlanillaMensualEmpleado
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE E
    SET E.Editable = 0
    FROM Empleado E
    INNER JOIN inserted I ON E.CedulaEmpleado = I.CedulaEmpleado;
END;