/* ============================================================================
   UNIVERSIDAD TÉCNICA ESTATAL DE QUEVEDO
   FACULTAD DE CIENCIAS DE LA COMPUTACIÓN
   CARRERA INGENIERÍA EN SOFTWARE
   PROCESO DE SOFTWARE "A"

   ARTEFACTO A11: Script DDL de creación de tablas (SQL Server)
   PROYECTO: Sistema de Reserva de Canchas Sintéticas

   PRESENTADO POR:
   Calderón Saltos Joseph Alexander
   Herrera Barco Humberto Aldair
   Reinoso Vélez Eduardo David
   Silva Triviño John Jairo

   Fuente: Artefacto A1 (Especificación de Requisitos, corregida) y Artefacto A7
   (Modelo Físico normalizado a 3FN, corregido: soporte multicancha, usuarios
   y roles).

   Contenido:
     0. Creación de la base de datos
     1. Tablas, claves y restricciones (según modelo físico A7 corregido)
     2. Roles de base de datos y login de arranque (bootstrap)
     3. Índice único filtrado para regla de negocio RN01
     4. Funciones escalares y de tabla
     5. Procedimientos almacenados (uno por cada requisito funcional RF01-RF16)

   NOTA DE ARQUITECTURA DE SEGURIDAD (Opción A del A1, sección 2.4):
   Cada persona posee su propio login individual de SQL Server. La app NO
   pide directamente el usuario/clave de SQL Server a la persona; en su
   lugar, USUARIOS guarda credenciales de aplicación (usuario_login +
   clave_app_hash) desacopladas del login real de motor (usuario_bd +
   clave_bd_enc). Para poder leer esa fila ANTES de que la persona esté
   autenticada, existe un login de arranque (login_bootstrap) de privilegio
   mínimo, con permiso de EXECUTE únicamente sobre sp_ObtenerCredencialesLogin.
   Este punto debe reflejarse también en el A1 (sección 2.5, supuestos) y
   en el A9 (diagrama de secuencia de inicio de sesión).
   ============================================================================ */

------------------------------------------------------------------------------
-- 0. CREACIÓN DE LA BASE DE DATOS
------------------------------------------------------------------------------
IF DB_ID(N'ReservaCanchasDB') IS NULL
BEGIN
    CREATE DATABASE ReservaCanchasDB;
END
GO

USE ReservaCanchasDB;
GO

-- El índice filtrado UQ_RESERVAS_horario_activa exige QUOTED_IDENTIFIER ON en
-- el lote que crea los procedimientos que hacen INSERT/UPDATE sobre RESERVAS.
-- sqlcmd deja esa opción en OFF salvo que se invoque con -I.
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Necesario para que sp_RegistrarUsuario (EXECUTE AS OWNER) pueda ejecutar
-- CREATE LOGIN y ALTER SERVER ROLE en LocalDB / instancias de desarrollo.
-- El propietario de la base debe ser sysadmin (cuenta de Windows que ejecutó A11).
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ReservaCanchasDB' AND is_trustworthy_on = 0)
    ALTER DATABASE ReservaCanchasDB SET TRUSTWORTHY ON;
GO

------------------------------------------------------------------------------
-- 1. TABLAS, CLAVES Y RESTRICCIONES
--    (nombres, tipos y constraints tomados literalmente del Artefacto A7)
------------------------------------------------------------------------------

-- 1.1 CLIENTES ----------------------------------------------------------------
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

    -- RN07: un cliente se identifica por tipo + número de documento, único en el sistema.
    CONSTRAINT UQ_CLIENTES_documento UNIQUE (tipo_documento_cliente, numero_documento_cliente),

    -- RN07: tipos de documento admitidos. CHAR porque es un catálogo cerrado
    -- verificado por CHECK, no texto libre (criterio del A7).
    CONSTRAINT CK_CLIENTES_tipo_documento CHECK (tipo_documento_cliente IN ('cedula','pasaporte','ruc')),

    -- RN07: si el tipo es cédula, el número debe tener exactamente 10 dígitos numéricos.
    CONSTRAINT CK_CLIENTES_numero_documento CHECK (
        tipo_documento_cliente <> 'cedula'
        OR (LEN(numero_documento_cliente) = 10 AND numero_documento_cliente NOT LIKE '%[^0-9]%')
    )
);
GO

