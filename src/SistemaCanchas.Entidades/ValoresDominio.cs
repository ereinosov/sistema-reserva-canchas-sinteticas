namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Valores literales persistidos en SQL Server (artefacto A11).
    /// Las columnas CHAR se rellenan con espacios en el motor; al leerlas hay que recortar.
    /// </summary>
    public static class ValoresDominio
    {
        /// <summary>Duración fija de cada franja, en minutos (RN05).</summary>
        public const int DuracionTurnoMinutos = 60;

        /// <summary>Hora de inicio de la primera franja del día (RN05).</summary>
        public const int HoraInicioFranja = 6;

        /// <summary>Hora de inicio de la última franja; esa franja termina a las 22:00 (RN05).</summary>
        public const int HoraInicioUltimaFranja = 21;

        /// <summary>Hora de cierre del establecimiento (RN05).</summary>
        public const int HoraFinOperacion = 22;

        /// <summary>Longitud máxima de usuario_login (A11: VARCHAR(30)).</summary>
        public const int LongitudMaximaUsuarioLogin = 30;

        /// <summary>Longitud mínima de usuario_login (debe empezar por letra).</summary>
        public const int LongitudMinimaUsuarioLogin = 3;

        /// <summary>Longitud máxima de nombre_usuario (A11: NVARCHAR(100)).</summary>
        public const int LongitudMaximaNombreUsuario = 100;

        /// <summary>Longitud mínima de la clave de aplicación (validación de negocio).</summary>
        public const int LongitudMinimaClaveApp = 8;

        /// <summary>Costo bcrypt de clave_app_hash (A12 §10.1).</summary>
        public const int CostoHashAplicacion = 12;

        /// <summary>Longitud máxima de nombre_cancha (A11: NVARCHAR(60)).</summary>
        public const int LongitudMaximaNombreCancha = 60;

        /// <summary>Longitud máxima de nombre_cliente (A11: NVARCHAR(100)).</summary>
        public const int LongitudMaximaNombreCliente = 100;

        /// <summary>Longitud máxima de numero_documento_cliente (A11: VARCHAR(20)).</summary>
        public const int LongitudMaximaNumeroDocumento = 20;

        /// <summary>Longitud de cédula (RN07).</summary>
        public const int LongitudCedula = 10;

        /// <summary>Longitud máxima de telefono_cliente (A11: VARCHAR(15)).</summary>
        public const int LongitudMaximaTelefono = 15;

        /// <summary>Longitud máxima de correo_cliente (A11: VARCHAR(100)).</summary>
        public const int LongitudMaximaCorreo = 100;

        /// <summary>Tipos de documento admitidos (RN07).</summary>
        public static class TipoDocumento
        {
            public const string Cedula = "cedula";
            public const string Pasaporte = "pasaporte";
            public const string Ruc = "ruc";
        }

        /// <summary>Roles de aplicación (RN11).</summary>
        public static class Rol
        {
            public const string Administrador = "administrador";
            public const string Empleado = "empleado";
        }

        /// <summary>Estados de USUARIOS.</summary>
        public static class EstadoUsuario
        {
            public const string Activo = "activo";
            public const string Inactivo = "inactivo";
        }

        /// <summary>Estados de CANCHAS (RN10).</summary>
        public static class EstadoCancha
        {
            public const string Activa = "activa";
            public const string Inactiva = "inactiva";
        }

        /// <summary>Estados de RESERVAS (RN01, RN03).</summary>
        public static class EstadoReserva
        {
            public const string Activa = "activa";
            public const string Cancelada = "cancelada";
        }

        /// <summary>Estado de una franja en sp_ConsultarDisponibilidad (RN08).</summary>
        public static class EstadoFranja
        {
            public const string Libre = "libre";
            public const string Ocupada = "ocupada";
        }

        /// <summary>Monto máximo de monto_pago (A11: DECIMAL(9,2)).</summary>
        public const decimal MontoPagoMaximo = 9999999.99m;

        /// <summary>Estados de PAGOS (RN04, RN09).</summary>
        public static class EstadoPago
        {
            public const string Pendiente = "pendiente";
            public const string Pagado = "pagado";
        }
    }
}
