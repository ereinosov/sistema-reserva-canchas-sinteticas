using System;

namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.PAGOS (A7 / A11). Una reserva admite a lo sumo un pago (RN04).
    /// </summary>
    public class Pago
    {
        public int IdPago { get; set; }

        public int IdReserva { get; set; }

        public decimal? MontoPago { get; set; }

        public DateTime? FechaPago { get; set; }

        public string EstadoPago { get; set; }

        /// <summary>Nombre del cliente (JOIN de sp_ConsultarEstadoPago). No es columna de PAGOS.</summary>
        public string NombreCliente { get; set; }

        public string NombreCancha { get; set; }

        /// <summary>Fecha de la franja reservada. Viene del JOIN con HORARIOS.</summary>
        public DateTime FechaHorario { get; set; }

        /// <summary>Hora de inicio de la franja. Viene del JOIN con HORARIOS.</summary>
        public TimeSpan HoraInicioHorario { get; set; }

        public string EstadoReserva { get; set; }
    }
}
