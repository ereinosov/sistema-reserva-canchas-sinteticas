/* ============================================================================
   Complemento A11: reactivar cancha/usuario, clave y nombre, unicidad de
   teléfono/correo y filtros de sp_ConsultarEstadoPago.
   Ejecutar SOLO si ya aplicó A11 y no quiere recrear la BD.
   ============================================================================ */
USE ReservaCanchasDB;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'UQ_CLIENTES_telefono')
    ALTER TABLE dbo.CLIENTES ADD CONSTRAINT UQ_CLIENTES_telefono UNIQUE (telefono_cliente);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'UQ_CLIENTES_correo')
    ALTER TABLE dbo.CLIENTES ADD CONSTRAINT UQ_CLIENTES_correo UNIQUE (correo_cliente);
GO

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

GRANT EXECUTE ON dbo.sp_RegistrarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarCliente        TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActivarCancha           TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActivarUsuario          TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CambiarClaveUsuario     TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ActualizarNombreUsuario TO db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ConsultarEstadoPago     TO db_rol_empleado, db_rol_administrador;
GO
