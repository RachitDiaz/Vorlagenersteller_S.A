USE VorlaDB

INSERT INTO TelefonosEmpresa (CedulaJuridica, Telefono)
	VALUES ('1-222-333444' ,'8000-9000')

INSERT INTO CorreosElectronicosEmpresa(CedulaJuridica, CorreoElectronico)
	VALUES ('1-222-333444' ,'correo@empresa.org')

INSERT INTO DireccionesEmpresa (CedulaJuridica, Provincia, Canton, Distrito, OtrasSenas)
	VALUES ('1-222-333444', 'Heredia', 'Santo domingo', 'Tures', '400m sur del super La Amistad')

Go

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
	VALUES(	@CedulaJuridica,
			'1-1909-0924',
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
	VALUES(	@CedulaJuridica, 
			@Telefono)

	INSERT INTO CorreosElectronicosEmpresa(CedulaJuridica, CorreoElectronico)
	VALUES(	@CedulaJuridica,
			@CorreoEmpresa)

	INSERT INTO DireccionesEmpresa (CedulaJuridica, Provincia, Canton, Distrito, OtrasSenas)
	VALUES(	@CedulaJuridica,
			@Provincia,
			@Canton,
			@Distrito,
			@OtrasSenas)
END;
GO
