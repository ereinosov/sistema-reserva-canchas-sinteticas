using System;

namespace SistemaCanchas.Datos.Excepciones
{
    /// <summary>
    /// Fallo al cifrar o descifrar clave_bd_enc (AES-256 CBC, A12 §10.2).
    /// </summary>
    public sealed class ErrorCifradoException : Exception
    {
        /// <summary>Inicializa la excepción con contexto y la causa original.</summary>
        /// <param name="mensaje">Mensaje seguro para capas superiores.</param>
        /// <param name="interna">Excepción criptográfica original.</param>
        public ErrorCifradoException(string mensaje, Exception interna)
            : base(mensaje, interna)
        {
        }

        /// <summary>Inicializa la excepción con un mensaje seguro.</summary>
        /// <param name="mensaje">Mensaje seguro para capas superiores.</param>
        public ErrorCifradoException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