-- 1.2 ROLES ---------------------------------------------------------------------
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

-- 1.3 USUARIOS --------------------------------------------------------------------
IF OBJECT_ID(N'dbo.USUARIOS', N'U') IS NOT NULL DROP TABLE dbo.USUARIOS;
GO

CREATE TABLE dbo.USUARIOS
(
    id_usuario       INT IDENTITY(1,1) NOT NULL,
    nombre_usuario    NVARCHAR(100)     NOT NULL,
    usuario_login     VARCHAR(30)       NOT NULL,  -- credencial de aplicación (lo que la persona escribe)
    clave_app_hash    VARCHAR(255)      NOT NULL,  -- hash (bcrypt) de la clave de aplicación
    usuario_bd        VARCHAR(30)       NOT NULL,  -- nombre del login real de SQL Server
    clave_bd_enc      VARCHAR(255)      NOT NULL,  -- clave del login de SQL Server, cifrada con AES (System.Security.Cryptography), distinto del DPAPI de RNF12
    id_rol            INT               NOT NULL,
    estado_usuario    CHAR(10)          NOT NULL CONSTRAINT DF_USUARIOS_estado DEFAULT ('activo'),

    CONSTRAINT PK_USUARIOS PRIMARY KEY (id_usuario),
    CONSTRAINT UQ_USUARIOS_login UNIQUE (usuario_login),
    CONSTRAINT UQ_USUARIOS_usuario_bd UNIQUE (usuario_bd),

    CONSTRAINT FK_USUARIOS_ROLES FOREIGN KEY (id_rol)
        REFERENCES dbo.ROLES (id_rol) ON DELETE NO ACTION,

    CONSTRAINT CK_USUARIOS_estado CHECK (estado_usuario IN ('activo','inactivo'))
);
GO

-- 1.4 CANCHAS -----------------------------------------------------------------
IF OBJECT_ID(N'dbo.CANCHAS', N'U') IS NOT NULL DROP TABLE dbo.CANCHAS;
GO

CREATE TABLE dbo.CANCHAS
(
    id_cancha       INT IDENTITY(1,1) NOT NULL,
    nombre_cancha    NVARCHAR(60)      NOT NULL,
    estado_cancha    CHAR(10)          NOT NULL CONSTRAINT DF_CANCHAS_estado DEFAULT ('activa'),

    CONSTRAINT PK_CANCHAS PRIMARY KEY (id_cancha),
    CONSTRAINT UQ_CANCHAS_nombre UNIQUE (nombre_cancha),
    CONSTRAINT CK_CANCHAS_estado CHECK (estado_cancha IN ('activa','inactiva'))
);
GO

-- 1.5 HORARIOS ------------------------------------------------------------------
IF OBJECT_ID(N'dbo.HORARIOS', N'U') IS NOT NULL DROP TABLE dbo.HORARIOS;
GO

CREATE TABLE dbo.HORARIOS
(
    id_horario           INT IDENTITY(1,1) NOT NULL,
    id_cancha             INT               NOT NULL,
    fecha_horario         DATE              NOT NULL,
    hora_inicio_horario   TIME(0)           NOT NULL,

    CONSTRAINT PK_HORARIOS PRIMARY KEY (id_horario),

    CONSTRAINT FK_HORARIOS_CANCHAS FOREIGN KEY (id_cancha)
        REFERENCES dbo.CANCHAS (id_cancha) ON DELETE NO ACTION,

    -- Una misma cancha no puede repetir la misma fecha y hora de inicio.
    CONSTRAINT UQ_HORARIOS_franja UNIQUE (id_cancha, fecha_horario, hora_inicio_horario),

    -- RN05: franjas fijas de una hora entre 06:00 y 21:00 (última franja termina a las 22:00).
    CONSTRAINT CK_HORARIOS_rango CHECK (hora_inicio_horario BETWEEN '06:00:00' AND '21:00:00'),

    -- RN05: la hora de inicio debe caer en punto (minutos = 0); la hora de fin es un dato
    -- derivado (no se almacena) y se calcula con dbo.fn_HoraFinFranja.
    CONSTRAINT CK_HORARIOS_minuto CHECK (DATEPART(MINUTE, hora_inicio_horario) = 0)
);
GO

