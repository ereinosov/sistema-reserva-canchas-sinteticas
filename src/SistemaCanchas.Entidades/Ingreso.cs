using System;

namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.fn_IngresosDetalle (RF12 / RN09). No es una tabla persistida.
    /// </summary>
    public class Ingreso
    {
        public int IdPago { get; set; }

        public int IdReserva { get; set; }

        public string NombreCliente { get; set; }

        public string NombreCancha { get; set; }

        public DateTime FechaHorario { get; set; }

        public TimeSpan HoraInicioHorario { get; set; }

        public decimal MontoPago { get; set; }

        public DateTime? FechaPago { get; set; }
    }
}
