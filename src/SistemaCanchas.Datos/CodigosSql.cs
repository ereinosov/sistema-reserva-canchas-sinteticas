namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Códigos THROW del artefacto A11 que la capa de negocio puede traducir.
    /// </summary>
    public static class CodigosSql
    {
        /// <summary>Ya existe usuario_login o usuario_bd (sp_RegistrarUsuario).</summary>
        public const int UsuarioDuplicado = 50030;

        /// <summary>El rol indicado no existe.</summary>
        public const int RolInexistente = 50031;

        /// <summary>El usuario indicado no existe (sp_DesactivarUsuario).</summary>
        public const int UsuarioNoExiste = 50032;

        /// <summary>El usuario ya está activo (sp_ActivarUsuario).</summary>
        public const int UsuarioYaActivo = 50034;

        /// <summary>Nombre de cancha duplicado (sp_RegistrarCancha / sp_ModificarCancha).</summary>
        public const int CanchaDuplicada = 50020;

        /// <summary>La cancha indicada no existe.</summary>
        public const int CanchaNoExiste = 50021;

        /// <summary>La cancha ya está activa (sp_ActivarCancha).</summary>
        public const int CanchaYaActiva = 50023;

        /// <summary>Documento de cliente duplicado (RN07).</summary>
        public const int ClienteDuplicado = 50001;

        /// <summary>El cliente indicado no existe.</summary>
        public const int ClienteNoExiste = 50002;

        /// <summary>No se puede eliminar: reservas activas (RN02).</summary>
        public const int ClienteConReservasActivas = 50003;

        /// <summary>No se puede eliminar: pagos pendientes (RN02).</summary>
        public const int ClienteConPagosPendientes = 50004;

        /// <summary>Teléfono de cliente duplicado.</summary>
        public const int ClienteTelefonoDuplicado = 50013;

        /// <summary>Correo de cliente duplicado.</summary>
        public const int ClienteCorreoDuplicado = 50014;

        /// <summary>La franja horaria indicada no existe.</summary>
        public const int FranjaNoExiste = 50005;

        /// <summary>Fecha de reserva anterior a la actual (RN06).</summary>
        public const int FechaReservaAnterior = 50006;

        /// <summary>Franja ocupada (RN01 / RN08).</summary>
        public const int FranjaOcupada = 50007;

        /// <summary>La reserva indicada no existe.</summary>
        public const int ReservaNoExiste = 50008;

        /// <summary>Solo se modifica el horario de una reserva activa.</summary>
        public const int ReservaNoActivaParaModificar = 50009;

        /// <summary>La reserva ya está cancelada (RN03).</summary>
        public const int ReservaYaCancelada = 50010;

        /// <summary>La reserva no existe o no está activa (sp_RegistrarPago).</summary>
        public const int ReservaNoActivaParaPago = 50011;

        /// <summary>La reserva ya tiene un pago (RN04).</summary>
        public const int PagoYaRegistrado = 50012;

        /// <summary>La cancha de la franja no está activa (RN10).</summary>
        public const int CanchaNoActiva = 50022;

        /// <summary>El usuario que registra la reserva no existe o está inactivo.</summary>
        public const int UsuarioRegistroInactivo = 50033;

        /// <summary>Violación de índice único (p. ej. UQ_RESERVAS_horario_activa).</summary>
        public const int IndiceUnicoDuplicado = 2601;

        /// <summary>Violación de restricción UNIQUE.</summary>
        public const int RestriccionUnicaDuplicada = 2627;
    }
}