-- 1.6 RESERVAS --------------------------------------------------------------------
IF OBJECT_ID(N'dbo.RESERVAS', N'U') IS NOT NULL DROP TABLE dbo.RESERVAS;
GO

CREATE TABLE dbo.RESERVAS
(
    id_reserva       INT IDENTITY(1,1) NOT NULL,
    id_cliente        INT               NOT NULL,
    id_horario        INT               NOT NULL,
    id_usuario        INT               NOT NULL,  -- quién registró la reserva (auditoría)
    estado_reserva    CHAR(10)          NOT NULL CONSTRAINT DF_RESERVAS_estado DEFAULT ('activa'),

    CONSTRAINT PK_RESERVAS PRIMARY KEY (id_reserva),

    CONSTRAINT FK_RESERVAS_CLIENTES FOREIGN KEY (id_cliente)
        REFERENCES dbo.CLIENTES (id_cliente) ON DELETE NO ACTION,

    CONSTRAINT FK_RESERVAS_HORARIOS FOREIGN KEY (id_horario)
        REFERENCES dbo.HORARIOS (id_horario) ON DELETE NO ACTION,

    CONSTRAINT FK_RESERVAS_USUARIOS FOREIGN KEY (id_usuario)
        REFERENCES dbo.USUARIOS (id_usuario) ON DELETE NO ACTION,

    CONSTRAINT CK_RESERVAS_estado CHECK (estado_reserva IN ('activa','cancelada'))
);
GO

-- 1.7 PAGOS -------------------------------------------------------------------------
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

    -- RN04: una reserva admite a lo sumo un pago registrado.
    CONSTRAINT UQ_PAGOS_reserva UNIQUE (id_reserva),

    CONSTRAINT FK_PAGOS_RESERVAS FOREIGN KEY (id_reserva)
        REFERENCES dbo.RESERVAS (id_reserva) ON DELETE NO ACTION,

    -- RN04: el monto del pago debe ser mayor a cero.
    CONSTRAINT CK_PAGOS_monto CHECK (monto_pago > 0),

    CONSTRAINT CK_PAGOS_estado CHECK (estado_pago IN ('pendiente','pagado'))
);
GO

------------------------------------------------------------------------------
-- 2. ROLES DE BASE DE DATOS Y LOGIN DE ARRANQUE (BOOTSTRAP)
--    Conforme a RNF13/RNF14 del A1: privilegio mínimo por rol, con la
--    excepción acotada y justificada del rol securityadmin para el admin.
------------------------------------------------------------------------------

-- 2.1 Roles de base de datos para empleado y administrador.
IF DATABASE_PRINCIPAL_ID('db_rol_empleado') IS NULL
    CREATE ROLE db_rol_empleado;
GO
IF DATABASE_PRINCIPAL_ID('db_rol_administrador') IS NULL
    CREATE ROLE db_rol_administrador;
GO

-- 2.2 Login y usuario de arranque (bootstrap): privilegio mínimo posible,
--     solo puede ejecutar sp_ObtenerCredencialesLogin. Es el único login
--     compartido por todas las instalaciones de la aplicación; con él se
--     resuelve la pantalla de inicio de sesión ANTES de que la persona
--     esté autenticada con su propio login individual.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'login_bootstrap')
    CREATE LOGIN login_bootstrap WITH PASSWORD = N'CAMBIAR_EN_DESPLIEGUE#2026', CHECK_POLICY = ON;
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'login_bootstrap')
    CREATE USER login_bootstrap FOR LOGIN login_bootstrap;
