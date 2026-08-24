using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a PAGOS mediante los procedimientos del artefacto A11 (RF09-RF10 / RN04).
    /// </summary>
    public interface IPagoRepository
    {
        /// <summary>Registra el único pago de una reserva activa (sp_RegistrarPago, RF09).</summary>
        /// <param name="pago">Reserva, monto, fecha y estado.</param>
        /// <returns>Id generado.</returns>
        int Insertar(Pago pago);

        /// <summary>Consulta el estado de pago de reservas activas (sp_ConsultarEstadoPago, RF10).</summary>
        /// <param name="idReserva">Reserva concreta o null para todas las activas.</param>
        /// <returns>Reservas activas con su pago, si existe.</returns>
        IList<Pago> ObtenerTodos(int? idReserva);
    }
}
