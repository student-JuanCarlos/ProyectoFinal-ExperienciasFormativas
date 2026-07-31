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
	)
);

CREATE TABLE DetalleReserva(
	IdDetalleReserva INT IDENTITY(1,1) PRIMARY KEY,
	IdReserva INT NOT NULL FOREIGN KEY REFERENCES Reserva(IdReserva),
	IdMesa INT NOT NULL FOREIGN KEY REFERENCES Mesa(IdMesa),
);

CREATE TABLE Venta(
	IdVenta INT IDENTITY(1,1) PRIMARY KEY,
	IdReserva INT NOT NULL FOREIGN KEY REFERENCES Reserva(IdReserva),
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario),
	FechaVenta DATE DEFAULT GETDATE(),
	MetodoPago VARCHAR(50) NOT NULL,
	Total DECIMAL(6,2) NOT NULL
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
--------SP DE Mesa
------------------------------------
GO
CREATE PROC sp_FiltradoMesa
AS
BEGIN
    SELECT
        m.IdMesa,
        m.NumeroMesa,
        m.EspacioOcupable,
        CASE
            WHEN NOT EXISTS (
                SELECT 1 FROM DetalleReserva dm
                INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva
                WHERE dm.IdMesa = m.IdMesa
                AND r.FechaReserva = CAST(GETDATE() AS DATE)
                AND r.Estado = 1
            ) THEN 1  -- libre
            WHEN EXISTS (
				SELECT 1 FROM DetalleReserva dm
				INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva
				WHERE dm.IdMesa = m.IdMesa
				AND r.FechaReserva = CAST(GETDATE() AS DATE)
				AND r.Estado = 1
				AND r.HoraReserva > DATEADD(HOUR, 2, CAST(GETDATE() AS TIME))
				AND r.HoraReserva > CAST(GETDATE() AS TIME)  
			) THEN 2 -- pendiente
            ELSE 3    -- ocupada 
        END AS Estado
    FROM Mesa m
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
        CASE
            WHEN NOT EXISTS (
                SELECT 1 FROM DetalleReserva dm
                INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva
                WHERE dm.IdMesa = m.IdMesa
                AND r.FechaReserva = CAST(GETDATE() AS DATE)
                AND r.Estado = 1
            ) THEN 1 --libre
            WHEN EXISTS (
				SELECT 1 FROM DetalleReserva dm
				INNER JOIN Reserva r ON r.IdReserva = dm.IdReserva
				WHERE dm.IdMesa = m.IdMesa
				AND r.FechaReserva = CAST(GETDATE() AS DATE)
				AND r.Estado = 1
				AND r.HoraReserva > DATEADD(HOUR, 2, CAST(GETDATE() AS TIME))
				AND r.HoraReserva > CAST(GETDATE() AS TIME)  
			) THEN 2 -- pendiente
            ELSE 3 --ocupado
        END AS Estado,
		ISNULL(r.HoraReserva, NULL) AS HoraReserva,
        ISNULL(c.Nombres+' '+c.Apellidos, r.NombreCliente) AS OcupadoPor
    FROM Mesa m
    LEFT JOIN DetalleReserva dm ON dm.IdMesa = m.IdMesa
    LEFT JOIN Reserva r ON r.IdReserva = dm.IdReserva
        AND r.FechaReserva = CAST(GETDATE() AS DATE)
        AND r.Estado = 1
    LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
    WHERE m.IdMesa = @IdMesa
	ORDER BY r.HoraReserva DESC
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

	UPDATE Reserva
	SET Estado = 3
	WHERE Estado = 1 AND FechaReserva < CAST(GETDATE() AS DATE)
END 
------------------------------------
--------SP DE Reserva
------------------------------------
------------RESERVA 
GO
CREATE PROC sp_DetalleReserva_Encabezado
@IdReserva INT
AS
BEGIN
	SELECT 
		r.IdReserva,
		ISNULL(c.Nombres+' '+c.Apellidos, u.NombreUsuario) AS GeneradoPor,
		r.NombreCliente,
		r.TelefonoCliente,
		r.TipoReserva,
		r.FechaReserva,
		r.HoraReserva,
		r.CantidadPersonas,
		r.CostoTotal,
		r.Estado
	FROM Reserva r
	LEFT JOIN Usuario u ON u.IdUsuario = r.IdUsuario
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
	WHERE r.IdReserva = @IdReserva
END
GO

CREATE PROC sp_DetalleReserva_Mesas-----Compartido para Ambos DTO
@IdReserva INT
AS
BEGIN
	SELECT 
		m.IdMesa,
		m.NumeroMesa
	FROM Mesa m
	INNER JOIN DetalleReserva dr ON dr.IdMesa = m.IdMesa
	WHERE dr.IdReserva = @IdReserva
END

