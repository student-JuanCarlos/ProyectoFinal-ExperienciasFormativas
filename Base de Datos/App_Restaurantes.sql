USE master;
GO
IF EXISTS(SELECT name FROM sys.databases WHERE name = 'App_Restaurantes')
BEGIN
	ALTER DATABASE App_Restaurantes SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE App_Restaurantes;
END

GO
CREATE DATABASE App_Restaurantes

GO
USE App_Restaurantes;

GO

------------------------------------
--------TABLAS INDEPENDIENTES
------------------------------------
CREATE TABLE Rol(
	IdRol INT IDENTITY(1,1) PRIMARY KEY,
	NombreRol VARCHAR(50) NOT NULL,
	Descripcion VARCHAR(150) NULL
);

CREATE TABLE Cargo(
	IdCargo INT IDENTITY(1,1) PRIMARY KEY,
	NombreCargo VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(150) NULL
);

CREATE TABLE Mesa(
	IdMesa INT IDENTITY(1,1) PRIMARY KEY,
	NumeroMesa INT NOT NULL,
	EspacioOcupable INT NOT NULL,
	Estado INT DEFAULT 1 CHECK(Estado IN (1,2,3)), -- 1 = Libre, 2 = Pendiente, 3 = Ocupado
);

CREATE TABLE Categoria(
	IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
	NombreCategoria VARCHAR(50) NOT NULL,
	Descripcion VARCHAR(150) NULL
);

CREATE TABLE ConfiguracionReserva (
    IdConfiguracion INT PRIMARY KEY CHECK(IdConfiguracion = 1),
    PrecioReserva   DECIMAL(10,2) NOT NULL
);

CREATE TABLE Cliente(
	IdCliente INT IDENTITY(1,1) PRIMARY KEY,
	Nombres VARCHAR(100) NOT NULL,
	Apellidos VARCHAR(100) NOT NULL,
	Fotografia VARCHAR(255) NOT NULL,
	Documento VARCHAR(100) NOT NULL UNIQUE,
	Telefono VARCHAR(50) NOT NULL, 
	Email VARCHAR(150) NOT NULL UNIQUE,
	Contraseña VARCHAR(255) NOT NULL,
);

------------------------------------
--------TABLAS DEPENDIENTES
------------------------------------
CREATE TABLE Usuario(
	IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
	NombreUsuario VARCHAR(100) NOT NULL,
	Documento VARCHAR(50) NOT NULL,
	Telefono VARCHAR(50) NOT NULL,
	FechaRegistro DATE DEFAULT GETDATE(),
	Email VARCHAR(150) NOT NULL UNIQUE,
	Contraseña VARCHAR(255) NOT NULL,
	Sueldo DECIMAL(6,2) NOT NULL,
	Estado BIT DEFAULT 1,
	IdCargo INT NOT NULL FOREIGN KEY REFERENCES Cargo(IdCargo),
	IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol(IdRol)
);

CREATE TABLE Descuento(
	IdDescuento INT IDENTITY(1,1) PRIMARY KEY,
	NombreDescuento VARCHAR(150) NOT NULL,
	TipoDescuento VARCHAR(50) NOT NULL CHECK(TipoDescuento IN ('Sin Fecha', 'Con Fecha')), --No todas los descuentos tendran fecha Ejm: Cumpleaños, Casamiento, etc.
	PorcentajeDescuento DECIMAL(4,2) NOT NULL,
	FechaInicio DATE NULL,
	FechaFin DATE NULL,
	ColorCard VARCHAR(30) DEFAULT 'white',
	Estado BIT DEFAULT 1
);

CREATE TABLE Platillo(
	IdPlatillo INT IDENTITY(1,1) PRIMARY KEY,
	NombrePlatillo VARCHAR(150) NOT NULL,
	Fotografia VARCHAR(255) NOT NULL,
	Precio DECIMAL(5,2) NOT NULL,
	IdCategoria INT NULL FOREIGN KEY REFERENCES Categoria(IdCategoria)
);

CREATE TABLE Reserva(
	IdReserva INT IDENTITY(1,1) PRIMARY KEY,
	IdCliente INT NULL FOREIGN KEY REFERENCES Cliente(IdCliente),				-- Al manejar 2 webs distintas (una para clientes y otra interna del sistema) 
	TipoReserva VARCHAR(20) NOT NULL CHECK(TipoReserva IN ('Directa', 'Web')),	--un cliente puede hacer su misma reserva mediante la web, si va directo, el trabajador se encarga de registrar la reserva
	NombreCliente VARCHAR(100) NULL,
	TelefonoCliente VARCHAR(100) NULL,
	FechaReserva DATE NOT NULL,
	HoraReserva TIME NOT NULL,
	CantidadPersonas INT NOT NULL,
	CostoTotal DECIMAL (10,2) DEFAULT 0,
	Estado INT DEFAULT 1 CHECK(Estado IN (1,2,3)), -- 1 = Pendiente, 2 = Concluido, 3 = Cancelado
	IdUsuario INT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario),
	CONSTRAINT ValidacionSesion CHECK(
		IdCliente IS NOT NULL OR IdUsuario IS NOT NULL
	),
	CONSTRAINT DF_Reserva_HoraReserva 
		DEFAULT CAST(GETDATE() AS TIME) FOR HoraReserva,
	CONSTRAINT DF_Reserva_FechaReserva 
		DEFAULT GETDATE() FOR FechaReserva
);

