using System;
using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Gestión de pagos (RF09-RF10 / RN04).
    /// </summary>
    public interface IPagoService
    {
        /// <summary>Registra el único pago de una reserva activa. Requiere sesión iniciada.</summary>
        /// <param name="idReserva">Reserva activa sin pago previo.</param>
        /// <param name="monto">Monto mayor a cero.</param>
        /// <param name="fechaPago">Fecha del pago.</param>
        /// <param name="estadoPago">pendiente o pagado.</param>
        /// <returns>Id generado.</returns>
        int RegistrarPago(int idReserva, decimal monto, DateTime fechaPago, string estadoPago);

        IList<Pago> ConsultarEstadoPago(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva);
    }
}
