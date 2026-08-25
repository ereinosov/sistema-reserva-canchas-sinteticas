/* ============================================================================
   Sistema de Reserva de Canchas Sintéticas — Script DDL definitivo (SQL Server)
   Ejecutar UNA sola vez contra una instancia nueva. Deja la base de datos
   lista para usar: no requiere ningún script adicional.
   ============================================================================ */

------------------------------------------------------------------------------
-- 0. BASE DE DATOS Y OPCIONES DE SESIÓN
------------------------------------------------------------------------------
IF DB_ID(N'ReservaCanchasDB') IS NULL
    CREATE DATABASE ReservaCanchasDB;
GO

USE ReservaCanchasDB;
GO

-- El índice filtrado UQ_RESERVAS_horario_activa requiere QUOTED_IDENTIFIER ON
-- en el lote que crea los procedimientos que insertan/actualizan RESERVAS.
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Requerido para que sp_RegistrarUsuario (EXECUTE AS OWNER) pueda ejecutar
-- CREATE LOGIN / ALTER SERVER ROLE. El propietario de la base debe ser sysadmin.
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ReservaCanchasDB' AND is_trustworthy_on = 0)
    ALTER DATABASE ReservaCanchasDB SET TRUSTWORTHY ON;
GO

------------------------------------------------------------------------------
-- 1. TABLAS
------------------------------------------------------------------------------

-- 1.1 CLIENTES
IF OBJECT_ID(N'dbo.CLIENTES', N'U') IS NOT NULL DROP TABLE dbo.CLIENTES;
GO
CREATE TABLE dbo.CLIENTES
(
    id_cliente               INT IDENTITY(1,1) NOT NULL,
    nombre_cliente            NVARCHAR(100)     NOT NULL,
    tipo_documento_cliente    CHAR(10)          NOT NULL,
    numero_documento_cliente  VARCHAR(20)       NOT NULL,
    telefono_cliente          VARCHAR(15)       NOT NULL,
    correo_cliente            VARCHAR(100)      NOT NULL,

    CONSTRAINT PK_CLIENTES PRIMARY KEY (id_cliente),
    CONSTRAINT UQ_CLIENTES_documento UNIQUE (tipo_documento_cliente, numero_documento_cliente),
    CONSTRAINT UQ_CLIENTES_telefono UNIQUE (telefono_cliente),
    CONSTRAINT UQ_CLIENTES_correo UNIQUE (correo_cliente),
    CONSTRAINT CK_CLIENTES_tipo_documento CHECK (tipo_documento_cliente IN ('cedula','pasaporte','ruc')),
    CONSTRAINT CK_CLIENTES_numero_documento CHECK (
        tipo_documento_cliente <> 'cedula'
        OR (LEN(numero_documento_cliente) = 10 AND numero_documento_cliente NOT LIKE '%[^0-9]%')
    )
);
GO

-- 1.2 ROLES
IF OBJECT_ID(N'dbo.ROLES', N'U') IS NOT NULL DROP TABLE dbo.ROLES;
GO
CREATE TABLE dbo.ROLES
(
    id_rol       INT IDENTITY(1,1) NOT NULL,
    nombre_rol    CHAR(15)          NOT NULL,

    CONSTRAINT PK_ROLES PRIMARY KEY (id_rol),
    CONSTRAINT UQ_ROLES_nombre UNIQUE (nombre_rol),
    CONSTRAINT CK_ROLES_nombre CHECK (nombre_rol IN ('administrador','empleado'))
);
GO
INSERT INTO dbo.ROLES (nombre_rol) VALUES ('administrador'), ('empleado');
GO

-- 1.3 USUARIOS
IF OBJECT_ID(N'dbo.USUARIOS', N'U') IS NOT NULL DROP TABLE dbo.USUARIOS;
GO
CREATE TABLE dbo.USUARIOS
(
    id_usuario       INT IDENTITY(1,1) NOT NULL,
    nombre_usuario    NVARCHAR(100)     NOT NULL,
    usuario_login     VARCHAR(30)       NOT NULL,  -- credencial de aplicación
    clave_app_hash    VARCHAR(255)      NOT NULL,  -- hash bcrypt de la clave de aplicación
    usuario_bd        VARCHAR(30)       NOT NULL,  -- login real de SQL Server
    clave_bd_enc      VARCHAR(255)      NOT NULL,  -- clave del login, cifrada con AES
    id_rol            INT               NOT NULL,
    estado_usuario    CHAR(10)          NOT NULL CONSTRAINT DF_USUARIOS_estado DEFAULT ('activo'),

    CONSTRAINT PK_USUARIOS PRIMARY KEY (id_usuario),
    CONSTRAINT UQ_USUARIOS_login UNIQUE (usuario_login),
    CONSTRAINT UQ_USUARIOS_usuario_bd UNIQUE (usuario_bd),
    CONSTRAINT FK_USUARIOS_ROLES FOREIGN KEY (id_rol) REFERENCES dbo.ROLES (id_rol) ON DELETE NO ACTION,
    CONSTRAINT CK_USUARIOS_estado CHECK (estado_usuario IN ('activo','inactivo'))
);
GO

