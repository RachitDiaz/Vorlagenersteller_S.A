Use VorlaDB
GO

CREATE PROCEDURE RegistrarAPI(
	@Nombre VARCHAR(30),
    @Tipo VARCHAR(20),
    @Descripcion VARCHAR(300),
    @MesesMinimos INT,
    @CantidadParametros INT,
    @UsuarioCrea INT,
    @UsuarioModifica INT,
    @CedulaEmpresa CHAR(12) = NULL,
    @ServicioExterno VARCHAR(300),
    @Metodo VARCHAR(5))
AS
BEGIN
	DECLARE @NuevoID int
	INSERT INTO Beneficio(Nombre, Tipo, Descripcion, MesesMinimos, CantidadParametros,
        UsuarioCrea, UsuarioModifica, CedulaEmpresa)
    VALUES
	(@Nombre, @Tipo, @Descripcion, @MesesMinimos, @CantidadParametros, @UsuarioCrea, @UsuarioModifica, @CedulaEmpresa);

	SET @NuevoID = SCOPE_IDENTITY();

	INSERT INTO API(IDBeneficio, ServicioExterno, Metodo)
    VALUES
	(@NuevoID, @ServicioExterno, @Metodo);
END;
GO

ALTER TABLE API
ADD NombreKey VARCHAR(30) NOT NULL,
	ValorKey VARCHAR(300) NOT NULL;
GO

ALTER PROCEDURE RegistrarAPI(
	@Nombre VARCHAR(30),
    @Tipo VARCHAR(20),
    @Descripcion VARCHAR(300),
    @MesesMinimos INT,
    @CantidadParametros INT,
    @UsuarioCrea INT,
    @UsuarioModifica INT,
    @CedulaEmpresa CHAR(12) = NULL,
    @ServicioExterno VARCHAR(300),
    @Metodo VARCHAR(5),
	@NombreKey VARCHAR(30),
	@ValorKey VARCHAR(300))
AS
BEGIN
	DECLARE @NuevoID int
	INSERT INTO Beneficio(Nombre, Tipo, Descripcion, MesesMinimos, CantidadParametros,
        UsuarioCrea, UsuarioModifica, CedulaEmpresa)
    VALUES
	(@Nombre, @Tipo, @Descripcion, @MesesMinimos, @CantidadParametros, @UsuarioCrea, @UsuarioModifica, @CedulaEmpresa);

	SET @NuevoID = SCOPE_IDENTITY();

	INSERT INTO API(IDBeneficio, ServicioExterno, Metodo, NombreKey, ValorKey)
    VALUES
	(@NuevoID, @ServicioExterno, @Metodo, @NombreKey, @ValorKey);
END;
GO

EXEC RegistrarAPI
	@Nombre = 'Asociacion Solidarista',
	@Tipo = 'API',
	@Descripcion = 'Asociacion Solidarista',
	@MesesMinimos = 0,
	@CantidadParametros = 2,
	@UsuarioCrea = 1,
	@UsuarioModifica = 1,
	@ServicioExterno = 'https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate',
	@Metodo = 'POST',
	@NombreKey = 'API-KEY',
	@ValorKey = 'Tralalerotralala';
GO

EXEC RegistrarAPI
	@Nombre = 'MediSeguro',
	@Tipo = 'API',
	@Descripcion = 'Seguro medico',
	@MesesMinimos = 0,
	@CantidadParametros = 3,
	@UsuarioCrea = 1,
	@UsuarioModifica = 1,
	@ServicioExterno = 'https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto',
	@Metodo = 'POST',
	@NombreKey = 'token',
	@ValorKey = 'TOKEN123';
GO

EXEC RegistrarAPI
	@Nombre = 'Poliza de Vida',
	@Tipo = 'API',
	@Descripcion = 'Poliza de Vida',
	@MesesMinimos = 0,
	@CantidadParametros = 2,
	@UsuarioCrea = 1,
	@UsuarioModifica = 1,
	@ServicioExterno = 'https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance',
	@Metodo = 'GET',
	@NombreKey = 'FRIENDS-API-TOKEN',
	@ValorKey = '1D0B194488C091852597B9AF7D1AA8F23D55C9784815489CF0A488B6F2C6D5C4C569AD51231FACB9920E5A763FE388247E03131D1601AC234E86BC0D266EB6A7';
GO

EXEC RegistrarAPI
	@Nombre = 'TSE',
	@Tipo = 'API',
	@Descripcion = 'TSE',
	@MesesMinimos = 0,
	@CantidadParametros = 1,
	@UsuarioCrea = 1,
	@UsuarioModifica = 1,
	@ServicioExterno = 'https://tse-infinipay-deengcb5bqazhdh0.southcentralus-01.azurewebsites.net/api/TSE',
	@Metodo = 'GET',
	@NombreKey = 'Auth-Token',
	@ValorKey = '789xyz$%&';
GO

EXEC RegistrarAPI
	@Nombre = 'Registro Nacional',
	@Tipo = 'API',
	@Descripcion = 'Registro Nacional',
	@MesesMinimos = 0,
	@CantidadParametros = 1,
	@UsuarioCrea = 1,
	@UsuarioModifica = 1,
	@ServicioExterno = 'https://registro-blitz-cegnaceuhnbsbdak.southcentralus-01.azurewebsites.net/api/NationalRegister/validate',
	@Metodo = 'POST',
	@NombreKey = 'Authorization',
	@ValorKey = '123Blitz';
GO

SELECT ID FROM Beneficio
WHERE
Beneficio.Nombre = 'Asociacion Solidarista';

SELECT * FROM Beneficio
SELECT * FROM API
GO

CREATE PROCEDURE RegistrarParametro(
	@NombreParametro VARCHAR(30),
	@TipoDeDatoParametro VARCHAR(20),
	@ValorDelParametro INT,
	@NombreBeneficio VARCHAR(30),
	@Tipo VARCHAR(20) = 'API')
AS
BEGIN
	DECLARE @ID int

	SELECT @ID = ID
	FROM Beneficio
	WHERE Nombre = @NombreBeneficio
	AND
	Tipo = @Tipo;

	INSERT INTO ParametrosBeneficio(IDBeneficio, Nombre, TipoDeDatoParametro, ValorDelParametro)
    VALUES
	(@ID, @NombreParametro, @TipoDeDatoParametro, @ValorDelParametro);
END;
GO


EXEC RegistrarParametro
	@NombreParametro = 'Fecha Nacimiento',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'MediSeguro',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Genero',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'MediSeguro',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Cantidad Dependientes',
	@TipoDeDatoParametro = 'INT',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'MediSeguro',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Nombre de la Aso',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'Asociacion Solidarista',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Salario del empleado',
	@TipoDeDatoParametro = 'INT',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'Asociacion Solidarista',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Date of Birth',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'Poliza de Vida',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Sex',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'Poliza de Vida',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'Cedula',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'TSE',
	@Tipo = 'API';
GO

EXEC RegistrarParametro
	@NombreParametro = 'LegalID',
	@TipoDeDatoParametro = 'STRING',
	@ValorDelParametro = 0,
	@NombreBeneficio = 'Registro Nacional',
	@Tipo = 'API';
GO