CREATE TABLE DetalleReserva(
	IdDetalleMesa INT IDENTITY(1,1) PRIMARY KEY,
	IdReserva INT NOT NULL FOREIGN KEY REFERENCES Reserva(IdReserva),
	IdMesa INT NOT NULL FOREIGN KEY REFERENCES Mesa(IdMesa),
);

CREATE TABLE Venta(
	IdVenta INT IDENTITY(1,1) PRIMARY KEY,
	IdCliente INT NULL FOREIGN KEY REFERENCES Cliente(IdCliente),
	IdReserva INT NOT NULL FOREIGN KEY REFERENCES Reserva(IdReserva),
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario),
	NombreCliente VARCHAR(150) NULL,
	FechaVenta DATE DEFAULT GETDATE(),
	MetodoPago VARCHAR(50) NOT NULL,
	Total DECIMAL(6,2) NOT NULL,
	CONSTRAINT ValidacionCliente CHECK(
		IdCliente IS NOT NULL OR NombreCliente IS NOT NULL
	)
);

CREATE TABLE DetalleVenta(
	IdDetalleVenta INT IDENTITY(1,1) PRIMARY KEY,
	IdVenta INT NOT NULL FOREIGN KEY REFERENCES Venta(IdVenta),
	IdPlatillo INT NOT NULL FOREIGN KEY REFERENCES Platillo(IdPlatillo),
	Cantidad INT NOT NULL,
	PrecioUnitario DECIMAL (5,2) NOT NULL,
	SubTotal AS (Cantidad * PrecioUnitario)
);

CREATE TABLE DetalleDescuento(
	IdDetalleDescuento INT IDENTITY(1,1) PRIMARY KEY,
	IdVenta INT NOT NULL FOREIGN KEY REFERENCES Venta(IdVenta),
	IdDescuento INT NOT NULL FOREIGN KEY REFERENCES Descuento(IdDescuento),
	DescuentoUnitario DECIMAL(4,2) NOT NULL 
);

------------------------------------
--------SP DE Rol
------------------------------------
GO
CREATE PROC sp_InsertarRol
@NombreRol VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Rol(NombreRol, Descripcion) 
	VALUES(@NombreRol, @Descripcion)
END

GO
CREATE PROC sp_ActualizarRol
@IdRol INT,
@NombreRol VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Rol
	SET NombreRol = @NombreRol,
	Descripcion = @Descripcion
	WHERE IdRol = @IdRol
END

GO
CREATE PROC sp_DetalleRol
@IdRol INT
AS
BEGIN
	SELECT 
	IdRol,
	NombreRol,
	ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Rol
	WHERE IdRol = @IdRol
END

GO
CREATE PROC sp_ListadoRol 
@Busqueda VARCHAR(50) = NULL
AS
BEGIN
	SELECT 
	IdRol,
	NombreRol,
	ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Rol
	WHERE (@Busqueda IS NULL OR NombreRol LIKE '%'+@Busqueda+'%')
END

------------------------------------
--------SP DE Cargo
------------------------------------
GO
CREATE PROC sp_InsertarCargo
@NombreCargo VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Cargo(NombreCargo, Descripcion) VALUES(@NombreCargo, @Descripcion)
END

GO
CREATE PROC sp_ActualizarCargo
@IdCargo INT,
@NombreCargo VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Cargo
	SET NombreCargo = @NombreCargo,
	Descripcion = @Descripcion 
	WHERE IdCargo = @IdCargo
END

GO
CREATE PROC sp_DetalleCargo
@IdCargo INT
AS
BEGIN
	SELECT
		IdCargo,
		NombreCargo,
		ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Cargo
	WHERE IdCargo = @IdCargo
END

GO
CREATE PROC sp_ListadoCargo
@Busqueda VARCHAR(50) = NULL
AS
BEGIN
	SELECT
		IdCargo,
		NombreCargo,
		ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Cargo
	WHERE (@Busqueda IS NULL OR NombreCargo LIKE '%'+@Busqueda+'%')