-- 1.4 CANCHAS
IF OBJECT_ID(N'dbo.CANCHAS', N'U') IS NOT NULL DROP TABLE dbo.CANCHAS;
GO
CREATE TABLE dbo.CANCHAS
(
    id_cancha              INT IDENTITY(1,1) NOT NULL,
    nombre_cancha           NVARCHAR(60)      NOT NULL,
    estado_cancha           CHAR(10)          NOT NULL CONSTRAINT DF_CANCHAS_estado DEFAULT ('activa'),
    hora_inicio_operacion   TIME(0)           NOT NULL CONSTRAINT DF_CANCHAS_hora_inicio DEFAULT ('06:00:00'),
    hora_fin_operacion      TIME(0)           NOT NULL CONSTRAINT DF_CANCHAS_hora_fin DEFAULT ('22:00:00'),

    CONSTRAINT PK_CANCHAS PRIMARY KEY (id_cancha),
    CONSTRAINT UQ_CANCHAS_nombre UNIQUE (nombre_cancha),
    CONSTRAINT CK_CANCHAS_estado CHECK (estado_cancha IN ('activa','inactiva')),
    CONSTRAINT CK_CANCHAS_horario CHECK (hora_inicio_operacion < hora_fin_operacion)
);
GO

-- 1.5 HORARIOS
IF OBJECT_ID(N'dbo.HORARIOS', N'U') IS NOT NULL DROP TABLE dbo.HORARIOS;
GO
CREATE TABLE dbo.HORARIOS
(
    id_horario           INT IDENTITY(1,1) NOT NULL,
    id_cancha             INT               NOT NULL,
    fecha_horario         DATE              NOT NULL,
    hora_inicio_horario   TIME(0)           NOT NULL,

    CONSTRAINT PK_HORARIOS PRIMARY KEY (id_horario),
    CONSTRAINT FK_HORARIOS_CANCHAS FOREIGN KEY (id_cancha) REFERENCES dbo.CANCHAS (id_cancha) ON DELETE NO ACTION,
    CONSTRAINT UQ_HORARIOS_franja UNIQUE (id_cancha, fecha_horario, hora_inicio_horario),
    CONSTRAINT CK_HORARIOS_minuto CHECK (DATEPART(MINUTE, hora_inicio_horario) = 0)
);
GO

-- 1.6 RESERVAS
IF OBJECT_ID(N'dbo.RESERVAS', N'U') IS NOT NULL DROP TABLE dbo.RESERVAS;
GO
CREATE TABLE dbo.RESERVAS
(
    id_reserva       INT IDENTITY(1,1) NOT NULL,
    id_cliente        INT               NOT NULL,
    id_horario        INT               NOT NULL,
    id_usuario        INT               NOT NULL,  -- quién registró la reserva
    estado_reserva    CHAR(10)          NOT NULL CONSTRAINT DF_RESERVAS_estado DEFAULT ('activa'),

    CONSTRAINT PK_RESERVAS PRIMARY KEY (id_reserva),
    CONSTRAINT FK_RESERVAS_CLIENTES FOREIGN KEY (id_cliente) REFERENCES dbo.CLIENTES (id_cliente) ON DELETE NO ACTION,
    CONSTRAINT FK_RESERVAS_HORARIOS FOREIGN KEY (id_horario) REFERENCES dbo.HORARIOS (id_horario) ON DELETE NO ACTION,
    CONSTRAINT FK_RESERVAS_USUARIOS FOREIGN KEY (id_usuario) REFERENCES dbo.USUARIOS (id_usuario) ON DELETE NO ACTION,
    CONSTRAINT CK_RESERVAS_estado CHECK (estado_reserva IN ('activa','cancelada'))
);
GO

-- 1.7 PAGOS
IF OBJECT_ID(N'dbo.PAGOS', N'U') IS NOT NULL DROP TABLE dbo.PAGOS;
GO
CREATE TABLE dbo.PAGOS
(
    id_pago       INT IDENTITY(1,1) NOT NULL,
    id_reserva     INT               NOT NULL,
    monto_pago     DECIMAL(9,2)      NOT NULL,
    fecha_pago     DATE              NULL,
    estado_pago    CHAR(10)          NOT NULL CONSTRAINT DF_PAGOS_estado DEFAULT ('pendiente'),

    CONSTRAINT PK_PAGOS PRIMARY KEY (id_pago),
    -- Una reserva admite a lo sumo un pago.
    CONSTRAINT UQ_PAGOS_reserva UNIQUE (id_reserva),
    CONSTRAINT FK_PAGOS_RESERVAS FOREIGN KEY (id_reserva) REFERENCES dbo.RESERVAS (id_reserva) ON DELETE NO ACTION,
    CONSTRAINT CK_PAGOS_monto CHECK (monto_pago > 0),
    CONSTRAINT CK_PAGOS_estado CHECK (estado_pago IN ('pendiente','pagado'))
);
GO

------------------------------------------------------------------------------
-- 2. ROLES DE BASE DE DATOS Y LOGIN DE ARRANQUE (BOOTSTRAP)
------------------------------------------------------------------------------
IF DATABASE_PRINCIPAL_ID('db_rol_empleado') IS NULL
    CREATE ROLE db_rol_empleado;
