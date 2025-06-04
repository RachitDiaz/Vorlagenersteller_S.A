Use VorlaDB
Go

CREATE TABLE PlanillaMensual (
    IDPlanilla INT IDENTITY(1,1) PRIMARY KEY,
    PeriodoMes CHAR(7) NOT NULL, -- Ejemplo: '2025-06'
    CedulaEmpleado CHAR(12) NOT NULL,
    CedulaEmpresa CHAR(12) NOT NULL,

    SalarioBruto DECIMAL(18, 2) NOT NULL,

    SEMEmpleado DECIMAL(18, 2) NOT NULL,
    SEMPatrono  DECIMAL(18, 2) NOT NULL,

    IVMEmpleado DECIMAL(18, 2) NOT NULL,
    IVMPatrono  DECIMAL(18, 2) NOT NULL,

    LPTEmpleado DECIMAL(18, 2) NOT NULL,
    LPTPatrono  DECIMAL(18, 2) NOT NULL,

    ImpuestoRenta DECIMAL(18, 2) NOT NULL,

    TotalDeduccionesEmpleado DECIMAL(18, 2) NOT NULL,
    TotalDeduccionesPatrono  DECIMAL(18, 2) NOT NULL,

    Beneficio1 DECIMAL(18, 2) NULL,
    Beneficio2 DECIMAL(18, 2) NULL,
    Beneficio3 DECIMAL(18, 2) NULL,

    FechaGeneracion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Empleado_Planilla FOREIGN KEY (CedulaEmpleado) REFERENCES Empleado(CedulaEmpleado),
    CONSTRAINT FK_Empresa_Planilla FOREIGN KEY (CedulaEmpresa) REFERENCES Empresa(CedulaJuridica)
);