END
------------------------------------
--------SP DE Mesa
------------------------------------
GO
CREATE PROC	sp_InsertarMesa
@NumeroMesa INT,
@EspacioOcupable INT
AS
BEGIN
	BEGIN TRAN
		SET NOCOUNT ON;
		IF @NumeroMesa IN(SELECT NumeroMesa FROM Mesa)
		BEGIN
			ROLLBACK;
			RAISERROR('Ya existe este numero de mesa', 16, 1);
			RETURN;
		END
		INSERT INTO Mesa(NumeroMesa, EspacioOcupable)
		VALUES(@NumeroMesa, @EspacioOcupable)
	COMMIT
END

GO
CREATE PROC sp_ActualizarMesa
@IdMesa INT,
@NumeroMesa INT,
@EspacioOcupable INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @NumeroMesa IN(SELECT NumeroMesa FROM Mesa WHERE @IdMesa <> IdMesa)
		BEGIN
			ROLLBACK;
			RAISERROR('Ya existe este numero de mesa', 16, 1);
			RETURN;
		END
		UPDATE Mesa
		SET NumeroMesa = @NumeroMesa,
			EspacioOcupable = @EspacioOcupable
			WHERE IdMesa = @IdMesa
	COMMIT
END

GO
CREATE PROC sp_FiltradoMesa
@Busqueda VARCHAR(150)
AS
BEGIN
    SELECT
        m.IdMesa,
        m.NumeroMesa,
        m.EspacioOcupable,
        CASE
            WHEN r.IdReserva IS NULL THEN 1  -- libre
            WHEN r.HoraReserva > CAST(GETDATE() AS TIME) THEN 2  -- pendiente
            ELSE 3  -- ocupada
        END AS Estado,
		c.Nombres+' '+c.Apellidos AS OcupadoPor
    FROM Mesa m
    LEFT JOIN DetalleReserva dm ON dm.IdMesa = m.IdMesa
    LEFT JOIN Reserva r ON dm.IdReserva = r.IdReserva
        AND r.FechaReserva = CAST(GETDATE() AS DATE)
        AND r.Estado = 1
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
		WHERE (@Busqueda IS NULL OR c.Nombres+' '+c.Apellidos LIKE '%'+@Busqueda+'%')
END

GO
CREATE PROC sp_DetalleMesa
@IdMesa INT
AS
BEGIN
    SELECT
        m.IdMesa,
        m.NumeroMesa,
        m.EspacioOcupable,
        m.Estado,
        r.HoraReserva,
        r.FechaReserva,
        c.Nombres+' '+c.Apellidos AS OcupadoPor
    FROM Mesa m
    LEFT JOIN DetalleReserva dm ON dm.IdMesa = m.IdMesa
    LEFT JOIN Reserva r ON dm.IdReserva = r.IdReserva
        AND r.FechaReserva = CAST(GETDATE() AS DATE)
        AND r.Estado = 1
    LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
    WHERE m.IdMesa = @IdMesa
END

------------------------------------
--------SP DE Categoria
------------------------------------
GO
CREATE PROC sp_InsertarCategoria
@NombreCategoria VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Categoria(NombreCategoria, Descripcion)
	VALUES(@NombreCategoria, @Descripcion)
END

GO
CREATE PROC sp_ActualizarCategoria
@IdCategoria INT,
@NombreCategoria VARCHAR(50),
@Descripcion VARCHAR(150) NULL
AS
BEGIN
	UPDATE Categoria
		SET NombreCategoria = @NombreCategoria,
			Descripcion = @Descripcion
			WHERE IdCategoria = @IdCategoria
END

GO
CREATE PROC sp_FiltradoCategoria
@Busqueda VARCHAR(50) = NULL
AS
BEGIN
	SELECT
		IdCategoria,
		NombreCategoria,
		ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Categoria
	WHERE (@Busqueda IS NULL OR NombreCategoria LIKE '%'+ @Busqueda +'%')
END

GO
CREATE PROC sp_DetalleCategoria
@IdCategoria INT
AS
BEGIN
	SELECT
		IdCategoria,
		NombreCategoria,
		ISNULL(Descripcion, 'Sin Descripcion') AS Descripcion
	FROM Categoria
	WHERE IdCategoria = @IdCategoria
END

GO
CREATE PROC sp_EliminarCategoria
@IdCategoria INT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRAN;
		UPDATE Platillo
		SET IdCategoria = NULL
		WHERE IdCategoria = @IdCategoria

		DELETE FROM Categoria
		WHERE IdCategoria = @IdCategoria
	COMMIT;