GO
-- El GRANT EXECUTE puntual sobre sp_ObtenerCredencialesLogin se otorga en la
-- sección 5, inmediatamente después de crear ese procedimiento.

------------------------------------------------------------------------------
-- 3. ÍNDICE ÚNICO FILTRADO — RN01
--    No pueden existir dos reservas activas sobre la misma franja horaria.
--    Se hace cumplir de forma declarativa (no depende de la capa de presentación),
--    conforme a RNF10 (Seguridad — Integridad). Al pertenecer cada horario a una
--    sola cancha (FK_HORARIOS_CANCHAS), la unicidad ya queda acotada por cancha.
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

-- 4.1 fn_HoraFinFranja — RN05
IF OBJECT_ID(N'dbo.fn_HoraFinFranja', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_HoraFinFranja;
GO
CREATE FUNCTION dbo.fn_HoraFinFranja (@hora_inicio TIME(0))
RETURNS TIME(0)
AS
BEGIN
    RETURN CAST(DATEADD(HOUR, 1, @hora_inicio) AS TIME(0));
END
GO

-- 4.2 fn_FranjaOcupada — RN08
IF OBJECT_ID(N'dbo.fn_FranjaOcupada', N'FN') IS NOT NULL DROP FUNCTION dbo.fn_FranjaOcupada;
GO
CREATE FUNCTION dbo.fn_FranjaOcupada (@id_horario INT)
RETURNS BIT
AS
BEGIN
    DECLARE @ocupada BIT = 0;

    IF EXISTS (
        SELECT 1 FROM dbo.RESERVAS
        WHERE id_horario = @id_horario AND estado_reserva = 'activa'
    )
        SET @ocupada = 1;

    RETURN @ocupada;
END
GO

-- 4.3 fn_IngresosDetalle — RF12 / RN09
IF OBJECT_ID(N'dbo.fn_IngresosDetalle', N'IF') IS NOT NULL DROP FUNCTION dbo.fn_IngresosDetalle;
GO
CREATE FUNCTION dbo.fn_IngresosDetalle (@fecha_inicio DATE, @fecha_fin DATE)
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.id_pago,
        r.id_reserva,
        c.nombre_cliente,
        ca.nombre_cancha,
        h.fecha_horario,
        h.hora_inicio_horario,
        p.monto_pago,
        p.fecha_pago
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
--    Un procedimiento por cada requisito funcional (RF01-RF16) del Artefacto A1.
--    Todos usan parámetros tipados (RNF11) y devuelven mensajes de error
--    controlados mediante THROW, sin exponer el error nativo del motor.
------------------------------------------------------------------------------

-- 5.0 sp_GenerarHorariosDia
-- Procedimiento de soporte (no corresponde a un RF directo): genera las 16
-- franjas horarias fijas de una fecha (06:00 a 21:00) para una cancha dada,
-- si aún no existen, conforme a RN05.
IF OBJECT_ID(N'dbo.sp_GenerarHorariosDia', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_GenerarHorariosDia;
GO
CREATE PROCEDURE dbo.sp_GenerarHorariosDia
    @id_cancha INT,
    @fecha     DATE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Horas AS (
        SELECT CAST('06:00:00' AS TIME(0)) AS hora_inicio
        UNION ALL
        SELECT DATEADD(HOUR, 1, hora_inicio) FROM Horas WHERE hora_inicio < '21:00:00'
    )
    INSERT INTO dbo.HORARIOS (id_cancha, fecha_horario, hora_inicio_horario)
    SELECT @id_cancha, @fecha, h.hora_inicio
    FROM Horas h
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.HORARIOS x
        WHERE x.id_cancha = @id_cancha AND x.fecha_horario = @fecha AND x.hora_inicio_horario = h.hora_inicio
    )
    OPTION (MAXRECURSION 16);
END
GO

-- 5.1 sp_RegistrarCliente — RF01
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
        IF EXISTS (
            SELECT 1 FROM dbo.CLIENTES
            WHERE tipo_documento_cliente = @tipo_documento_cliente
              AND numero_documento_cliente = @numero_documento_cliente
        )
            THROW 50001, 'Ya existe un cliente registrado con ese tipo y número de documento.', 1;

        INSERT INTO dbo.CLIENTES (nombre_cliente, tipo_documento_cliente, numero_documento_cliente, telefono_cliente, correo_cliente)
        VALUES (@nombre_cliente, @tipo_documento_cliente, @numero_documento_cliente, @telefono_cliente, @correo_cliente);

        SET @id_cliente_nuevo = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 5.2 sp_ConsultarClientes — RF02
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

-- 5.3 sp_ModificarCliente — RF03
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

-- 5.4 sp_EliminarCliente — RF04 / RN02
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

        IF EXISTS (
            SELECT 1 FROM dbo.RESERVAS
            WHERE id_cliente = @id_cliente AND estado_reserva = 'activa'
        )
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

-- 5.5 sp_RegistrarCancha — RF13
IF OBJECT_ID(N'dbo.sp_RegistrarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarCancha;
GO
CREATE PROCEDURE dbo.sp_RegistrarCancha
    @nombre_cancha   NVARCHAR(60),
    @id_cancha_nueva INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE nombre_cancha = @nombre_cancha)
            THROW 50020, 'Ya existe una cancha registrada con ese nombre.', 1;

        INSERT INTO dbo.CANCHAS (nombre_cancha) VALUES (@nombre_cancha);
        SET @id_cancha_nueva = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 5.6 sp_ConsultarCanchas — RF13
IF OBJECT_ID(N'dbo.sp_ConsultarCanchas', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarCanchas;
GO
CREATE PROCEDURE dbo.sp_ConsultarCanchas
    @estado_cancha CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_cancha, nombre_cancha, estado_cancha
    FROM dbo.CANCHAS
    WHERE (@estado_cancha IS NULL OR estado_cancha = @estado_cancha)
    ORDER BY nombre_cancha;
END
GO

-- 5.7 sp_ModificarCancha — RF13
IF OBJECT_ID(N'dbo.sp_ModificarCancha', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ModificarCancha;
GO
CREATE PROCEDURE dbo.sp_ModificarCancha
    @id_cancha     INT,
    @nombre_cancha NVARCHAR(60)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE id_cancha = @id_cancha)
            THROW 50021, 'La cancha indicada no existe.', 1;

        IF EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE nombre_cancha = @nombre_cancha AND id_cancha <> @id_cancha)
            THROW 50020, 'Ya existe otra cancha registrada con ese nombre.', 1;

        UPDATE dbo.CANCHAS SET nombre_cancha = @nombre_cancha WHERE id_cancha = @id_cancha;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 5.8 sp_DesactivarCancha — RF13 / RN10
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

-- 5.9 sp_RegistrarUsuario — RF14
-- Crea la fila de USUARIOS y, con EXECUTE AS OWNER, el login real de SQL
-- Server correspondiente (RNF14: excepción acotada de privilegio para el
-- rol securityadmin del perfil administrador). La clave de aplicación llega
-- ya hasheada (bcrypt) desde la capa de negocio; la clave de SQL Server la
-- genera este procedimiento y la devuelve cifrada para que la app la guarde.
IF OBJECT_ID(N'dbo.sp_RegistrarUsuario', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarUsuario;
GO
CREATE PROCEDURE dbo.sp_RegistrarUsuario
    @nombre_usuario   NVARCHAR(100),
    @usuario_login    VARCHAR(30),
    @clave_app_hash   VARCHAR(255),
    @usuario_bd       VARCHAR(30),
    @clave_bd_plana   VARCHAR(128),  -- solo viaja dentro de esta transacción; nunca se persiste en claro
    @clave_bd_enc     VARCHAR(255),  -- ya cifrada por la capa de negocio con AES (System.Security.Cryptography), se persiste tal cual
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

-- 5.10 sp_DesactivarUsuario — RF15 / RN12
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

-- 5.10b sp_ConsultarUsuarios — RF14 / RF15 (listado de cuentas; sin hashes ni claves)
IF OBJECT_ID(N'dbo.sp_ConsultarUsuarios', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarUsuarios;
GO
CREATE PROCEDURE dbo.sp_ConsultarUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.id_usuario,
        u.nombre_usuario,
        u.usuario_login,
        r.nombre_rol,
        u.estado_usuario
    FROM dbo.USUARIOS u
    INNER JOIN dbo.ROLES r ON r.id_rol = u.id_rol
    ORDER BY u.nombre_usuario;
END
GO

-- 5.11 sp_ObtenerCredencialesLogin — RF16
-- Único procedimiento accesible con el login de arranque (login_bootstrap).
-- Devuelve lo necesario para que la capa de negocio verifique la clave de
-- aplicación (bcrypt) y descifre la clave del login real de SQL Server.
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

-- Privilegio mínimo del login de arranque: solo puede ejecutar este procedimiento.
GRANT EXECUTE ON dbo.sp_ObtenerCredencialesLogin TO login_bootstrap;
GO

-- 5.12 sp_CrearReserva — RF05 / RN01 / RN06 / RN08
IF OBJECT_ID(N'dbo.sp_CrearReserva', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_CrearReserva;
GO
CREATE PROCEDURE dbo.sp_CrearReserva
    @id_cliente       INT,
    @id_horario       INT,
    @id_usuario       INT,
    @id_reserva_nueva INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.CLIENTES WHERE id_cliente = @id_cliente)
            THROW 50002, 'El cliente indicado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.USUARIOS WHERE id_usuario = @id_usuario AND estado_usuario = 'activo')
            THROW 50033, 'El usuario que registra la reserva no existe o está inactivo.', 1;

        DECLARE @fecha_horario DATE, @id_cancha INT;
        SELECT @fecha_horario = fecha_horario, @id_cancha = id_cancha FROM dbo.HORARIOS WHERE id_horario = @id_horario;

        IF @fecha_horario IS NULL
            THROW 50005, 'La franja horaria indicada no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.CANCHAS WHERE id_cancha = @id_cancha AND estado_cancha = 'activa')
            THROW 50022, 'La cancha de la franja seleccionada no está activa.', 1;

        -- RN06: no se permiten reservas con fecha anterior a la fecha actual.
        IF @fecha_horario < CAST(GETDATE() AS DATE)
            THROW 50006, 'No se pueden registrar reservas con fecha anterior a la actual.', 1;

        -- RN01 / RN08: la franja debe estar libre (validación de aplicación;
        -- la integridad final la garantiza el índice único filtrado UQ_RESERVAS_horario_activa).
        IF dbo.fn_FranjaOcupada(@id_horario) = 1
            THROW 50007, 'La franja horaria seleccionada ya se encuentra ocupada.', 1;

        BEGIN TRANSACTION;
            INSERT INTO dbo.RESERVAS (id_cliente, id_horario, id_usuario, estado_reserva)
            VALUES (@id_cliente, @id_horario, @id_usuario, 'activa');

            SET @id_reserva_nueva = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 5.13 sp_ConsultarReservas — RF06
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
        r.id_reserva,
        c.nombre_cliente,
        ca.nombre_cancha,
        h.fecha_horario,
        h.hora_inicio_horario,
        dbo.fn_HoraFinFranja(h.hora_inicio_horario) AS hora_fin_horario,
        u.nombre_usuario AS registrado_por,
        r.estado_reserva
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

-- 5.14 sp_ModificarReservaHorario — RF07
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

        UPDATE dbo.RESERVAS
        SET id_horario = @nuevo_id_horario
        WHERE id_reserva = @id_reserva;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 5.15 sp_CancelarReserva — RF08 / RN03
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

        UPDATE dbo.RESERVAS
        SET estado_reserva = 'cancelada'
        WHERE id_reserva = @id_reserva;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 5.16 sp_RegistrarPago — RF09 / RN04
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

-- 5.17 sp_ConsultarEstadoPago — RF10
IF OBJECT_ID(N'dbo.sp_ConsultarEstadoPago', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ConsultarEstadoPago;
GO
CREATE PROCEDURE dbo.sp_ConsultarEstadoPago
    @id_reserva INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.id_reserva,
        c.nombre_cliente,
        h.fecha_horario,
        h.hora_inicio_horario,
        ISNULL(p.estado_pago, 'pendiente') AS estado_pago,
        p.monto_pago,
        p.fecha_pago
    FROM dbo.RESERVAS r
    INNER JOIN dbo.CLIENTES c ON c.id_cliente = r.id_cliente
    INNER JOIN dbo.HORARIOS h ON h.id_horario = r.id_horario
    LEFT JOIN dbo.PAGOS p ON p.id_reserva = r.id_reserva
    WHERE r.estado_reserva = 'activa'
      AND (@id_reserva IS NULL OR r.id_reserva = @id_reserva)
    ORDER BY h.fecha_horario, h.hora_inicio_horario;
END
GO

-- 5.18 sp_ConsultarDisponibilidad — RF11 / RN08
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

    -- Asegura que las 16 franjas de la fecha consultada existan para esa cancha (RN05).
    EXEC dbo.sp_GenerarHorariosDia @id_cancha = @id_cancha, @fecha = @fecha;

    SELECT
        h.id_horario,
        h.fecha_horario,
        h.hora_inicio_horario,
        dbo.fn_HoraFinFranja(h.hora_inicio_horario) AS hora_fin_horario,
        CASE WHEN dbo.fn_FranjaOcupada(h.id_horario) = 1 THEN 'ocupada' ELSE 'libre' END AS estado_franja
    FROM dbo.HORARIOS h
    WHERE h.id_cancha = @id_cancha AND h.fecha_horario = @fecha
    ORDER BY h.hora_inicio_horario;
END
GO

-- 5.19 sp_ConsultarIngresos — RF12 / RN09  (uso exclusivo del rol administrador)
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

    SELECT *
    FROM dbo.fn_IngresosDetalle(@fecha_inicio, @fecha_fin)
    ORDER BY fecha_horario, hora_inicio_horario;
END
GO

------------------------------------------------------------------------------
-- 6. PERMISOS POR ROL (privilegio mínimo — RNF13/RNF14)
--    El rol empleado NO recibe permiso sobre: sp_EliminarCliente,
--    sp_RegistrarCancha/ModificarCancha/DesactivarCancha, sp_RegistrarUsuario,
--    sp_DesactivarUsuario, sp_ConsultarIngresos (todo esto es exclusivo de
--    db_rol_administrador, conforme a la sección 2.3 del A1).
------------------------------------------------------------------------------
GRANT EXECUTE ON dbo.sp_RegistrarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarClientes       TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CrearReserva            TO db_rol_empleado, db_rol_administrador;
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
GRANT EXECUTE ON dbo.sp_RegistrarUsuario        TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_DesactivarUsuario       TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarUsuarios       TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarIngresos       TO db_rol_administrador;
GO

/* ============================================================================
   FIN DEL ARTEFACTO A11.
   Trazabilidad: RF01-RF16 y RN01-RN12 del Artefacto A1 (corregido); estructura
   de tablas y tipos de dato tomados del Artefacto A7 (Modelo Físico, corregido
   con soporte multicancha, usuarios y roles).

   PENDIENTE A DOCUMENTAR EN A1/A9: el login_bootstrap y el flujo de dos
   conexiones (arranque → sp_ObtenerCredencialesLogin → reconexión con el
   login individual real) deben quedar explícitos en la sección 2.5
   (Suposiciones) del A1 y en el diagrama de secuencia de inicio de sesión (A9).
   ============================================================================ */
