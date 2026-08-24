using System;

namespace SistemaCanchas.Datos.Excepciones
{
    /// <summary>
    /// Error de lectura de App.config: cadena de conexión ausente o mal formada.
    /// </summary>
    public sealed class ConfiguracionInvalidaException : Exception
    {
        /// <summary>Inicializa la excepción con un mensaje descriptivo.</summary>
        /// <param name="mensaje">Texto que se mostrará al operador.</param>
        public ConfiguracionInvalidaException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