END
------------------------------------
--------SP DE Cliente
------------------------------------
GO
CREATE PROC sp_RegistrarCliente
@Nombres VARCHAR(100),
@Apellidos VARCHAR(100),
@Fotografia VARCHAR(255),
@Documento VARCHAR(100),
@Telefono VARCHAR(50),
@Email VARCHAR(150),
@Contraseña VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Cliente(Nombres, Apellidos, Fotografia, Documento, Telefono, Email, Contraseña)
	VALUES(@Nombres, @Apellidos, @Fotografia, @Documento, @Telefono, @Email, @Contraseña)
END

GO
CREATE PROC sp_ActualizarCliente --Solo lo puede hacer el mismo cliente
@IdCliente INT,
@Nombres VARCHAR(100),
@Apellidos VARCHAR(100),
@Fotografia VARCHAR(255),
@Documento VARCHAR(100),
@Telefono VARCHAR(50),
@Email VARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Cliente
	SET Nombres = @Nombres,
		Apellidos = @Apellidos,
		Fotografia = @Fotografia,
		Documento = @Documento,
		Telefono = @Telefono,
		Email = @Email
		WHERE IdCliente = @IdCliente
END

GO
CREATE PROC sp_FiltradoCliente --Listado para el sistema interno
@Busqueda VARCHAR(150)
AS
BEGIN	
	SELECT
		c.IdCliente,
		c.Nombres+' '+Apellidos AS NombreCompleto,
		c.Documento, 
		c.Telefono,
		c.Email
	FROM Cliente c
	INNER JOIN Reserva r ON r.IdCliente = c.IdCliente
	WHERE (@Busqueda IS NULL OR Nombres LIKE '%'+ @Busqueda +'%' OR
	Email LIKE '%'+ @Busqueda +'%' OR Documento LIKE '%'+@Busqueda+'%')
	AND r.FechaReserva = CAST(GETDATE() AS DATE)
END

GO
CREATE PROC sp_DetalleCliente
@IdCliente INT
AS
BEGIN
	SELECT
		IdCliente,
		Nombres,
		Apellidos,
		Fotografia,
		Documento,
		Telefono,
		Email
	FROM Cliente
	WHERE IdCliente = @IdCliente
END

GO 
CREATE PROC sp_LoginCliente
@Email VARCHAR(150),
@Contraseña VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		IdCliente,
		Nombres,
		Apellidos,
		Documento,
		Telefono,
		Email
	FROM Cliente
	WHERE Email = @Email AND Contraseña = @Contraseña
END
------------------------------------
--------SP DE Usuario
------------------------------------
GO
CREATE PROC sp_RegistrarUsuario
@NombreUsuario VARCHAR(100),
@Documento VARCHAR(50),
@Telefono VARCHAR(50),
@Email VARCHAR(150),
@Contraseña VARCHAR(255),
@Sueldo DECIMAL(6,2),
@IdCargo INT,
@IdRol INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Usuario(NombreUsuario, Documento, Telefono, Email, Contraseña, Sueldo, IdCargo, IdRol)
	VALUES(@NombreUsuario, @Documento, @Telefono, @Email, @Contraseña, @Sueldo, @IdCargo, @IdRol)
END

GO
CREATE PROC sp_ActualizarUsuario
@IdUsuario INT,
@NombreUsuario VARCHAR(100),
@Documento VARCHAR(50),
@Telefono VARCHAR(50),
@Email VARCHAR(150),
@Sueldo DECIMAL(5,2),
@IdCargo INT,
@IdRol INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Usuario
	SET NombreUsuario = @NombreUsuario,
		Documento = @Documento,
		Telefono = @Telefono ,
		Email = @Email,
		Sueldo = @Sueldo,
		IdCargo = @IdCargo,
		IdRol = @IdRol
	WHERE IdUsuario = @IdUsuario
END

GO
CREATE PROC sp_FiltradoUsuario
@Busqueda VARCHAR(150) = NULL,
@Estado BIT = NULL
AS
BEGIN
	SELECT
		u.IdUsuario,
		u.NombreUsuario,
		u.Telefono,
		c.NombreCargo,
		r.NombreRol,
		u.Email,
		u.Estado
	FROM Usuario u
		INNER JOIN Cargo c ON c.IdCargo = u.IdCargo
		INNER JOIN Rol r ON r.IdRol = u.IdRol
		WHERE (@Busqueda IS NULL OR NombreUsuario LIKE '%'+@Busqueda+'%' OR Email LIKE '%'+@Busqueda+'%')
		AND (@Estado IS NULL OR Estado = @Estado)
END

GO
CREATE PROC sp_DetalLeUsuario
@IdUsuario INT
AS
BEGIN
	SELECT
		u.IdUsuario,
		u.IdCargo,
		u.IdRol,
		u.NombreUsuario,
		u.Documento,
		u.Telefono,
		u.FechaRegistro,
		c.NombreCargo,
		r.NombreRol,
		u.Email,
		u.Sueldo,
		u.Estado
	FROM Usuario u
	INNER JOIN Cargo c ON c.IdCargo = u.IdCargo
	INNER JOIN Rol r ON r.IdRol = u.IdRol
	WHERE IdUsuario = @IdUsuario
