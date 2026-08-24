using System;

namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.RESERVAS (A7 / A11).
    /// </summary>
    public class Reserva
    {
        public int IdReserva { get; set; }

        public int IdCliente { get; set; }

        public int IdHorario { get; set; }

        /// <summary>
        /// Usuario que registró la reserva. Obligatorio y no se modifica después de creada (RN13).
        /// El diagrama A8 omite este campo; A1 y A11 lo exigen.
        /// </summary>
        public int IdUsuario { get; set; }

        public string EstadoReserva { get; set; }

        /// <summary>Nombre del cliente (JOIN de sp_ConsultarReservas). No es columna de RESERVAS.</summary>
        public string NombreCliente { get; set; }

        /// <summary>Nombre de la cancha (JOIN de sp_ConsultarReservas). No es columna de RESERVAS.</summary>
        public string NombreCancha { get; set; }

        /// <summary>Fecha de la franja. Viene del JOIN con HORARIOS.</summary>
        public DateTime FechaHorario { get; set; }

        /// <summary>Hora de inicio de la franja. Viene del JOIN con HORARIOS.</summary>
        public TimeSpan HoraInicioHorario { get; set; }

        /// <summary>Hora de fin derivada (RN05).</summary>
        public TimeSpan HoraFinHorario { get; set; }

        /// <summary>Usuario que registró la reserva (RN13). Viene del JOIN con USUARIOS.</summary>
        public string RegistradoPor { get; set; }
    }
}
