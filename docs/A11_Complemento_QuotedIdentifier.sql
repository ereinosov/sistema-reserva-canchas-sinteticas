/* ============================================================================
   Complemento A11: QUOTED_IDENTIFIER en procedimientos de reservas
   Causa: el índice filtrado UQ_RESERVAS_horario_activa exige SET QUOTED_IDENTIFIER ON.
   Si A11 se ejecutó con sqlcmd (opción OFF por defecto), Registrar reserva falla con:
   INSERT failed ... SET options have incorrect settings: 'QUOTED_IDENTIFIER'.

   Ejecutar en SSMS (o sqlcmd -I) contra ReservaCanchasDB.
   NO vuelve a crear tablas ni borra datos.
   ============================================================================ */

USE ReservaCanchasDB;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

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

GRANT EXECUTE ON dbo.sp_CrearReserva            TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_ModificarReservaHorario TO db_rol_empleado, db_rol_administrador;
GRANT EXECUTE ON dbo.sp_CancelarReserva         TO db_rol_empleado, db_rol_administrador;
GO