END

GO
CREATE PROC sp_LoginUsuario
@Email VARCHAR(150),
@Contraseña VARCHAR(255)
AS
BEGIN
	SELECT
		u.IdUsuario,
		u.IdCargo,
		u.IdRol,
		u.NombreUsuario,
		u.Telefono,
		c.NombreCargo,
		r.NombreRol,
		u.Email,
		u.Estado
	FROM Usuario u
		INNER JOIN Cargo c ON c.IdCargo = u.IdCargo
		INNER JOIN Rol r ON r.IdRol = u.IdRol
		WHERE u.Email = @Email AND u.Contraseña = @Contraseña
END

GO
CREATE PROC sp_CambiarEstadoUsuario
@IdUsuario INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Usuario
	SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
	WHERE IdUsuario = @IdUsuario
END
------------------------------------
--------SP DE Descuento
------------------------------------
GO
CREATE PROC sp_InsertarDescuento
@NombreDescuento VARCHAR(150),
@TipoDescuento VARCHAR(50),
@PorcentajeDescuento DECIMAL(4,2),
@FechaInicio DATE NULL,
@FechaFin DATE NULL,
@ColorCard VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Descuento(NombreDescuento, TipoDescuento, PorcentajeDescuento, FechaInicio, FechaFin, ColorCard)
	VALUES(@NombreDescuento, @TipoDescuento, @PorcentajeDescuento, @FechaInicio, @FechaFin, @ColorCard)
END

GO
CREATE PROC sp_ActualizarDescuento
@IdDescuento INT,
@NombreDescuento VARCHAR(150),
@TipoDescuento VARCHAR(50),
@PorcentajeDescuento DECIMAL(4,2),
@FechaInicio DATE NULL,
@FechaFin DATE NULL,
@ColorCard VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Descuento
	SET NombreDescuento = @NombreDescuento,
		TipoDescuento = @TipoDescuento,
		PorcentajeDescuento = @PorcentajeDescuento,
		FechaInicio = @FechaInicio,
		FechaFin = @FechaFin,
		ColorCard = @ColorCard
		WHERE IdDescuento = @IdDescuento
END

GO
CREATE PROC sp_FiltradoDescuento
@Busqueda VARCHAR(150),
@Estado BIT 
AS
BEGIN
	SELECT 
		de.IdDescuento,
		de.NombreDescuento,
		de.TipoDescuento,
		de.PorcentajeDescuento,
		de.ColorCard,
		de.Estado
	FROM Descuento de
	WHERE (@Busqueda IS NULL OR de.NombreDescuento LIKE '%'+@Busqueda+'%')
	AND (@Estado IS NULL OR de.Estado = @Estado)
END

GO
CREATE PROC sp_DetalleDescuento
@IdDescuento INT
AS
BEGIN
	SELECT 
		de.NombreDescuento,
		de.TipoDescuento,
		de.PorcentajeDescuento,
		de.FechaInicio,
		de.FechaFin,
		de.ColorCard,
		de.Estado
	FROM Descuento de
	WHERE de.IdDescuento = @IdDescuento
END

GO
CREATE PROC sp_CambiarEstadoDescuento
@IdDescuento INT
AS
BEGIN	
	SET NOCOUNT ON;
	UPDATE Descuento
	SET Estado = CASE WHEN Estado = 1 THEN  0 ELSE 1 END
	WHERE IdDescuento = @IdDescuento
END
------------------------------------
--------SP DE Platillo
------------------------------------
GO
CREATE PROC sp_InsertarPlatillo
@NombrePlatillo VARCHAR(150),
@Fotografia VARCHAR(255),
@Precio DECIMAL (5,2),
@IdCategoria INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Platillo(NombrePlatillo, Fotografia, Precio, IdCategoria)
	VALUES(@NombrePlatillo, @Fotografia, @Precio, @IdCategoria)
END

GO
CREATE PROC sp_ActualizarPlatillo
@IdPlatillo INT,
@NombrePlatillo VARCHAR(150),
@Fotografia VARCHAR(255),
@Precio DECIMAL (5,2),
@IdCategoria INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Platillo
	SET NombrePlatillo = @NombrePlatillo,
		Fotografia = @Fotografia,
		Precio = @Precio,
		IdCategoria = @IdCategoria
	WHERE IdPlatillo = @IdPlatillo
END

