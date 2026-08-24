-- Complemento A11: permiso sobre el procedimiento de soporte que genera las 16 franjas (RN05).
-- sp_ConsultarDisponibilidad lo invoca con EXEC; sin este GRANT el empleado/administrador
-- reciben error de permisos al consultar disponibilidad.
-- Ejecutar en SSMS contra ReservaCanchasDB. NO vuelve a crear tablas.

USE ReservaCanchasDB;
GO

GRANT EXECUTE ON dbo.sp_GenerarHorariosDia TO db_rol_empleado, db_rol_administrador;
GO
