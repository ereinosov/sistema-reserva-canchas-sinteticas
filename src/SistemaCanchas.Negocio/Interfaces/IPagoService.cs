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

        /// <summary>Consulta el estado de pago de reservas activas (RF10). Requiere sesión iniciada.</summary>
        /// <param name="idReserva">Reserva concreta; null o 0 para todas las activas.</param>
        /// <returns>Reservas activas con su pago, si existe.</returns>
        IList<Pago> ConsultarEstadoPago(int? idReserva);
    }
}
