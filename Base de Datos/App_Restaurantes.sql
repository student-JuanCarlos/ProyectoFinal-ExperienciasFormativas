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
	CostoTotal AS (10 * CantidadPersonas),
	Estado INT DEFAULT 1 CHECK(Estado IN (1,2,3)), -- 1 = Pendiente, 2 = Concluido, 3 = Cancelado
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(IdUsuario)
);

CREATE TABLE DetalleMesa(
	IdDetalleMesa INT IDENTITY(1,1) PRIMARY KEY,
	IdReserva INT NOT NULL FOREIGN KEY REFERENCES Reserva(IdReserva),
	IdMesa INT NOT NULL FOREIGN KEY REFERENCES Mesa(IdMesa),
	EspacioTotal INT NOT NULL
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

------------------------------------
--------SP DE Venta
------------------------------------