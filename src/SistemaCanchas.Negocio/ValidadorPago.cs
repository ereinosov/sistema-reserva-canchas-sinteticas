using System;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Validaciones de pago (RN04). La unicidad la confirma el motor.
    /// </summary>
    internal static class ValidadorPago
    {
        internal static Pago Normalizar(int idReserva, decimal monto, DateTime fechaPago, string estadoPago)
        {
            if (idReserva <= 0)
            {
                throw new ValidacionNegocioException("Seleccione una reserva activa de la lista.");
            }

            if (monto <= 0m)
            {
                throw new ValidacionNegocioException("El monto del pago debe ser mayor a cero.");
            }

            if (monto > ValoresDominio.MontoPagoMaximo)
            {
                throw new ValidacionNegocioException("El monto del pago no puede superar 9.999.999,99.");
            }

            if (fechaPago == default(DateTime))
            {
                throw new ValidacionNegocioException("La fecha del pago es obligatoria.");
            }

            if (!EsEstadoPago(estadoPago))
            {
                throw new ValidacionNegocioException("El estado del pago debe ser pendiente o pagado.");
            }

            return new Pago
            {
                IdReserva = idReserva,
                MontoPago = decimal.Round(monto, 2),
                FechaPago = fechaPago.Date,
                EstadoPago = estadoPago
            };
        }

        private static bool EsEstadoPago(string estado)
        {
            return string.Equals(estado, ValoresDominio.EstadoPago.Pagado, StringComparison.Ordinal) ||
                   string.Equals(estado, ValoresDominio.EstadoPago.Pendiente, StringComparison.Ordinal);
        }
    }
}
