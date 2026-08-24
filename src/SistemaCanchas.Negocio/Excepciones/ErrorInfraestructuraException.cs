using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// Fallo de infraestructura (conexión, cifrado o configuración) presentado de forma segura (RNF06).
    /// </summary>
    public sealed class ErrorInfraestructuraException : Exception
    {
        /// <summary>Inicializa la excepción con un mensaje seguro y la causa original.</summary>
        /// <param name="mensaje">Texto que puede mostrarse al operador.</param>
        /// <param name="interna">Causa técnica; no se muestra en la interfaz.</param>
        public ErrorInfraestructuraException(string mensaje, Exception interna)
            : base(mensaje, interna)
        {
        }

        /// <summary>Inicializa la excepción con un mensaje seguro.</summary>
        /// <param name="mensaje">Texto que puede mostrarse al operador.</param>
        public ErrorInfraestructuraException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