GO
CREATE PROC sp_FiltradoPlatillo
@Busqueda VARCHAR(150) = NULL
AS
BEGIN
	SELECT 
		p.IdPlatillo,
		p.IdCategoria,
		p.NombrePlatillo,
		p.Fotografia,
		c.NombreCategoria,
		p.Precio
	FROM Platillo p
	INNER JOIN Categoria c ON c.IdCategoria = p.IdCategoria
	WHERE (@Busqueda IS NULL OR NombrePlatillo LIKE '%'+@Busqueda+'%' OR c.NombreCategoria LIKE '%'+@Busqueda+'%')
END

GO
CREATE PROC sp_DetallePlatillo
@IdPlatillo INT
AS
BEGIN
	SELECT 
		p.IdPlatillo,
		p.IdCategoria,
		p.NombrePlatillo,
		p.Fotografia,
		c.NombreCategoria,
		p.Precio
	FROM Platillo p
	INNER JOIN Categoria c ON c.IdCategoria = p.IdCategoria
	WHERE IdPlatillo = @IdPlatillo
END

------------------------------------
--------SP DE Reserva
------------------------------------
GO
CREATE TYPE TVP_Mesas AS TABLE(
    IdMesa INT
);
GO
CREATE PROC sp_InsertarReserva
@IdCliente        INT NULL,
@TipoReserva      VARCHAR(20),
@NombreCliente	  VARCHAR(100) = NULL,
@TelefonoCliente  VARCHAR(100) = NULL,
@FechaReserva     DATE = NULL,
@HoraReserva      TIME = NULL, --Solo para que tome la hora exacta al momento del registro
@CantidadPersonas INT,
@IdUsuario        INT NULL,
@Mesas            TVP_Mesas READONLY
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CostoTotal DECIMAL(10,2) = 0;
    DECLARE @IdReserva  INT;

    BEGIN TRY
        BEGIN TRAN

        IF (SELECT SUM(m.EspacioOcupable) 
            FROM @Mesas ms
            INNER JOIN Mesa m ON m.IdMesa = ms.IdMesa) < @CantidadPersonas
        BEGIN
            RAISERROR('Espacio insuficiente para la cantidad de personas', 16, 1);
            ROLLBACK;
            RETURN;
        END

        IF @TipoReserva = 'Web'
        BEGIN
            DECLARE @Precio DECIMAL(10,2);
            SELECT @Precio = PrecioReserva FROM ConfiguracionReserva WHERE IdConfiguracion = 1;
            SET @CostoTotal = @Precio * @CantidadPersonas;
        END

        INSERT INTO Reserva(IdCliente, TipoReserva, NombreCliente, TelefonoCliente, FechaReserva, HoraReserva, CantidadPersonas, CostoTotal, IdUsuario)
        VALUES(@IdCliente, @TipoReserva, @NombreCliente, @TelefonoCliente, @FechaReserva, @HoraReserva, @CantidadPersonas, @CostoTotal, @IdUsuario);
        SET @IdReserva = SCOPE_IDENTITY();

        INSERT INTO DetalleReserva(IdReserva, IdMesa)
        SELECT @IdReserva, ms.IdMesa
        FROM @Mesas ms;

        IF @FechaReserva = CAST(GETDATE() AS DATE)
		BEGIN
			UPDATE Mesa SET Estado = 2
			WHERE IdMesa IN (SELECT IdMesa FROM @Mesas)
		END

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END

GO
CREATE PROC sp_ActualizarReserva 
@IdReserva INT,
@NombreCliente VARCHAR(100),
@TelefonoCliente VARCHAR(100),
@CantidadPersonas INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Reserva
	SET NombreCliente = @NombreCliente,
		TelefonoCliente = @TelefonoCliente,
		CantidadPersonas = @CantidadPersonas,
		CostoTotal = CASE
						WHEN TipoReserva = 'Web'
						THEN @CantidadPersonas * (SELECT PrecioReserva FROM ConfiguracionReserva WHERE IdConfiguracion = 1)
						ELSE 0
					 END
		WHERE IdReserva = @IdReserva
END

GO
CREATE PROC sp_CancelarReserva
@IdReserva INT,
@Mesas TVP_Mesas READONLY
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRAN
			UPDATE Reserva
			SET Estado = 3
			WHERE Estado = 1 AND IdReserva = @IdReserva

			UPDATE Mesa
			SET Estado = 1
			WHERE IdMesa IN (SELECT IdMesa FROM @Mesas)
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