GO
CREATE PROC sp_DetalleReserva_Cliente
@IdReserva INT
AS
BEGIN
	SELECT 
		c.IdCliente,
		c.Nombres+' '+c.Apellidos AS NombreCompleto,
		c.Fotografia,
		c.Telefono,
		c.Email,
		c.Documento
	FROM Cliente c
	LEFT JOIN Reserva r ON r.IdCliente = c.IdCliente
	WHERE r.IdReserva = @IdReserva
END
------------
------------CLIENTE
GO
CREATE PROC sp_DetalleReserva_Cliente_Encabezado
@IdReserva INT
AS
BEGIN
	SELECT 
		r.IdReserva,
		r.FechaReserva,
		r.HoraReserva,
		r.CantidadPersonas,
		r.CostoTotal,
		r.Estado
	FROM Reserva r
	WHERE r.IdReserva = @IdReserva
END
------------
------------------------------------
--------SP DE Venta
------------------------------------
GO
CREATE TYPE TVP_DetalleVenta AS TABLE(
	IdPlatillo INT,
	Cantidad INT
);
GO
CREATE TYPE TVP_DetalleDescuento AS TABLE(
	IdDescuento INT NULL,
	PorcentajeAplicado DECIMAL (4,2) NULL
);
GO
CREATE PROC sp_RegistrarVenta
@IdReserva INT,
@IdUsuario INT,
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

        INSERT INTO Venta(IdReserva, IdUsuario, MetodoPago, Total)
        VALUES(@IdReserva, @IdUsuario, @MetodoPago, 0);
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
		v.IdVenta,
		ISNULL(c.Nombres+' '+c.Apellidos, r.NombreCliente) AS NombreCompleto,
		r.TipoReserva,
		ISNULL(c.Email, r.TelefonoCliente) AS Contacto,
		r.CantidadPersonas,
		r.CostoTotal,
		v.FechaVenta,
		v.MetodoPago,
		v.Total,
		u.NombreUsuario
	FROM Venta v
	LEFT JOIN Usuario u ON u.IdUsuario = v.IdUsuario
	LEFT JOIN Reserva r ON r.IdReserva = v.IdReserva
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
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
	LEFT JOIN Descuento de ON de.IdDescuento = dd.IdDescuento
	WHERE IdVenta = @IdVenta
END

GO
CREATE PROC sp_FiltradoVentas
@Busqueda VARCHAR(100)
AS
BEGIN
	SELECT
		v.IdVenta,
		ISNULL(c.Nombres + ' ' + c.Apellidos, r.NombreCliente) AS NombreCompleto,
		v.FechaVenta,
		v.MetodoPago,
		v.Total
	FROM Venta v
	LEFT JOIN Reserva r ON r.IdReserva = v.IdReserva
	LEFT JOIN Cliente c ON c.IdCliente = r.IdCliente
	WHERE (@Busqueda IS NULL OR ISNULL(c.Nombres+' '+ c.Apellidos, r.NombreCliente) LIKE '%'+ @Busqueda +'%')
END

------------------------------------
--------INSERCIONES BASICAS
------------------------------------
INSERT INTO ConfiguracionReserva VALUES(1, 10.00);
INSERT INTO Rol (NombreRol) VALUES('Administrador');
INSERT INTO Rol (NombreRol)VALUES('Trabajador');
INSERT INTO Cargo (NombreCargo) VALUES('Admin');

INSERT INTO Usuario(NombreUsuario, Documento, Telefono, Email, Contraseña, Sueldo, IdCargo, IdRol) VALUES('useradmin', 11111111, 2222222, 'useradmin@gmail.com', 123, 1200.00, 1, 1)
------------------------------------
--------SELECTS
------------------------------------
SELECT * FROM Cliente;
SELECT * FROM Mesa;
SELECT * FROM ConfiguracionReserva;
SELECT * FROM Descuento;
SELECT * FROM Rol;
SELECT * FROM Reserva;
SELECT * FROM DetalleReserva;
SELECT * FROM Usuario;
SELECT * FROM Venta;
SELECT * FROM DetalleDescuento;
SELECT * FROM DetalleVenta;
SELECT * FROM Cargo;
SELECT * FROM Categoria;
SELECT * FROM Platillo;
------------------------------------
--------INDICES
------------------------------------
CREATE INDEX IDX_Ciente_Id ON Cliente(IdCliente);
CREATE INDEX IDX_Mesa_Id ON Mesa(IdMesa);
CREATE INDEX IDX_Descuento_Id ON Descuento(IdDescuento);
CREATE INDEX IDX_Reserva_Id ON Reserva(IdReserva);
CREATE INDEX IDX_DetalleReserva ON DetalleReserva(IdDetalleMesa);
CREATE INDEX IDX_Usuario_Id ON Usuario(IdUsuario);
CREATE INDEX IDX_Venta_Id ON Venta(IdVenta);
CREATE INDEX IDX_DetalleVenta_Id ON DetalleVenta(IdDetalleVenta);
CREATE INDEX IDX_DetalleDescuento_Id ON DetalleDescuento(IdDetalleDescuento);
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
