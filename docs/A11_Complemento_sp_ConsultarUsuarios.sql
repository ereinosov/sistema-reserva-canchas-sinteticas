/* ============================================================================
   Complemento A11: sp_ConsultarUsuarios
   Ejecutar SOLO si ya aplicó el script A11 original y no quiere recrear la BD.
   Si va a ejecutar A11 completo (base nueva), este archivo no es necesario.
   ============================================================================ */
USE ReservaCanchasDB;
GO

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

GRANT EXECUTE ON dbo.sp_ConsultarUsuarios TO db_rol_administrador;
GO