GO
CREATE PROC sp_ActualizarMesas
@IdReserva INT,
@Mesas TVP_Mesas READONLY
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRAN
			
			IF (SELECT SUM(m.EspacioOcupable)
			FROM @Mesas ms
			INNER JOIN Mesa m ON m.IdMesa = ms.IdMesa) < (
				SELECT CantidadPersonas FROM Reserva WHERE IdReserva = @IdReserva
			)
			BEGIN
				RAISERROR('Espacio insuficiente para la cantidad de personas', 16, 1);
				ROLLBACK;
				RETURN;
			END
			
			UPDATE Mesa
			SET Estado = 1
			WHERE IdMesa IN (SELECT IdMesa FROM DetalleReserva WHERE IdReserva = @IdReserva) AND IdMesa NOT IN (SELECT IdMesa FROM @Mesas)

			DELETE FROM DetalleReserva
			WHERE IdReserva = @IdReserva

			INSERT INTO DetalleReserva(IdReserva, IdMesa)
			SELECT @IdReserva, ms.IdMesa
			FROM @Mesas ms;

			UPDATE Mesa 
			SET Estado = 2
			WHERE IdMesa IN (SELECT IdMesa FROM @Mesas) AND EXISTS
			(SELECT 1 FROM Reserva WHERE IdReserva = @IdReserva AND FechaReserva = CAST(GETDATE() AS DATE))
		COMMIT
    END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

GO
CREATE PROC sp_DetalleReserva
@IdReserva INT
AS
BEGIN
	SELECT 
		ISNULL(c.Nombres, u.NombreUsuario) AS GeneradoPor,
		r.TipoReserva,
		r.FechaReserva,
		r.HoraReserva,
		r.CantidadPersonas,
		r.CostoTotal
	FROM Reserva r
	LEFT JOIN Usuario u ON u.IdUsuario = r.IdUsuario
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
	WHERE IdReserva = @IdReserva

	SELECT 
		m.NumeroMesa
	FROM Mesa m
	INNER JOIN DetalleReserva dr ON dr.IdMesa = m.IdMesa
	WHERE dr.IdReserva = @IdReserva

	SELECT 
		c.IdCliente,
		c.Nombres+' '+c.Apellidos AS NombreCompleto,
		c.Fotografia,
		c.Telefono,
		c.Email,
		c.Documento
	FROM Cliente c
	LEFT JOIN Reserva r ON r.IdCliente = c.IdCliente
	LEFT JOIN DetalleReserva dr ON dr.IdReserva = r.IdReserva
	WHERE r.IdReserva = @IdReserva
END

GO
CREATE PROC sp_FiltradoReservas
@Busqueda VARCHAR(150),
@Estado INT
AS
BEGIN
	SELECT
		r.IdReserva,
		r.TipoReserva,
		ISNULL(r.NombreCliente, c.Nombres+' '+c.Apellidos) AS Cliente,
		r.FechaReserva,
		r.HoraReserva,
		r.CantidadPersonas,
		r.CostoTotal,
		r.Estado
	FROM Reserva r
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
	WHERE (@Busqueda IS NULL OR r.TipoReserva = @Busqueda
	OR c.Nombres+' '+c.Apellidos LIKE '%'+@Busqueda+'%') AND 
	(@Estado IS NULL OR r.Estado = @Estado)
END
GO 
------------------------------------
--------SP DE Venta
------------------------------------

CREATE TYPE TVP_DetalleVenta AS TABLE(
	IdPlatillo INT,
	Cantidad INT
);

CREATE TYPE TVP_DetalleDescuento AS TABLE(
	IdDescuento INT NULL,
	PorcentajeAplicado DECIMAL (4,2) NULL
);
GO
CREATE PROC sp_RegistrarVenta
@IdCliente INT NULL,
@IdReserva INT,
@IdUsuario INT,
@NombreCliente VARCHAR(50) NULL,
@MetodoPago VARCHAR(50),
@Detalle TVP_DetalleVenta READONLY,
@Descuento TVP_DetalleDescuento READONLY
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IdVenta INT;
    BEGIN TRY
        BEGIN TRAN

        IF NOT EXISTS(SELECT 1 FROM @Detalle)
        BEGIN
            RAISERROR('La venta no contiene platillos', 16, 1);
            ROLLBACK;
            RETURN;
        END

        INSERT INTO Venta(IdCliente, IdReserva, IdUsuario, NombreCliente, MetodoPago, Total)
        VALUES(@IdCliente, @IdReserva, @IdUsuario, @NombreCliente, @MetodoPago, 0);
        SET @IdVenta = SCOPE_IDENTITY();

        INSERT INTO DetalleVenta(IdVenta, IdPlatillo, Cantidad, PrecioUnitario)
        SELECT @IdVenta, d.IdPlatillo, d.Cantidad, p.Precio
        FROM @Detalle d
        INNER JOIN Platillo p ON p.IdPlatillo = d.IdPlatillo;

        INSERT INTO DetalleDescuento(IdVenta, IdDescuento, DescuentoUnitario)
        SELECT @IdVenta, de.IdDescuento, de.PorcentajeAplicado
        FROM @Descuento de
        WHERE IdDescuento IS NOT NULL;

        UPDATE Venta
        SET Total = (SELECT SUM(SubTotal) FROM DetalleVenta WHERE IdVenta = @IdVenta)
                  + (SELECT CostoTotal FROM Reserva WHERE IdReserva = @IdReserva)
        WHERE IdVenta = @IdVenta;

        UPDATE Venta
        SET Total = Total * (1 - (SELECT ISNULL(SUM(PorcentajeAplicado), 0) / 100 FROM @Descuento))
        WHERE IdVenta = @IdVenta AND EXISTS(SELECT 1 FROM @Descuento WHERE IdDescuento IS NOT NULL);

        UPDATE Mesa SET Estado = 1
        WHERE IdMesa IN (SELECT IdMesa FROM DetalleReserva WHERE IdReserva = @IdReserva);

        UPDATE Reserva SET Estado = 2
        WHERE IdReserva = @IdReserva;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
		
