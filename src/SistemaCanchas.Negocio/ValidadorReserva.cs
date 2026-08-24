using System;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Validaciones de reserva previas al motor (RN06).
    /// </summary>
    internal static class ValidadorReserva
    {
        internal static void ExigirId(int id, string mensaje)
        {
            if (id <= 0)
            {
                throw new ValidacionNegocioException(mensaje);
            }
        }

        internal static string NormalizarEstado(string estadoReserva)
        {
            if (string.IsNullOrWhiteSpace(estadoReserva))
            {
                return null;
            }

            string estado = estadoReserva.Trim();
            if (string.Equals(estado, ValoresDominio.EstadoReserva.Activa, StringComparison.Ordinal) ||
                string.Equals(estado, ValoresDominio.EstadoReserva.Cancelada, StringComparison.Ordinal))
            {
                return estado;
            }

            throw new ValidacionNegocioException("El estado de la reserva debe ser activa o cancelada.");
        }
    }
}