GO
IF DATABASE_PRINCIPAL_ID('db_rol_administrador') IS NULL
    CREATE ROLE db_rol_administrador;
GO

-- Login de privilegio mínimo usado por la pantalla de inicio de sesión ANTES
-- de que la persona esté autenticada con su propio login individual.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'login_bootstrap')
    CREATE LOGIN login_bootstrap WITH PASSWORD = N'CAMBIAR_EN_DESPLIEGUE#2026', CHECK_POLICY = ON;
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'login_bootstrap')
    CREATE USER login_bootstrap FOR LOGIN login_bootstrap;
GO

------------------------------------------------------------------------------
-- 3. ÍNDICE ÚNICO FILTRADO
--    No pueden existir dos reservas activas sobre la misma franja horaria.
------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_RESERVAS_horario_activa' AND object_id = OBJECT_ID('dbo.RESERVAS'))
    DROP INDEX UQ_RESERVAS_horario_activa ON dbo.RESERVAS;
GO
CREATE UNIQUE INDEX UQ_RESERVAS_horario_activa
    ON dbo.RESERVAS (id_horario)
    WHERE estado_reserva = 'activa';
GO

------------------------------------------------------------------------------
-- 4. FUNCIONES
------------------------------------------------------------------------------

IF OBJECT_ID(N'dbo.fn_HoraFinFranja', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_HoraFinFranja;
GO
CREATE FUNCTION dbo.fn_HoraFinFranja (@hora_inicio TIME(0))
RETURNS TIME(0)
AS
BEGIN
    RETURN CAST(DATEADD(HOUR, 1, @hora_inicio) AS TIME(0));
END
GO

IF OBJECT_ID(N'dbo.fn_FranjaOcupada', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_FranjaOcupada;
GO
CREATE FUNCTION dbo.fn_FranjaOcupada (@id_horario INT)
RETURNS BIT
AS
BEGIN
    DECLARE @ocupada BIT = 0;
    IF EXISTS (SELECT 1 FROM dbo.RESERVAS WHERE id_horario = @id_horario AND estado_reserva = 'activa')
        SET @ocupada = 1;
    RETURN @ocupada;
END
GO

IF OBJECT_ID(N'dbo.fn_IngresosDetalle', N'IF') IS NOT NULL DROP FUNCTION dbo.fn_IngresosDetalle;
GO
CREATE FUNCTION dbo.fn_IngresosDetalle (@fecha_inicio DATE, @fecha_fin DATE)
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.id_pago, r.id_reserva, c.nombre_cliente, ca.nombre_cancha,
        h.fecha_horario, h.hora_inicio_horario, p.monto_pago, p.fecha_pago
    FROM dbo.PAGOS p
    INNER JOIN dbo.RESERVAS r ON r.id_reserva = p.id_reserva
    INNER JOIN dbo.HORARIOS h ON h.id_horario = r.id_horario
    INNER JOIN dbo.CANCHAS ca ON ca.id_cancha = h.id_cancha
    INNER JOIN dbo.CLIENTES c ON c.id_cliente = r.id_cliente
    WHERE p.estado_pago = 'pagado'
      AND h.fecha_horario BETWEEN @fecha_inicio AND @fecha_fin
);
GO

------------------------------------------------------------------------------
-- 5. PROCEDIMIENTOS ALMACENADOS
------------------------------------------------------------------------------

-- sp_GenerarHorariosDia: franjas de 1 hora entre hora_inicio_operacion y hora_fin_operacion de la cancha.
IF OBJECT_ID(N'dbo.sp_GenerarHorariosDia', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_GenerarHorariosDia;
GO
CREATE PROCEDURE dbo.sp_GenerarHorariosDia
    @id_cancha INT,
    @fecha     DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @hora_inicio TIME(0), @hora_fin TIME(0);
    SELECT @hora_inicio = hora_inicio_operacion, @hora_fin = hora_fin_operacion
    FROM dbo.CANCHAS
    WHERE id_cancha = @id_cancha;

    IF @hora_inicio IS NULL
        RETURN;

    ;WITH Horas AS (
        SELECT @hora_inicio AS hora_inicio
        UNION ALL
        SELECT DATEADD(HOUR, 1, hora_inicio)
        FROM Horas
        WHERE DATEADD(HOUR, 1, hora_inicio) < @hora_fin
    )
    INSERT INTO dbo.HORARIOS (id_cancha, fecha_horario, hora_inicio_horario)
    SELECT @id_cancha, @fecha, h.hora_inicio
    FROM Horas h
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.HORARIOS x
        WHERE x.id_cancha = @id_cancha AND x.fecha_horario = @fecha AND x.hora_inicio_horario = h.hora_inicio
    )
    OPTION (MAXRECURSION 24);
END
GO

-- sp_RegistrarCliente
IF OBJECT_ID(N'dbo.sp_RegistrarCliente', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarCliente;
GO
CREATE PROCEDURE dbo.sp_RegistrarCliente
    @nombre_cliente           NVARCHAR(100),
    @tipo_documento_cliente   CHAR(10),
    @numero_documento_cliente VARCHAR(20),
    @telefono_cliente         VARCHAR(15),
    @correo_cliente           VARCHAR(100),
    @id_cliente_nuevo         INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE tipo_documento_cliente = @tipo_documento_cliente AND numero_documento_cliente = @numero_documento_cliente)
            THROW 50001, 'Ya existe un cliente registrado con ese tipo y número de documento.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE telefono_cliente = @telefono_cliente)
            THROW 50013, 'Ya existe un cliente registrado con ese teléfono.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE correo_cliente = @correo_cliente)
            THROW 50014, 'Ya existe un cliente registrado con ese correo.', 1;

        INSERT INTO dbo.CLIENTES (nombre_cliente, tipo_documento_cliente, numero_documento_cliente, telefono_cliente, correo_cliente)
        VALUES (@nombre_cliente, @tipo_documento_cliente, @numero_documento_cliente, @telefono_cliente, @correo_cliente);

        SET @id_cliente_nuevo = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ConsultarClientes
IF OBJECT_ID(N'dbo.sp_ConsultarClientes', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarClientes;
GO
CREATE PROCEDURE dbo.sp_ConsultarClientes
    @numero_documento_cliente VARCHAR(20) = NULL,
    @nombre_cliente           NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_cliente, nombre_cliente, tipo_documento_cliente, numero_documento_cliente, telefono_cliente, correo_cliente
    FROM dbo.CLIENTES
    WHERE (@numero_documento_cliente IS NULL OR numero_documento_cliente = @numero_documento_cliente)
      AND (@nombre_cliente IS NULL OR nombre_cliente LIKE '%' + @nombre_cliente + '%');
END
GO

-- sp_ModificarCliente
IF OBJECT_ID(N'dbo.sp_ModificarCliente', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ModificarCliente;
GO
CREATE PROCEDURE dbo.sp_ModificarCliente
    @id_cliente               INT,
    @nombre_cliente           NVARCHAR(100),
    @tipo_documento_cliente   CHAR(10),
    @numero_documento_cliente VARCHAR(20),
    @telefono_cliente         VARCHAR(15),
    @correo_cliente           VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE id_cliente = @id_cliente)
            THROW 50002, 'El cliente indicado no existe.', 1;

        IF EXISTS (
            SELECT 1 FROM dbo.CLIENTES
            WHERE tipo_documento_cliente = @tipo_documento_cliente
              AND numero_documento_cliente = @numero_documento_cliente
              AND id_cliente <> @id_cliente
        )
            THROW 50001, 'Ya existe otro cliente registrado con ese tipo y número de documento.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE telefono_cliente = @telefono_cliente AND id_cliente <> @id_cliente)
            THROW 50013, 'Ya existe otro cliente registrado con ese teléfono.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE correo_cliente = @correo_cliente AND id_cliente <> @id_cliente)
            THROW 50014, 'Ya existe otro cliente registrado con ese correo.', 1;

        UPDATE dbo.CLIENTES
        SET nombre_cliente = @nombre_cliente,
            tipo_documento_cliente = @tipo_documento_cliente,
            numero_documento_cliente = @numero_documento_cliente,
            telefono_cliente = @telefono_cliente,
            correo_cliente = @correo_cliente
        WHERE id_cliente = @id_cliente;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_EliminarCliente
IF OBJECT_ID(N'dbo.sp_EliminarCliente', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EliminarCliente;
GO
CREATE PROCEDURE dbo.sp_EliminarCliente
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE id_cliente = @id_cliente)
            THROW 50002, 'El cliente indicado no existe.', 1;

        IF EXISTS (SELECT 1 FROM dbo.RESERVAS WHERE id_cliente = @id_cliente AND estado_reserva = 'activa')
            THROW 50003, 'No se puede eliminar el cliente: tiene reservas activas.', 1;

        IF EXISTS (
            SELECT 1 FROM dbo.PAGOS p
            INNER JOIN dbo.RESERVAS r ON r.id_reserva = p.id_reserva
            WHERE r.id_cliente = @id_cliente AND p.estado_pago = 'pendiente'
        )
            THROW 50004, 'No se puede eliminar el cliente: tiene pagos pendientes.', 1;

        DELETE FROM dbo.CLIENTES WHERE id_cliente = @id_cliente;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_RegistrarCancha
IF OBJECT_ID(N'dbo.sp_RegistrarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarCancha;
GO
CREATE PROCEDURE dbo.sp_RegistrarCancha
    @nombre_cancha          NVARCHAR(60),
    @hora_inicio_operacion  TIME(0) = '06:00:00',
    @hora_fin_operacion     TIME(0) = '22:00:00',
    @id_cancha_nueva        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE nombre_cancha = @nombre_cancha)
            THROW 50020, 'Ya existe una cancha registrada con ese nombre.', 1;

        INSERT INTO dbo.CANCHAS (nombre_cancha, hora_inicio_operacion, hora_fin_operacion)
        VALUES (@nombre_cancha, @hora_inicio_operacion, @hora_fin_operacion);
        SET @id_cancha_nueva = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ConsultarCanchas
IF OBJECT_ID(N'dbo.sp_ConsultarCanchas', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarCanchas;
GO
CREATE PROCEDURE dbo.sp_ConsultarCanchas
    @estado_cancha CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_cancha, nombre_cancha, estado_cancha, hora_inicio_operacion, hora_fin_operacion
    FROM dbo.CANCHAS
    WHERE (@estado_cancha IS NULL OR estado_cancha = @estado_cancha)
    ORDER BY nombre_cancha;
END
GO

-- sp_ModificarCancha
IF OBJECT_ID(N'dbo.sp_ModificarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ModificarCancha;
GO
CREATE PROCEDURE dbo.sp_ModificarCancha
    @id_cancha              INT,
    @nombre_cancha          NVARCHAR(60),
    @hora_inicio_operacion  TIME(0) = '06:00:00',
    @hora_fin_operacion     TIME(0) = '22:00:00'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE id_cancha = @id_cancha)
            THROW 50021, 'La cancha indicada no existe.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE nombre_cancha = @nombre_cancha AND id_cancha <> @id_cancha)
            THROW 50020, 'Ya existe otra cancha registrada con ese nombre.', 1;

        UPDATE dbo.CANCHAS
        SET nombre_cancha = @nombre_cancha,
            hora_inicio_operacion = @hora_inicio_operacion,
            hora_fin_operacion = @hora_fin_operacion
        WHERE id_cancha = @id_cancha;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_DesactivarCancha
IF OBJECT_ID(N'dbo.sp_DesactivarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_DesactivarCancha;
GO
CREATE PROCEDURE dbo.sp_DesactivarCancha
    @id_cancha INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE id_cancha = @id_cancha)
            THROW 50021, 'La cancha indicada no existe.', 1;

        UPDATE dbo.CANCHAS SET estado_cancha = 'inactiva' WHERE id_cancha = @id_cancha;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ActivarCancha
IF OBJECT_ID(N'dbo.sp_ActivarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActivarCancha;
GO
CREATE PROCEDURE dbo.sp_ActivarCancha
    @id_cancha INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @estado CHAR(10);
        SELECT @estado = estado_cancha FROM dbo.CANCHAS WHERE id_cancha = @id_cancha;

        IF @estado IS NULL
            THROW 50021, 'La cancha indicada no existe.', 1;

        IF @estado = 'activa'
            THROW 50023, 'La cancha ya se encuentra activa.', 1;

        UPDATE dbo.CANCHAS SET estado_cancha = 'activa' WHERE id_cancha = @id_cancha;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_RegistrarUsuario: crea la fila de USUARIOS y, con EXECUTE AS OWNER, el login real de SQL Server.
IF OBJECT_ID(N'dbo.sp_RegistrarUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarUsuario;
GO
CREATE PROCEDURE dbo.sp_RegistrarUsuario
    @nombre_usuario   NVARCHAR(100),
    @usuario_login    VARCHAR(30),
    @clave_app_hash   VARCHAR(255),
    @usuario_bd       VARCHAR(30),
    @clave_bd_plana   VARCHAR(128),  -- solo viaja dentro de esta transacción; nunca se persiste en claro
    @clave_bd_enc     VARCHAR(255),  -- ya cifrada por la capa de negocio, se persiste tal cual
    @nombre_rol       CHAR(15),
    @id_usuario_nuevo INT OUTPUT
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.USUARIOS WHERE usuario_login = @usuario_login OR usuario_bd = @usuario_bd)
            THROW 50030, 'Ya existe un usuario registrado con ese usuario de acceso.', 1;

        DECLARE @id_rol INT;
        SELECT @id_rol = id_rol FROM dbo.ROLES WHERE nombre_rol = @nombre_rol;
        IF @id_rol IS NULL
            THROW 50031, 'El rol indicado no existe.', 1;

        BEGIN TRANSACTION;
            INSERT INTO dbo.USUARIOS (nombre_usuario, usuario_login, clave_app_hash, usuario_bd, clave_bd_enc, id_rol)
            VALUES (@nombre_usuario, @usuario_login, @clave_app_hash, @usuario_bd, @clave_bd_enc, @id_rol);

            SET @id_usuario_nuevo = SCOPE_IDENTITY();

            DECLARE @sql NVARCHAR(MAX);
            SET @sql = N'CREATE LOGIN ' + QUOTENAME(@usuario_bd) + N' WITH PASSWORD = ' + QUOTENAME(@clave_bd_plana, N'''') + N', CHECK_POLICY = ON;';
            EXEC (@sql);
            SET @sql = N'CREATE USER ' + QUOTENAME(@usuario_bd) + N' FOR LOGIN ' + QUOTENAME(@usuario_bd) + N';';
            EXEC (@sql);
            SET @sql = N'ALTER ROLE ' + QUOTENAME(CASE @nombre_rol WHEN 'administrador' THEN 'db_rol_administrador' ELSE 'db_rol_empleado' END)
                       + N' ADD MEMBER ' + QUOTENAME(@usuario_bd) + N';';
            EXEC (@sql);

            IF @nombre_rol = 'administrador'
            BEGIN
                SET @sql = N'ALTER SERVER ROLE securityadmin ADD MEMBER ' + QUOTENAME(@usuario_bd) + N';';
                EXEC (@sql);
            END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- sp_DesactivarUsuario
IF OBJECT_ID(N'dbo.sp_DesactivarUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_DesactivarUsuario;
GO
CREATE PROCEDURE dbo.sp_DesactivarUsuario
    @id_usuario INT
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @usuario_bd VARCHAR(30);
        SELECT @usuario_bd = usuario_bd FROM dbo.USUARIOS WHERE id_usuario = @id_usuario;

        IF @usuario_bd IS NULL
            THROW 50032, 'El usuario indicado no existe.', 1;

        UPDATE dbo.USUARIOS SET estado_usuario = 'inactivo' WHERE id_usuario = @id_usuario;

        DECLARE @sql NVARCHAR(MAX) = N'ALTER LOGIN ' + QUOTENAME(@usuario_bd) + N' DISABLE;';
        EXEC (@sql);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ActivarUsuario
IF OBJECT_ID(N'dbo.sp_ActivarUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActivarUsuario;
GO
CREATE PROCEDURE dbo.sp_ActivarUsuario
    @id_usuario INT
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @usuario_bd VARCHAR(30), @estado CHAR(10);
        SELECT @usuario_bd = usuario_bd, @estado = estado_usuario
        FROM dbo.USUARIOS WHERE id_usuario = @id_usuario;

        IF @usuario_bd IS NULL
            THROW 50032, 'El usuario indicado no existe.', 1;

        IF @estado = 'activo'
            THROW 50034, 'El usuario ya se encuentra activo.', 1;

        UPDATE dbo.USUARIOS SET estado_usuario = 'activo' WHERE id_usuario = @id_usuario;

        DECLARE @sql NVARCHAR(MAX) = N'ALTER LOGIN ' + QUOTENAME(@usuario_bd) + N' ENABLE;';
        EXEC (@sql);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_CambiarClaveUsuario: recibe el hash bcrypt ya calculado en negocio.
IF OBJECT_ID(N'dbo.sp_CambiarClaveUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CambiarClaveUsuario;
GO
CREATE PROCEDURE dbo.sp_CambiarClaveUsuario
    @id_usuario     INT,
    @clave_app_hash VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIOS WHERE id_usuario = @id_usuario)
            THROW 50032, 'El usuario indicado no existe.', 1;

        UPDATE dbo.USUARIOS SET clave_app_hash = @clave_app_hash WHERE id_usuario = @id_usuario;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ActualizarNombreUsuario
IF OBJECT_ID(N'dbo.sp_ActualizarNombreUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ActualizarNombreUsuario;
GO
CREATE PROCEDURE dbo.sp_ActualizarNombreUsuario
    @id_usuario     INT,
    @nombre_usuario NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIOS WHERE id_usuario = @id_usuario)
            THROW 50032, 'El usuario indicado no existe.', 1;

        UPDATE dbo.USUARIOS SET nombre_usuario = @nombre_usuario WHERE id_usuario = @id_usuario;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ConsultarUsuarios: listado de cuentas, sin hashes ni claves.
IF OBJECT_ID(N'dbo.sp_ConsultarUsuarios', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarUsuarios;
GO
CREATE PROCEDURE dbo.sp_ConsultarUsuarios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.id_usuario, u.nombre_usuario, u.usuario_login, r.nombre_rol, u.estado_usuario
    FROM dbo.USUARIOS u
    INNER JOIN dbo.ROLES r ON r.id_rol = u.id_rol
    ORDER BY u.nombre_usuario;
END
GO

-- sp_ObtenerCredencialesLogin: único procedimiento accesible con login_bootstrap.
IF OBJECT_ID(N'dbo.sp_ObtenerCredencialesLogin', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ObtenerCredencialesLogin;
GO
CREATE PROCEDURE dbo.sp_ObtenerCredencialesLogin
    @usuario_login VARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.id_usuario, u.nombre_usuario, u.clave_app_hash, u.usuario_bd, u.clave_bd_enc,
           r.nombre_rol, u.estado_usuario
    FROM dbo.USUARIOS u
    INNER JOIN dbo.ROLES r ON r.id_rol = u.id_rol
    WHERE u.usuario_login = @usuario_login;
END
GO
GRANT EXECUTE ON dbo.sp_ObtenerCredencialesLogin TO login_bootstrap;
GO

-- sp_CrearReserva
IF OBJECT_ID(N'dbo.sp_CrearReserva', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CrearReserva;
GO
IF TYPE_ID(N'dbo.ListaIdsHorario') IS NOT NULL DROP TYPE dbo.ListaIdsHorario;
GO
CREATE TYPE dbo.ListaIdsHorario AS TABLE
(
    id_horario INT NOT NULL PRIMARY KEY
);
GO
CREATE PROCEDURE dbo.sp_CrearReserva
    @id_cliente       INT,
    @horarios         dbo.ListaIdsHorario READONLY,
    @id_usuario       INT,
    @id_reserva_nueva INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @horarios)
            THROW 50015, 'Debe seleccionar al menos una franja horaria.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE id_cliente = @id_cliente)
            THROW 50002, 'El cliente indicado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIOS WHERE id_usuario = @id_usuario AND estado_usuario = 'activo')
            THROW 50033, 'El usuario que registra la reserva no existe o está inactivo.', 1;

        IF EXISTS (
            SELECT 1 FROM @horarios t
            WHERE NOT EXISTS (SELECT 1 FROM dbo.HORARIOS h WHERE h.id_horario = t.id_horario)
        )
            THROW 50005, 'La franja horaria indicada no existe.', 1;

        IF EXISTS (
            SELECT 1
            FROM @horarios t
            INNER JOIN dbo.HORARIOS h ON h.id_horario = t.id_horario
            INNER JOIN dbo.CANCHAS c ON c.id_cancha = h.id_cancha
            WHERE c.estado_cancha <> 'activa'
        )
            THROW 50022, 'La cancha de la franja seleccionada no está activa.', 1;

        IF EXISTS (
            SELECT 1
            FROM @horarios t
            INNER JOIN dbo.HORARIOS h ON h.id_horario = t.id_horario
            WHERE h.fecha_horario < CAST(GETDATE() AS DATE)
        )
            THROW 50006, 'No se pueden registrar reservas con fecha anterior a la actual.', 1;

        IF EXISTS (
            SELECT 1 FROM @horarios t
            WHERE dbo.fn_FranjaOcupada(t.id_horario) = 1
        )
            THROW 50007, 'La franja horaria seleccionada ya se encuentra ocupada.', 1;

        BEGIN TRANSACTION;
            INSERT INTO dbo.RESERVAS (id_cliente, id_horario, id_usuario, estado_reserva)
            SELECT @id_cliente, t.id_horario, @id_usuario, 'activa'
            FROM @horarios t;

            SET @id_reserva_nueva = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- sp_ConsultarReservas
IF OBJECT_ID(N'dbo.sp_ConsultarReservas', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarReservas;
GO
CREATE PROCEDURE dbo.sp_ConsultarReservas
    @fecha      DATE = NULL,
    @id_cliente INT = NULL,
    @id_cancha  INT = NULL,
    @estado     CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        r.id_reserva, c.nombre_cliente, ca.nombre_cancha,
        h.fecha_horario, h.hora_inicio_horario,
        dbo.fn_HoraFinFranja(h.hora_inicio_horario) AS hora_fin_horario,
        u.nombre_usuario AS registrado_por, r.estado_reserva
    FROM dbo.RESERVAS r
    INNER JOIN dbo.CLIENTES c ON c.id_cliente = r.id_cliente
    INNER JOIN dbo.HORARIOS h ON h.id_horario = r.id_horario
    INNER JOIN dbo.CANCHAS ca ON ca.id_cancha = h.id_cancha
    INNER JOIN dbo.USUARIOS u ON u.id_usuario = r.id_usuario
    WHERE (@fecha IS NULL OR h.fecha_horario = @fecha)
      AND (@id_cliente IS NULL OR r.id_cliente = @id_cliente)
      AND (@id_cancha IS NULL OR h.id_cancha = @id_cancha)
      AND (@estado IS NULL OR r.estado_reserva = @estado)
    ORDER BY h.fecha_horario, h.hora_inicio_horario;
END
GO

-- sp_ModificarReservaHorario
IF OBJECT_ID(N'dbo.sp_ModificarReservaHorario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ModificarReservaHorario;
GO
CREATE PROCEDURE dbo.sp_ModificarReservaHorario
    @id_reserva       INT,
    @nuevo_id_horario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @estado_actual CHAR(10);
        SELECT @estado_actual = estado_reserva FROM dbo.RESERVAS WHERE id_reserva = @id_reserva;

        IF @estado_actual IS NULL
            THROW 50008, 'La reserva indicada no existe.', 1;

        IF @estado_actual <> 'activa'
            THROW 50009, 'Solo se puede modificar el horario de una reserva activa.', 1;

        DECLARE @fecha_horario DATE;
        SELECT @fecha_horario = fecha_horario FROM dbo.HORARIOS WHERE id_horario = @nuevo_id_horario;

        IF @fecha_horario IS NULL
            THROW 50005, 'La franja horaria indicada no existe.', 1;

        IF @fecha_horario < CAST(GETDATE() AS DATE)
            THROW 50006, 'No se puede reprogramar la reserva a una fecha anterior a la actual.', 1;

        IF dbo.fn_FranjaOcupada(@nuevo_id_horario) = 1
            THROW 50007, 'La nueva franja horaria seleccionada ya se encuentra ocupada.', 1;

        UPDATE dbo.RESERVAS SET id_horario = @nuevo_id_horario WHERE id_reserva = @id_reserva;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_CancelarReserva
IF OBJECT_ID(N'dbo.sp_CancelarReserva', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CancelarReserva;
GO
CREATE PROCEDURE dbo.sp_CancelarReserva
    @id_reserva INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @estado_actual CHAR(10);
        SELECT @estado_actual = estado_reserva FROM dbo.RESERVAS WHERE id_reserva = @id_reserva;

        IF @estado_actual IS NULL
            THROW 50008, 'La reserva indicada no existe.', 1;

        IF @estado_actual = 'cancelada'
            THROW 50010, 'La reserva ya se encuentra cancelada.', 1;

        UPDATE dbo.RESERVAS SET estado_reserva = 'cancelada' WHERE id_reserva = @id_reserva;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_RegistrarPago
IF OBJECT_ID(N'dbo.sp_RegistrarPago', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarPago;
GO
CREATE PROCEDURE dbo.sp_RegistrarPago
    @id_reserva    INT,
    @monto_pago    DECIMAL(9,2),
    @fecha_pago    DATE,
    @estado_pago   CHAR(10) = 'pagado',
    @id_pago_nuevo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.RESERVAS WHERE id_reserva = @id_reserva AND estado_reserva = 'activa')
            THROW 50011, 'La reserva indicada no existe o no se encuentra activa.', 1;

        IF EXISTS (SELECT 1 FROM dbo.PAGOS WHERE id_reserva = @id_reserva)
            THROW 50012, 'La reserva indicada ya tiene un pago registrado.', 1;

        INSERT INTO dbo.PAGOS (id_reserva, monto_pago, fecha_pago, estado_pago)
        VALUES (@id_reserva, @monto_pago, @fecha_pago, @estado_pago);

        SET @id_pago_nuevo = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- sp_ConsultarEstadoPago
IF OBJECT_ID(N'dbo.sp_ConsultarEstadoPago', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarEstadoPago;
GO
CREATE PROCEDURE dbo.sp_ConsultarEstadoPago
    @fecha      DATE = NULL,
    @id_cliente INT = NULL,
    @id_cancha  INT = NULL,
    @estado     CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        r.id_reserva, c.nombre_cliente, ca.nombre_cancha,
        h.fecha_horario, h.hora_inicio_horario,
        ISNULL(p.estado_pago, 'pendiente') AS estado_pago, p.monto_pago, p.fecha_pago,
        r.estado_reserva
    FROM dbo.RESERVAS r
    INNER JOIN dbo.CLIENTES c ON c.id_cliente = r.id_cliente
    INNER JOIN dbo.HORARIOS h ON h.id_horario = r.id_horario
    INNER JOIN dbo.CANCHAS ca ON ca.id_cancha = h.id_cancha
    LEFT JOIN dbo.PAGOS p ON p.id_reserva = r.id_reserva
    WHERE (@fecha IS NULL OR h.fecha_horario = @fecha)
      AND (@id_cliente IS NULL OR r.id_cliente = @id_cliente)
      AND (@id_cancha IS NULL OR h.id_cancha = @id_cancha)
      AND (@estado IS NULL OR r.estado_reserva = @estado)
    ORDER BY h.fecha_horario, h.hora_inicio_horario;
END
GO

-- sp_ConsultarDisponibilidad
IF OBJECT_ID(N'dbo.sp_ConsultarDisponibilidad', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarDisponibilidad;
GO
CREATE PROCEDURE dbo.sp_ConsultarDisponibilidad
    @id_cancha INT,
    @fecha     DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE id_cancha = @id_cancha)
        THROW 50021, 'La cancha indicada no existe.', 1;

    EXEC dbo.sp_GenerarHorariosDia @id_cancha = @id_cancha, @fecha = @fecha;

    SELECT
        h.id_horario, h.fecha_horario, h.hora_inicio_horario,
        dbo.fn_HoraFinFranja(h.hora_inicio_horario) AS hora_fin_horario,
        CASE WHEN dbo.fn_FranjaOcupada(h.id_horario) = 1 THEN 'ocupada' ELSE 'libre' END AS estado_franja
    FROM dbo.HORARIOS h
    WHERE h.id_cancha = @id_cancha AND h.fecha_horario = @fecha
    ORDER BY h.hora_inicio_horario;
END
GO

-- sp_ConsultarIngresos (uso exclusivo del rol administrador)
IF OBJECT_ID(N'dbo.sp_ConsultarIngresos', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarIngresos;
GO
CREATE PROCEDURE dbo.sp_ConsultarIngresos
    @fecha_inicio DATE,
    @fecha_fin    DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(SUM(monto_pago), 0) AS total_ingresos
    FROM dbo.fn_IngresosDetalle(@fecha_inicio, @fecha_fin);

    SELECT * FROM dbo.fn_IngresosDetalle(@fecha_inicio, @fecha_fin)
    ORDER BY fecha_horario, hora_inicio_horario;
END
GO

------------------------------------------------------------------------------
-- 6. PERMISOS POR ROL
------------------------------------------------------------------------------
GRANT EXECUTE ON dbo.sp_RegistrarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarClientes       TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CrearReserva            TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON TYPE::dbo.ListaIdsHorario      TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarReservas       TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarReservaHorario TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CancelarReserva         TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_RegistrarPago           TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarEstadoPago     TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarDisponibilidad TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_GenerarHorariosDia      TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarCanchas        TO db_rol_empleado, db_rol_administrador;

GRANT EXECUTE ON dbo.sp_EliminarCliente         TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_RegistrarCancha         TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarCancha         TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_DesactivarCancha        TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActivarCancha           TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_RegistrarUsuario        TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_DesactivarUsuario       TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActivarUsuario          TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CambiarClaveUsuario     TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActualizarNombreUsuario TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarUsuarios       TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarIngresos       TO db_rol_administrador;
GO
