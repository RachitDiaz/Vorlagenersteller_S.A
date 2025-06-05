Use VorlaDB
Go

CREATE TABLE PlanillaMensualEmpresa (
    IDPlanilla INT PRIMARY KEY,
    CedulaEmpresa CHAR(12) NOT NULL,
    Periodo CHAR(7) NOT NULL, -- Ejemplo: '2025-06'

    TotalSEMPagar     DECIMAL(18, 2) NOT NULL,
    TotalSEMDeducir   DECIMAL(18, 2) NOT NULL,

    TotalIVMPagar     DECIMAL(18, 2) NOT NULL,
    TotalIVMDeducir   DECIMAL(18, 2) NOT NULL,

    TotalLPTPagar     DECIMAL(18, 2) NOT NULL,
    TotalLPTDeducir   DECIMAL(18, 2) NOT NULL,

    TotalRentaDeducir       DECIMAL(18, 2) NOT NULL,
    TotalBeneficiosDeducir  DECIMAL(18, 2) NOT NULL,

    FechaGeneracion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Empresa_PlanillaMensual FOREIGN KEY (CedulaEmpresa)
        REFERENCES Empresa(CedulaJuridica)
);
go

CREATE TABLE PlanillaMensualEmpleado (
    IDPlanilla INT NOT NULL,
    CedulaEmpleado CHAR(12) NOT NULL,

    SalarioBruto     DECIMAL(18, 2) NOT NULL,

    SEMEmpleado      DECIMAL(18, 2) NOT NULL,
    SEMPatrono       DECIMAL(18, 2) NOT NULL,

    IVMEmpleado      DECIMAL(18, 2) NOT NULL,
    IVMPatrono       DECIMAL(18, 2) NOT NULL,

    LPTEmpleado      DECIMAL(18, 2) NOT NULL,
    LPTPatrono       DECIMAL(18, 2) NOT NULL,

    ImpuestoRenta    DECIMAL(18, 2) NOT NULL,

    Beneficio1       DECIMAL(18, 2) NULL,
    Beneficio2       DECIMAL(18, 2) NULL,
    Beneficio3       DECIMAL(18, 2) NULL,

    TotalDeduccionesEmpleado DECIMAL(18, 2) NOT NULL,
    TotalDeduccionesPatrono  DECIMAL(18, 2) NOT NULL,

    FechaGeneracion DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_PlanillaMensualEmpleado PRIMARY KEY (IDPlanilla, CedulaEmpleado),

    CONSTRAINT FK_Empleado_PlanillaMensual FOREIGN KEY (CedulaEmpleado)
        REFERENCES Empleado(CedulaEmpleado),

    CONSTRAINT FK_PlanillaMensualEmpresa_Planilla FOREIGN KEY (IDPlanilla)
        REFERENCES PlanillaMensualEmpresa(IDPlanilla)
);
go