USE VorlaDB
GO

INSERT INTO Persona(Cedula, Nombre, Apellido1, Apellido2, Genero)
VALUES
('1-0000-4444', 'Admin', 'Admin' , 'Admin', 'Masculino')
GO

INSERT INTO Usuario(Cedula, Correo, Contrasena)
VALUES
('1-0000-4444', 'Admin.Admin@Admin.com', 'AQAAAAIAAYagAAAAEAd1kDuOukejtScmmXnGCS51tHxlWdO01USF/KsCN0K4aMhtKCj8nJefGofQbbPC4A==')
GO

INSERT INTO Administrador (Cedula)
values('1-0000-4444');
GO

DROP PROCEDURE RegistrarEmpresa
GO

CREATE PROCEDURE RegistrarEmpresa (@CedulaJuridica char(12), @CedulaDueno char(12), @TipoDePago varchar(10), @RazonSocial varchar(100),  
    @Nombre varchar(100), @Descripcion varchar(300), @BeneficiosMaximos int, @CorreoCreador char(60), @CorreoEmpresa varchar(300),  
    @Telefono varchar(15), @Provincia varchar(20), @Canton varchar(20), @Distrito varchar(20), @OtrasSenas varchar(300))  
AS  
BEGIN  
 DECLARE @CreadorId int  
  
 SELECT TOP 1 @CreadorId = ID  
  FROM Usuario  
  WHERE Correo = @CorreoCreador  
  
 INSERT INTO Empresa (CedulaJuridica, CedulaAdmin, CedulaDueno, TipoDePago, RazonSocial, Nombre,  
    Descripcion, BeneficiosMaximos, UsuarioCreador, UltimoEnModificar, activo)  
 VALUES( @CedulaJuridica,  
   '1-0000-4444',  
   @CedulaDueno,  
   @TipoDePago,  
   @RazonSocial,  
   @Nombre,  
   @Descripcion,  
   @BeneficiosMaximos,  
   @CreadorId,  
   @CreadorId,  
   1)  
  
 INSERT INTO TelefonosEmpresa (CedulaJuridica, Telefono)  
 VALUES( @CedulaJuridica,   
   @Telefono)  
  
 INSERT INTO CorreosElectronicosEmpresa(CedulaJuridica, CorreoElectronico)  
 VALUES( @CedulaJuridica,  
   @CorreoEmpresa)  
  
 INSERT INTO DireccionesEmpresa (CedulaJuridica, Provincia, Canton, Distrito, OtrasSenas)  
 VALUES( @CedulaJuridica,  
   @Provincia,  
   @Canton,  
   @Distrito,  
   @OtrasSenas)  
END;

DROP TRIGGER IF EXISTS trg_Empresa_Delete;
GO

-- Trigger para la eliminación de empresa
CREATE TRIGGER trg_Empresa_Delete
ON Empresa
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE e
    SET Borrado = 1
    FROM Empresa e
    INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
    WHERE EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );

	UPDATE e
	SET CedulaAdmin = NULL
	FROM Empresa e
	INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
	WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );

	DELETE e
	FROM Empleado e
	INNER JOIN DELETED d ON e.CedulaEmpresa = d.CedulaJuridica
	WHERE CedulaEmpresa = d.CedulaJuridica

	DELETE e
    FROM Empresa e
    INNER JOIN DELETED d ON e.CedulaJuridica = d.CedulaJuridica
    WHERE NOT EXISTS (
        SELECT 1 FROM PlanillaDeduccionesEmpresa pde
        WHERE pde.CedulaEmpresa = d.CedulaJuridica
    );
END;
GO