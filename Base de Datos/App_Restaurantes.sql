USE master;
GO
IF EXISTS(SELECT name FROM sys.databases WHERE name = 'App_Restaurantes')
BEGIN
	ALTER DATABASE App_Restaurantes SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE App_Restaurantes;
END

GO
CREATE DATABASE App_Restaurantes

USE App_Restaurantes;

GO

------------------------------------
--------TABLAS INDEPENDIENTES
------------------------------------
CREATE TABLE Rol(
	IdRol INT IDENTITY(1,1) PRIMARY KEY,
	NombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Cargo(
	IdCargo INT IDENTITY(1,1) PRIMARY KEY,
	NombreCargo VARCHAR(100) NOT NULL
);

CREATE TABLE Mesa(
	IdMesa INT IDENTITY(1,1) PRIMARY KEY,
	NumeroMesa INT NOT NULL,
	EspacioOcupable INT NOT NULL,
	Estado BIT DEFAULT 1
);

CREATE TABLE Categoria(
	IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
	NombreCategoria VARCHAR(50) NOT NULL
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
	Documento VARCHAR(255) NOT NULL UNIQUE,
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
	IdCategoria INT NOT NULL FOREIGN KEY REFERENCES Categoria(IdCategoria)
);

CREATE TABLE Reserva(
	IdReserva INT IDENTITY(1,1) PRIMARY KEY,
	IdCliente INT NULL FOREIGN KEY REFERENCES Cliente(IdCliente),				-- Al manejar 2 webs distintas (una para clientes y otra interna del sistema) 
	TipoReserva VARCHAR(20) NOT NULL CHECK(TipoReserva IN ('Directa', 'Web')),	--un cliente puede hacer su misma reserva mediante la web, si va directo, el trabajador se encarga de registrar la reserva
	FechaReserva DATE NOT NULL,
	HoraReserva TIME NOT NULL,
	CantidadPersonas INT NOT NULL,
	CostoTotal DECIMAL (10,2) DEFAULT 0,
	Estado INT DEFAULT 1 CHECK(Estado IN (1,2,3)), -- 1 = Pendiente, 2 = Concluido, 3 = Cancelado
	IdUsuario INT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario)
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


------------------------------------
--------SP DE Cargo
------------------------------------

------------------------------------
--------SP DE Mesa
------------------------------------

------------------------------------
--------SP DE Categoria
------------------------------------

------------------------------------
--------SP DE Cliente
------------------------------------

------------------------------------
--------SP DE Usuario
------------------------------------

------------------------------------
--------SP DE Descuento
------------------------------------

------------------------------------
--------SP DE Platillo
------------------------------------

------------------------------------
--------SP DE Reserva
------------------------------------
CREATE TYPE TVP_Mesas AS TABLE(
    IdMesa INT
);
GO
CREATE PROC sp_InsertarReserva
@IdCliente        INT NULL,
@TipoReserva      VARCHAR(20),
@FechaReserva     DATE,
@HoraReserva      TIME,
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

        INSERT INTO Reserva(IdCliente, TipoReserva, FechaReserva, HoraReserva, CantidadPersonas, CostoTotal, IdUsuario)
        VALUES(@IdCliente, @TipoReserva, @FechaReserva, @HoraReserva, @CantidadPersonas, @CostoTotal, @IdUsuario);
        SET @IdReserva = SCOPE_IDENTITY();

        INSERT INTO DetalleReserva(IdReserva, IdMesa)
        SELECT @IdReserva, ms.IdMesa
        FROM @Mesas ms;

        UPDATE Mesa
        SET Estado = 0
        WHERE IdMesa IN (SELECT IdMesa FROM @Mesas);

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
@FechaReserva DATE,
@HoraReserva TIME,
@CantidadPersonas INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Reserva
	SET FechaReserva = @FechaReserva,
		HoraReserva = @HoraReserva,
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
	UPDATE Reserva
	SET Estado = 3
	WHERE Estado = 1 AND IdReserva = @IdReserva

	UPDATE Mesa
	SET Estado = 1
	WHERE IdMesa IN (SELECT IdMesa FROM @Mesas)
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
			SET Estado = 0
			WHERE IdMesa IN (SELECT IdMesa FROM @Mesas) 
		COMMIT
    END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

GO
CREATE PROC sp_DetalleReserva


GO
CREATE PROC sp_FiltradoVentas


GO
------------------------------------
--------SP DE Venta
------------------------------------
GO
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
			RAISERROR('La venta no contiene productos', 16, 1);
			ROLLBACK;
			RETURN;
		END

		INSERT INTO Venta(IdCliente, IdReserva, IdUsuario, NombreCliente, MetodoPago)
		VALUES(@IdCliente, @IdReserva, @IdUsuario, @NombreCliente, @MetodoPago);
		SET @IdVenta = SCOPE_IDENTITY();

		INSERT INTO DetalleVenta(IdVenta, IdPlatillo, Cantidad, PrecioUnitario)
		SELECT 
			@IdVenta,
			d.IdPlatillo,
			d.Cantidad,
			p.Precio
		FROM @Detalle d
		INNER JOIN Platillo p ON p.IdPlatillo = d.IdPlatillo

		INSERT INTO DetalleDescuento(IdVenta, IdDescuento, DescuentoUnitario)
		SELECT 
			@IdVenta,
			de.IdDescuento,
			de.PorcentajeAplicado
		FROM @Descuento de
		WHERE IdDescuento IS NOT NULL

		UPDATE Venta 
		SET Total = (
			SELECT 
				SUM(d.SubTotal)
			FROM DetalleVenta d
			WHERE IdVenta = @IdVenta
		) 
		WHERE IdVenta = @IdVenta;

		UPDATE Venta
		SET Total = Total * (1 - (
			SELECT 
				ISNULL(SUM(de.PorcentajeAplicado), 0) / 100
			FROM @Descuento de
		))
		WHERE IdVenta = @IdVenta AND EXISTS(SELECT 1 FROM @Descuento)

		UPDATE Mesa
		SET Estado = 1
		WHERE IdMesa IN (
			SELECT IdMesa FROM DetalleReserva
			WHERE IdReserva = @IdReserva
		)
		
		UPDATE Reserva
		SET Estado = 2 -- Mesa o Mesas libres
		WHERE IdReserva = @IdReserva

		UPDATE Venta 
		SET Total = Total + (
			SELECT r.CostoTotal 
			FROM Reserva r 
			WHERE r.IdReserva = @IdReserva
		)  
		WHERE IdVenta = @IdVenta 

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
		ISNULL(v.IdCliente, 'Venta directa') AS IdCliente,
		ISNULL(c.Nombres+' '+c.Apellidos, v.NombreCliente) AS NombreCompleto,
		v.NombreCliente,
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
		v.NombreCliente,
		c.Nombres + ' ' + c.Apellidos AS NombreCompleto,
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