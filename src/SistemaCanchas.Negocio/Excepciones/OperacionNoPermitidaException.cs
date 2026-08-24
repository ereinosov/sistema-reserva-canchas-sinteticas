using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// La operación no está autorizada para el rol o el estado actual de la sesión (A1 §2.3).
    /// </summary>
    public sealed class OperacionNoPermitidaException : Exception
    {
        /// <summary>Inicializa la excepción con el motivo.</summary>
        /// <param name="mensaje">Motivo que puede mostrarse al operador.</param>
        public OperacionNoPermitidaException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