GO
CREATE PROC sp_DetalleVenta
@IdVenta INT
AS
BEGIN
	SELECT 
		ISNULL(c.Nombres+' '+c.Apellidos, v.NombreCliente) AS NombreCompleto,
		r.TipoReserva,
		c.Email,
		r.CantidadPersonas,
		r.CostoTotal AS CostoReserva,
		v.FechaVenta,
		v.MetodoPago,
		u.NombreUsuario
	FROM Venta v
	LEFT JOIN Cliente c ON c.IdCliente = v.IdCliente
	INNER JOIN Usuario u ON u.IdUsuario = v.IdUsuario
	INNER JOIN Reserva r ON r.IdReserva = v.IdReserva
	WHERE IdVenta = @IdVenta

	SELECT 
		p.NombrePlatillo,
		d.Cantidad,
		d.PrecioUnitario
	FROM DetalleVenta d
	INNER JOIN Platillo p ON p.IdPlatillo = d.IdPlatillo
	WHERE IdVenta = @IdVenta

	SELECT 
		de.NombreDescuento,
		de.ColorCard,
		dd.DescuentoUnitario
	FROM DetalleDescuento dd
	INNER JOIN Descuento de ON de.IdDescuento = dd.IdDescuento
	WHERE IdVenta = @IdVenta

END

GO
CREATE PROC sp_FiltradoVentas
@Busqueda VARCHAR(100)
AS
BEGIN
	SELECT
		v.IdVenta,
		ISNULL(c.Nombres + ' ' + c.Apellidos, v.NombreCliente) AS NombreCompleto,
		v.FechaVenta,
		v.MetodoPago,
		v.Total
	FROM Venta v
	LEFT JOIN Cliente c ON c.IdCliente = v.IdCliente
	WHERE (@Busqueda IS NULL OR ISNULL(c.Nombres+' '+ c.Apellidos, v.NombreCliente) LIKE '%'+ @Busqueda +'%')
END

------------------------------------
--------INSERCIONES BASICAS
------------------------------------
INSERT INTO ConfiguracionReserva VALUES(1, 10.00);
INSERT INTO Rol (NombreRol) VALUES('Administrador');
INSERT INTO Rol (NombreRol)VALUES('Trabajador');

------------------------------------
--------INSERCIONES BASICAS
------------------------------------
SELECT * FROM Cliente;
SELECT * FROM Mesa;
SELECT * FROM ConfiguracionReserva;
SELECT * FROM Descuento;
SELECT * FROM Rol;
SELECT * FROM Reserva;

------------------------------------
--------Control interno del sistema
------------------------------------
GO
CREATE PROC sp_PrecioReserva 
@Precio DECIMAL(5,2)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE ConfiguracionReserva
	SET PrecioReserva = @Precio
END

GO
CREATE PROC sp_ActualizarEstadoMesasHoy
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Mesa 
    SET Estado = 1 -- Libre
    WHERE Estado = 2 
    AND IdMesa IN (    
        SELECT dm.IdMesa 
        FROM DetalleReserva dm    
        INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva    
        WHERE r.FechaReserva < CAST(GETDATE() AS DATE)    
        AND r.Estado = 1  
    )
    AND IdMesa NOT IN (
        SELECT dm2.IdMesa 
        FROM DetalleReserva dm2    
        INNER JOIN Reserva r2 ON r2.IdReserva = dm2.IdReserva    
        WHERE r2.FechaReserva = CAST(GETDATE() AS DATE)    
        AND r2.Estado = 1
    );

    UPDATE Mesa 
    SET Estado = 2 -- Pendiente
    WHERE Estado = 1 
    AND IdMesa IN (    
        SELECT dm.IdMesa 
        FROM DetalleReserva dm    
        INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva    
        WHERE r.FechaReserva = CAST(GETDATE() AS DATE)    
        AND r.Estado = 1  
    );
END
