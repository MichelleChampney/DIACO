CREATE DATABASE DIACO
GO 
USE DIACO
GO

CREATE TABLE Roles(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	CONSTRAINT PK_Roles PRIMARY KEY (Id)
)
GO

CREATE TABLE Usuarios(
	Id int identity not null,
	Nombre nvarchar(50) not null,
	Usuario nvarchar(50) not null,
	Clave nvarchar(100) not null,
	Salt varbinary(20) not null,
	Activo bit not null,
	IdRol int not null,
	CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
	CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles (Id)
)
GO

CREATE TABLE Regiones(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	CONSTRAINT PK_Regiones PRIMARY KEY (Id)
)
GO

CREATE TABLE Departamentos(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	IdRegion int not null,
	CONSTRAINT PK_Departamentos PRIMARY KEY (Id),
	CONSTRAINT FK_Departamentos_Regiones FOREIGN KEY (IdRegion) REFERENCES Regiones (Id)
)
GO

CREATE TABLE Municipios(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	IdDepartamento int not null,
	CONSTRAINT PK_Municipios PRIMARY KEY (Id),
	CONSTRAINT FK_Municipios_Departamentos FOREIGN KEY (IdDepartamento) REFERENCES Departamentos (Id)
)
GO

CREATE TABLE Ubicaciones(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	IdMunicipio int not null,
	CONSTRAINT PK_Ubicaciones PRIMARY KEY (Id),
	CONSTRAINT FK_Ubicaciones_Municipios FOREIGN KEY (IdMunicipio) REFERENCES Municipios (Id)
)
GO

CREATE TABLE Comercios(
	Id int identity not null,
	NIT nvarchar(15) not null,
	Nombre nvarchar(100) not null,
	RazonSocial nvarchar(100) not null,
	Telefono nvarchar(9) not null,
	CorreoElectronico nvarchar(50) not null,
	CONSTRAINT PK_Comercios PRIMARY KEY (Id)
)
GO

CREATE TABLE ComercioSucursales(
	Id int identity not null,
	IdComercio int not null,
	Nombre nvarchar(50) not null,
	IdUbicacion int not null,
	Direccion nvarchar(100) not null,
	Central bit not null,
	Telefono nvarchar(9) not null,
	CONSTRAINT PK_ComercioSucursales PRIMARY KEY (Id),
	CONSTRAINT FK_ComercioSucursales_Comercios FOREIGN KEY (IdComercio) REFERENCES Comercios (Id),
	CONSTRAINT FK_ComercioSucursales_Ubicaciones FOREIGN KEY (IdUbicacion) REFERENCES Ubicaciones (Id)
)
GO

Create Table QuejaTipos(
	Id int identity not null,
	Abreviatura nvarchar(3) not null,
	Nombre nvarchar(30) not null,
	CONSTRAINT PK_QuejaTipos PRIMARY KEY (Id)
)
GO

Create Table QuejaEstados(
	Id int identity not null,
	Nombre nvarchar(30) not null,
	Inicial bit not null,
	Final bit not null,
	Rechazado bit not null,
	CONSTRAINT PK_QuejaEstados PRIMARY KEY (Id)
)

Create Table Quejas(
	Id bigint identity not null,
	IdSucursal int not null,
	IdTipo int not null,
	Codigo nvarchar(30) not null,
	Titulo nvarchar(50) not null,
	Queja nvarchar(2000) not null,
	Peticion nvarchar(2000) not null,
	IdEstado int not null,
	FechaIngreso datetime not null,
	CONSTRAINT PK_Quejas PRIMARY KEY (Id),
	CONSTRAINT FK_Quejas_ComercioSucursales FOREIGN KEY (IdSucursal) REFERENCES ComercioSucursales (Id),
	CONSTRAINT FK_Quejas_QuejaTipos FOREIGN KEY (IdTipo) REFERENCES QuejaTipos (Id),
	CONSTRAINT FK_Quejas_QuejaEstados FOREIGN KEY (IdEstado) REFERENCES QuejaEstados (Id)
)
GO

Create Table QuejaSeguimientos(
	Id int identity not null,
	IdQueja bigint not null,
	IdEstado int not null,
	FechaIngreso datetime not null,
	Comentario nvarchar(1000) not null,
	CONSTRAINT PK_QuejaSeguimientos PRIMARY KEY (Id),
	CONSTRAINT FK_QuejaSeguimientos_Quejas FOREIGN KEY (IdQueja) REFERENCES Quejas (Id),
	CONSTRAINT FK_QuejaSeguimientos_QuejaEstados FOREIGN KEY (IdEstado) REFERENCES QuejaEstados (Id)
)
GO

