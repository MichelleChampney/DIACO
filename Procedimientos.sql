USE DIACO
GO

--ROLES
----------------------------------------------------------------------
CREATE PROC sp_GetAllRoles
AS
BEGIN
	SELECT Id, Nombre FROM Roles
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetRoles(@Id int)
AS
BEGIN
	SELECT Id, Nombre FROM Roles
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveRoles(@Id int,
@Nombre nvarchar(30))
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Roles(Nombre)
		VALUES (@Nombre);
	END
	ELSE
	BEGIN
		UPDATE Roles SET Nombre = @Nombre
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteRoles(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Usuarios WHERE IdRol = @Id)
	BEGIN
		RAISERROR ('El rol ya cuenta con usuarios, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Roles WHERE Id = @Id;
	END
END
GO
--USUARIOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllUsuarios
AS
BEGIN
	SELECT u.Id, u.Usuario, u.Nombre, u.Activo, u.IdRol, r.Nombre as NombreRol
	FROM Usuarios as u
	INNER JOIN Roles as r ON u.IdRol = r.Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetUsuarios(@Id int)
AS
BEGIN
	SELECT Id, Usuario, Nombre, Activo, IdRol FROM Usuarios
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetCuentaUsuarios(@Usuario nvarchar(50))
AS
BEGIN
	SELECT Usuario, Salt, r.Nombre as NombreRol
	FROM Usuarios as u
	INNER JOIN Roles as r ON u.IdRol = r.Id
	WHERE Usuario = @Usuario
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveUsuarios(@Usuario nvarchar(50),
@Nombre nvarchar(50),
@Clave nvarchar(100),
@ConfirmacionClave nvarchar(100),
@Salt varbinary(20),
@Activo bit,
@IdRol int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Usuarios WHERE Usuario = @Usuario)
	BEGIN
		RAISERROR ('Usuario ya registrado.',18,1)
	END
	ELSE
	BEGIN
		INSERT INTO Usuarios(Nombre, Usuario, Clave, Salt, Activo, IdRol)
		VALUES (@Nombre, @Usuario, @Clave, @Salt, @Activo, @IdRol);
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_UpdateUsuarios(@Id int,
@Nombre nvarchar(50),
@Activo bit,
@IdRol int)
AS
BEGIN
	UPDATE Usuarios SET Nombre = @Nombre, Activo = @Activo, IdRol = @IdRol
	WHERE Id = @Id;
END
GO
----------------------------------------------------------------------
CREATE PROC sp_UpdatePasswordUsuarios(@Id int,
@Clave nvarchar(100),
@ConfirmacionClave nvarchar(100),
@Salt varbinary(1024))
AS
BEGIN
	UPDATE Usuarios SET Clave = @Clave, Salt = @Salt
	WHERE Id = @Id;
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteUsuarios(@Id int)
AS
BEGIN
	DELETE FROM Usuarios WHERE Id = @Id;
END
GO
----------------------------------------------------------------------
CREATE PROC sp_ValidarUsuarios(@Usuario nvarchar(50),
@Clave nvarchar(100))
AS
BEGIN
	IF 0 = ANY(SELECT COUNT(1) FROM Usuarios WHERE Usuario = @Usuario)
	BEGIN
		RAISERROR ('Usuario no registrado.',18,1)
	END
	ELSE
	BEGIN
		DECLARE @Activo BIT = 0;
		DECLARE @ClaveUsuario nvarchar(50);
		DECLARE @Rol nvarchar(30);
		SELECT @ClaveUsuario = u.Clave, @Activo = u.Activo, @Rol = r.Nombre
		FROM Usuarios AS u
		INNER JOIN Roles AS r ON u.IdRol = r.Id
		WHERE u.Usuario = @Usuario;

		IF (@Activo = 0)
		BEGIN
			RAISERROR ('El usuario no esta activo.',18,1)
			RETURN;
		END
		
		IF (@Clave <> @ClaveUsuario)
		BEGIN
			RAISERROR ('La clave es incorrecta.',18,1)
			RETURN;
		END

	END
END
GO
--REGIONES
----------------------------------------------------------------------
CREATE PROC sp_GetAllRegiones
AS
BEGIN
	SELECT Id, Nombre FROM Regiones
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetRegiones(@Id int)
AS
BEGIN
	SELECT Id, Nombre FROM Regiones
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveRegiones(@Id int,
@Nombre nvarchar(30))
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Regiones(Nombre)
		VALUES (@Nombre);
	END
	ELSE
	BEGIN
		UPDATE Regiones SET Nombre = @Nombre
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteRegiones(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Departamentos WHERE Id = @Id)
	BEGIN
		RAISERROR ('La region ya cuenta con departamentos, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Regiones WHERE Id = @Id;
	END
END
GO
--DEPARTAMENTOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllDepartamentosByRegion(@IdRegion int)
AS
BEGIN
	SELECT Id, Nombre FROM Departamentos
	Where IdRegion = @IdRegion
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetDepartamentos(@Id int)
AS
BEGIN
	SELECT d.Id, d.Nombre, d.IdRegion, r.Nombre as NombreRegion
	FROM Departamentos as d
	INNER JOIN Regiones as r ON d.IdRegion = r.Id
	WHERE d.Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveDepartamentos(@Id int,
@Nombre nvarchar(30),
@IdRegion int)
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Departamentos(Nombre, IdRegion)
		VALUES (@Nombre, @IdRegion);
	END
	ELSE
	BEGIN
		UPDATE Departamentos SET Nombre = @Nombre, IdRegion = @IdRegion
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteDepartamentos(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Municipios WHERE IdDepartamento = @Id)
	BEGIN
		RAISERROR ('El departamento ya cuenta con municipios, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Departamentos WHERE Id = @Id
	END
END
GO
--MUNICIPIOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllMunicipiosByDepartamento(@IdRegion int,
@IdDepartamento int)
AS
BEGIN
	SELECT m.Id, m.Nombre
	FROM Municipios AS m
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	WHERE d.IdRegion = @IdRegion AND m.IdDepartamento = @IdDepartamento
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetMunicipios(@Id int)
AS
BEGIN
	SELECT m.Id, m.Nombre, m.IdDepartamento, d.Nombre AS NombreDepartamento, r.Id AS IdRegion, r.Nombre AS NombreRegion
	FROM Municipios AS m
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones AS r ON d.IdRegion = r.Id
	WHERE m.Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveMunicipios(@Id int,
@Nombre nvarchar(30),
@IdDepartamento int)
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Municipios(Nombre, IdDepartamento)
		VALUES (@Nombre, @IdDepartamento);
	END
	ELSE
	BEGIN
		UPDATE Municipios SET Nombre = @Nombre, IdDepartamento = @IdDepartamento
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteMunicipios(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Ubicaciones WHERE IdMunicipio = @Id)
	BEGIN
		RAISERROR ('El municipio ya cuenta con ubicaciones, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Municipios WHERE Id = @Id
	END
END
GO
--UBICACIONES
----------------------------------------------------------------------
CREATE PROC sp_GetAllUbicacionesByMunicipio(@IdRegion int,
@IdDepartamento int,
@IdMunicipio int)
AS
BEGIN
	SELECT u.Id, u.Nombre
	FROM Ubicaciones AS u
	INNER JOIN Municipios AS m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	WHERE d.IdRegion = @IdRegion AND m.IdDepartamento = @IdDepartamento AND u.IdMunicipio = @IdMunicipio
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetUbicaciones(@Id int)
AS
BEGIN
	SELECT u.Id, u.Nombre, u.IdMunicipio, m.Nombre as NombreMunicipio, d.Id AS IdDepartamento, d.Nombre AS NombreDepartamento, r.Id AS IdRegion, r.Nombre AS NombreRegion
	FROM Ubicaciones AS u
	INNER JOIN Municipios AS m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones AS r ON d.IdRegion = r.Id
	WHERE u.Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveUbicaciones(@Id int,
@Nombre nvarchar(30),
@IdMunicipio int)
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Ubicaciones(Nombre, IdMunicipio)
		VALUES (@Nombre, @IdMunicipio);
	END
	ELSE
	BEGIN
		UPDATE Ubicaciones SET Nombre = @Nombre, IdMunicipio = @IdMunicipio
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteUbicaciones(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM ComercioSucursales WHERE IdUbicacion = @Id)
	BEGIN
		RAISERROR ('La ubicacion ya cuenta con comercios, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Ubicaciones WHERE Id = @Id
	END
END
GO
--COMERCIOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllComercios
AS
BEGIN
	SELECT Id, NIT, Nombre, RazonSocial, Telefono, CorreoElectronico FROM Comercios
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetComercios(@Id int)
AS
BEGIN
	SELECT Id, NIT, Nombre, RazonSocial, Telefono, CorreoElectronico FROM Comercios
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveComercios(@Id int,
@NIT nvarchar(15),
@Nombre nvarchar(100),
@RazonSocial nvarchar(100),
@Telefono nvarchar(9),
@CorreoElectronico nvarchar(50))
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO Comercios(NIT, Nombre, RazonSocial, Telefono, CorreoElectronico)
		VALUES (@NIT, @Nombre, @RazonSocial, @Telefono, @CorreoElectronico);
	END
	ELSE
	BEGIN
		UPDATE Comercios SET NIT = @NIT, Nombre = @Nombre, RazonSocial = @RazonSocial,
		Telefono = @Telefono, CorreoElectronico = @CorreoElectronico
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteComercios(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM ComercioSucursales WHERE IdComercio = @Id)
	BEGIN
		RAISERROR ('El comercio ya cuenta con sucursales, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM Comercios WHERE Id = @Id
	END
END
GO
--COMERCIOSUCURSALES
----------------------------------------------------------------------
CREATE PROC sp_GetAllComercioSucursalesByComercio(@IdComercio int)
AS
BEGIN
	SELECT s.Id, s.IdComercio, s.Nombre, r.Nombre as NombreRegion, d.Nombre as NombreDepartamento, m.Nombre as NombreMunicipio, s.IdUbicacion, s.IdUbicacion, u.Nombre as NombreUbicacion, s.Direccion, s.Central, s.Telefono
	FROM ComercioSucursales as s
	INNER JOIN Ubicaciones as u ON s.IdUbicacion = u.Id
	INNER JOIN Municipios as m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos as d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones as r ON d.IdRegion = r.Id
	WHERE s.IdComercio = @IdComercio
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetComercioSucursales(@Id int)
AS
BEGIN
	SELECT s.Id, s.IdComercio, c.Nombre as NombreComercio, s.Nombre, r.Id as IdRegion, d.Id as IdDepartamento, m.Id as IdMunicipio, s.IdUbicacion, s.Direccion, s.Central, s.Telefono
	FROM ComercioSucursales as s
	INNER JOIN Comercios as c ON s.IdComercio = c.Id
	INNER JOIN Ubicaciones as u ON s.IdUbicacion = u.Id
	INNER JOIN Municipios as m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos as d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones as r ON d.IdRegion = r.Id
	WHERE s.Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveComercioSucursales(@Id int,
@IdComercio int,
@Nombre nvarchar(50),
@IdUbicacion int,
@Direccion nvarchar(100),
@Central bit,
@Telefono nvarchar(9))
AS
BEGIN
	IF @Central = 1 AND (1 = ANY(SELECT COUNT(1) FROM ComercioSucursales
	WHERE IdComercio = @IdComercio AND Central = 1 AND Id <> @Id))
	BEGIN
		RAISERROR ('Ya existe una sucursal marcada como central.',18,1)
	END
	ELSE
	BEGIN
		IF @Id = 0
		BEGIN
			INSERT INTO ComercioSucursales(IdComercio, Nombre, IdUbicacion, Direccion, Central, Telefono)
			VALUES (@IdComercio, @Nombre, @IdUbicacion, @Direccion, @Central, @Telefono);
		END
		ELSE
		BEGIN
			UPDATE ComercioSucursales SET IdComercio = @IdComercio, Nombre = @Nombre, IdUbicacion = @IdUbicacion,
			Direccion = @Direccion, Central = @Central, Telefono = @Telefono
			WHERE Id = @Id;
		END
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteComercioSucursales(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Quejas WHERE IdSucursal = @Id)
	BEGIN
		RAISERROR ('La sucursal ya cuenta con quejas, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM ComercioSucursales WHERE Id = @Id
	END
END
GO
--QUEJATIPOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllQuejaTipos
AS
BEGIN
	SELECT Id, Abreviatura, Nombre FROM QuejaTipos
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetQuejaTipos(@Id int)
AS
BEGIN
	SELECT Id, Abreviatura, Nombre FROM QuejaTipos
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveQuejaTipos(@Id int,
@Abreviatura nvarchar(3),
@Nombre nvarchar(30))
AS
BEGIN
	IF @Id = 0
	BEGIN
		IF 1 = ANY(SELECT COUNT(1) FROM QuejaTipos WHERE Abreviatura = @Abreviatura)
		BEGIN
			RAISERROR ('La abreviatura ya se encuentra registrada.',18,1)
		END
		ELSE
		BEGIN
			INSERT INTO QuejaTipos(Abreviatura, Nombre)
			VALUES (@Abreviatura, @Nombre);
		END
	END
	ELSE
	BEGIN
		UPDATE QuejaTipos SET Abreviatura = @Abreviatura, Nombre = @Nombre
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteQuejaTipos(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Quejas WHERE IdTipo = @Id)
	BEGIN
		RAISERROR ('El tipo ya cuenta con quejas, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM QuejaTipos WHERE Id = @Id;
	END
END
GO
--QUEJAESTADOS
----------------------------------------------------------------------
CREATE PROC sp_GetAllQuejaEstados
AS
BEGIN
	SELECT Id, Nombre, Inicial, Final, Rechazado FROM QuejaEstados
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetQuejaEstados(@Id int)
AS
BEGIN
	SELECT Id, Nombre, Inicial, Final, Rechazado FROM QuejaEstados
	WHERE Id = @Id
END
GO
----------------------------------------------------------------------
CREATE PROC sp_SaveQuejaEstados(@Id int,
@Nombre nvarchar(30),
@Inicial bit,
@Final bit,
@Rechazado bit)
AS
BEGIN
	IF @Id = 0
	BEGIN
		INSERT INTO QuejaEstados(Nombre, Inicial, Final, Rechazado)
		VALUES (@Nombre, @Inicial, @Final, @Rechazado);
	END
	ELSE
	BEGIN
		UPDATE QuejaEstados SET Nombre = @Nombre, Inicial = @Inicial, Final = @Final, Rechazado = @Rechazado
		WHERE Id = @Id;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_DeleteQuejaEstados(@Id int)
AS
BEGIN
	IF 1 = ANY(SELECT COUNT(1) FROM Quejas WHERE IdEstado = @Id)
	BEGIN
		RAISERROR ('El estado ya cuenta con quejas, no se puede eliminar.',18,1)
	END
	ELSE
	BEGIN
		DELETE FROM QuejaEstados WHERE Id = @Id;
	END
END
GO
--QUEJAS
----------------------------------------------------------------------
CREATE PROC sp_SaveQuejas(@IdSucursal int,
@IdTipo int,
@Titulo nvarchar(50),
@Queja nvarchar(2000),
@Peticion nvarchar(2000))
AS
BEGIN

	DECLARE @IdQueja bigint = 0;

	BEGIN TRANSACTION;
		
		DECLARE @IdEstadoInicial int = 0;
		DECLARE @Codigo nvarchar(50) = '';
		DECLARE @Id table (Id bigint);
		SELECT @IdEstadoInicial = ISNULL(Id,0) FROM QuejaEstados WHERE Inicial = 1;
		SELECT @Codigo = Abreviatura + '_' + CONVERT(nvarchar(4),YEAR(GETDATE())) + '_' + CONVERT(nvarchar(2),MONTH(GETDATE())) + CONVERT(nvarchar,FLOOR(RAND()*(99999-10000+1))+10000)
		FROM QuejaTipos WHERE Id = @IdTipo;
	
		IF @IdEstadoInicial = 0
		BEGIN
			RAISERROR ('No se ha configurado un estado inicial.',18,1)
		END
		ELSE
			INSERT INTO Quejas(IdSucursal, IdTipo, Codigo, Titulo, Queja, Peticion, FechaIngreso, IdEstado)
			OUTPUT INSERTED.Id into @Id
			VALUES (@IdSucursal, @IdTipo, @Codigo, @Titulo, @Queja, @Peticion, GETDATE(), @IdEstadoInicial)

			SELECT @IdQueja = Id FROM @Id;

			INSERT INTO QuejaSeguimientos(IdQueja, IdEstado, FechaIngreso, Comentario)
			VALUES (@IdQueja, @IdEstadoInicial, GETDATE(), 'Creación de queja.')
		END
	
	COMMIT;

	SELECT Codigo FROM Quejas as q WHERE Id = @IdQueja;

GO
----------------------------------------------------------------------
CREATE PROC sp_GetQuejas(@Codigo nvarchar(30))
AS
BEGIN
	SELECT q.Id, q.Codigo, c.Nombre as NombreComercio, sc.Nombre as NombreSucursal, t.Nombre as NombreTipoQueja, e.Nombre as NombreEstadoQueja, q.Titulo, q.FechaIngreso, q.Queja, q.Peticion
	FROM Quejas as q
	INNER JOIN ComercioSucursales as sc ON q.IdSucursal = sc.Id
	INNER JOIN Comercios as c ON sc.IdComercio = c.Id
	INNER JOIN QuejaTipos as t on q.IdTipo = t.Id
	INNER JOIN QuejaEstados as e on q.IdEstado = e.Id
	WHERE q.Codigo = @Codigo
END
GO
--QUEJASEGUIMIENTOS
----------------------------------------------------------------------
CREATE PROC sp_SaveQuejaSeguimientos(@IdQueja bigint,
@IdEstado int,
@Comentario nvarchar(1000))
AS
BEGIN
	DECLARE @IdUsuario int = 0;
	DECLARE @IdEstadoActual int = 0;
	SELECT IdEstado FROM Quejas WHERE Id = @IdQueja;
	IF @IdEstado = @IdEstadoActual
	BEGIN
		RAISERROR ('Debe modificar el estado de la queja.',18,1)
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION;
			INSERT INTO QuejaSeguimientos(IdQueja, IdEstado, FechaIngreso, Comentario)
			VALUES (@IdQueja, @IdEstado, GETDATE(), @Comentario);

			UPDATE Quejas SET IdEstado = @IdEstado WHERE Id = @IdQueja;
		COMMIT;
	END
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetQuejaSeguimientos(@IdQueja bigint)
AS
BEGIN
	SELECT e.Nombre as NombreEstado, qs.FechaIngreso, qs.Comentario
	FROM QuejaSeguimientos as qs
	INNER JOIN QuejaEstados as e on qs.IdEstado = e.Id
	WHERE qs.IdQueja = @IdQueja
	ORDER BY qs.FechaIngreso
END
GO
--CONSULTAS
----------------------------------------------------------------------
CREATE PROC sp_GetConteoQuejasByFiltros(@FechaDel datetime,
@FechaAl datetime,
@IdComercio int = null,
@IdSucursal int = null,
@IdUbicacion int = null,
@IdMunicipio int = null,
@IdDepartamento int = null,
@IdRegion int = null,
@IdTipo int = null,
@Estado int,--0: todos, 1: inicial, 2: proceso, 3: finalizado, 4: rechazada
@TipoConteo bit = null)--null: todos, 1: con quejas, 2: sin quejas
AS
BEGIN
	SELECT c.Nombre as NombreComercio, cs.Nombre AS NombreSucursal, cs.Central, u.Nombre AS NombreUbicacion,
	m.Nombre AS NombreMunicipio, d.Nombre AS NombreDepartamento, r.Nombre AS NombreRegion, COUNT(1) AS Conteo
	FROM Comercios AS c
	INNER JOIN ComercioSucursales AS cs ON c.Id = cs.IdComercio
	INNER JOIN Ubicaciones AS u ON cs.IdUbicacion = u.Id
	INNER JOIN Municipios AS m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones as r ON d.IdRegion = r.Id
	LEFT JOIN Quejas as q ON cs.Id = q.IdSucursal AND q.FechaIngreso BETWEEN @FechaDel AND @FechaAl
	LEFT JOIN QuejaEstados as e ON q.IdEstado = e.Id
	WHERE c.Id = ISNULL(@IdComercio,c.Id)
	AND cs.Id = ISNULL(@IdSucursal,cs.Id)
	AND u.Id = ISNULL(@IdUbicacion,u.Id)
	AND m.Id = ISNULL(@IdMunicipio,m.Id)
	AND d.Id = ISNULL(@IdDepartamento,d.Id)
	AND r.Id = ISNULL(@IdRegion,r.Id)
	AND q.IdTipo = ISNULL(@IdTipo,q.IdTipo)
	AND (@Estado = 0 OR (@Estado = 1 AND ISNULL(e.Inicial,0) = 1) OR (@Estado = 2 AND ISNULL(e.Inicial,0) = 0
	AND ISNULL(e.Final,0) = 0 AND ISNULL(e.Rechazado,0) = 0) OR (@Estado = 3 AND ISNULL(e.Final,0) = 1)
	OR (@Estado = 4 AND ISNULL(e.Rechazado,0) = 1))
	Group By c.Nombre, cs.Nombre, cs.Central, u.Nombre, m.Nombre, d.Nombre, r.Nombre
	HAVING @TipoConteo IS NULL OR (@TipoConteo = 0 AND COUNT(1) = 0) OR (@TipoConteo = 1 AND COUNT(1) > 0)
END
GO
--execute sp_GetConteoQuejasByFiltros '2020/01/01','2020/01/01',null,null,null,0,0
----------------------------------------------------------------------
CREATE PROC sp_GetQuejasByFiltros(@FechaDel datetime,
@FechaAl datetime,
@IdComercio int = null,
@IdSucursal int = null,
@IdUbicacion int = null,
@IdMunicipio int = null,
@IdDepartamento int = null,
@IdRegion int = null,
@IdTipo int = null,
@Estado int)--0: todos, 1: inicial, 2: proceso, 3: finalizado, 4: rechazada
AS
BEGIN
	SELECT q.Id as IdQueja, q.Codigo, t.Nombre as NombreTipoQueja, q.Titulo, q.FechaIngreso, q.Queja, q.Peticion, e.Nombre as NombreEstadoQueja, c.Nombre as NombreComercio, cs.Nombre AS NombreSucursal, cs.Central, u.Nombre AS NombreUbicacion,
	m.Nombre AS NombreMunicipio, d.Nombre AS NombreDepartamento, r.Nombre AS NombreRegion
	FROM Quejas as q
	INNER JOIN QuejaTipos as t ON q.IdTipo = t.Id
	INNER JOIN QuejaEstados as e ON q.IdEstado = e.Id
	INNER JOIN ComercioSucursales AS cs ON q.IdSucursal = cs.Id
	INNER JOIN Comercios AS c ON cs.IdComercio = c.Id
	INNER JOIN Ubicaciones AS u ON cs.IdUbicacion = u.Id
	INNER JOIN Municipios AS m ON u.IdMunicipio = m.Id
	INNER JOIN Departamentos AS d ON m.IdDepartamento = d.Id
	INNER JOIN Regiones as r ON d.IdRegion = r.Id
	WHERE q.FechaIngreso BETWEEN @FechaDel AND @FechaAl
	AND c.Id = ISNULL(@IdComercio,c.Id)
	AND cs.Id = ISNULL(@IdSucursal,cs.Id)
	AND u.Id = ISNULL(@IdUbicacion,u.Id)
	AND m.Id = ISNULL(@IdMunicipio,m.Id)
	AND d.Id = ISNULL(@IdDepartamento,d.Id)
	AND r.Id = ISNULL(@IdRegion,r.Id)
	AND ISNULL(q.IdTipo,0) = ISNULL(@IdTipo,ISNULL(q.IdTipo,0))
	AND (@Estado = 0 OR (@Estado = 1 AND ISNULL(e.Inicial,0) = 1) OR (@Estado = 2 AND ISNULL(e.Inicial,0) = 0
	AND ISNULL(e.Final,0) = 0 AND ISNULL(e.Rechazado,0) = 0) OR (@Estado = 3 AND ISNULL(e.Final,0) = 1)
	OR (@Estado = 4 AND ISNULL(e.Rechazado,0) = 1))
END
GO
--execute sp_GetQuejasByFiltros '2020/01/01','2020/01/01',null,null,null,0
----------------------------------------------------------------------
CREATE PROC sp_GetConteoQuejasByRegion(@Anio int,
@TipoConteo bit = null)--null: todos, 1: con quejas, 2: sin quejas
AS
BEGIN
	SELECT r.Nombre AS NombreRegion, COUNT(q.Id) as Conteo
	FROM Regiones as r
	LEFT JOIN Departamentos AS d ON r.Id = d.IdRegion
	LEFT JOIN Municipios AS m ON d.Id = m.IdDepartamento
	LEFT JOIN Ubicaciones AS u ON m.Id = u.IdMunicipio
	LEFT JOIN ComercioSucursales AS sc ON u.Id = sc.IdUbicacion
	LEFT JOIN Quejas AS q ON sc.Id = q.IdSucursal AND YEAR(q.FechaIngreso) = @Anio
	GROUP BY r.Nombre
	HAVING @TipoConteo IS NULL OR (@TipoConteo = 0 AND COUNT(1) = 0) OR (@TipoConteo = 1 AND COUNT(1) > 0)
	ORDER BY r.Nombre
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetConteoQuejasByDepartamento(@Anio int,
@TipoConteo bit = null)--null: todos, 1: con quejas, 2: sin quejas
AS
BEGIN
	SELECT r.Nombre AS NombreRegion, ISNULL(d.Nombre,'<Sin Ingresar>') as NombreDepartamento, COUNT(q.Id) as Conteo
	FROM Regiones as r
	LEFT JOIN Departamentos AS d ON r.Id = d.IdRegion
	LEFT JOIN Municipios AS m ON d.Id = m.IdDepartamento
	LEFT JOIN Ubicaciones AS u ON m.Id = u.IdMunicipio
	LEFT JOIN ComercioSucursales AS sc ON u.Id = sc.IdUbicacion
	LEFT JOIN Quejas AS q ON sc.Id = q.IdSucursal AND YEAR(q.FechaIngreso) = @Anio
	GROUP BY r.Nombre, d.Nombre
	HAVING @TipoConteo IS NULL OR (@TipoConteo = 0 AND COUNT(1) = 0) OR (@TipoConteo = 1 AND COUNT(1) > 0)
	ORDER BY r.Nombre, d.Nombre
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetConteoQuejasByMunicipio(@Anio int,
@TipoConteo bit = null)--null: todos, 1: con quejas, 2: sin quejas
AS
BEGIN
	SELECT r.Nombre AS NombreRegion, ISNULL(d.Nombre,'<Sin Ingresar>') as NombreDepartamento, ISNULL(m.Nombre,'<Sin Ingresar>') as NombreMunicipio, COUNT(q.Id) as Conteo
	FROM Regiones as r
	LEFT JOIN Departamentos AS d ON r.Id = d.IdRegion
	LEFT JOIN Municipios AS m ON d.Id = m.IdDepartamento
	LEFT JOIN Ubicaciones AS u ON m.Id = u.IdMunicipio
	LEFT JOIN ComercioSucursales AS sc ON u.Id = sc.IdUbicacion
	LEFT JOIN Quejas AS q ON sc.Id = q.IdSucursal AND YEAR(q.FechaIngreso) = @Anio
	GROUP BY r.Nombre, d.Nombre, m.Nombre
	HAVING @TipoConteo IS NULL OR (@TipoConteo = 0 AND COUNT(1) = 0) OR (@TipoConteo = 1 AND COUNT(1) > 0)
	ORDER BY r.Nombre, d.Nombre, m.Nombre
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetConteoQuejasByComercio(@Anio int,
@TipoConteo bit = null)--null: todos, 1: con quejas, 2: sin quejas
AS
BEGIN
	SELECT c.Nombre AS NombreComercio, COUNT(q.Id) as Conteo
	FROM Comercios as c
	LEFT JOIN ComercioSucursales AS sc ON c.Id = sc.IdComercio
	LEFT JOIN Quejas AS q ON sc.Id = q.IdSucursal AND YEAR(q.FechaIngreso) = @Anio
	GROUP BY c.Nombre
	HAVING @TipoConteo IS NULL OR (@TipoConteo = 0 AND COUNT(1) = 0) OR (@TipoConteo = 1 AND COUNT(1) > 0)
	ORDER BY c.Nombre
END
GO
----------------------------------------------------------------------
CREATE PROC sp_GetQuejasPendientes(@FechaDel datetime,
@FechaAl datetime,
@IdComercio int = null,
@IdSucursal int = null,
@IdTipo int = null)
AS
BEGIN
	SELECT q.Id, q.Codigo, t.Nombre as NombreTipoQueja, q.Titulo, q.FechaIngreso, q.Queja, q.Peticion, e.Nombre as NombreEstadoQueja, c.Nombre as NombreComercio, cs.Nombre AS NombreSucursal, cs.Central
	FROM Quejas as q
	INNER JOIN QuejaTipos as t ON q.IdTipo = t.Id
	INNER JOIN QuejaEstados as e ON q.IdEstado = e.Id
	INNER JOIN ComercioSucursales AS cs ON q.IdSucursal = cs.Id
	INNER JOIN Comercios AS c ON cs.IdComercio = c.Id
	WHERE q.FechaIngreso BETWEEN @FechaDel AND @FechaAl
	AND c.Id = ISNULL(@IdComercio,c.Id)
	AND cs.Id = ISNULL(@IdSucursal,cs.Id)
	AND ISNULL(q.IdTipo,0) = ISNULL(@IdTipo,ISNULL(q.IdTipo,0))
	AND e.Final = 0 AND e.Rechazado = 0
END
GO
=