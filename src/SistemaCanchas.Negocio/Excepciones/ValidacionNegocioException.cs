using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// Incumplimiento de una regla de validación de negocio (formato, unicidad, estados).
    /// </summary>
    public sealed class ValidacionNegocioException : Exception
    {
        /// <summary>Inicializa la excepción con el mensaje que puede mostrarse al operador.</summary>
        /// <param name="mensaje">Descripción de la validación incumplida.</param>
        public ValidacionNegocioException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
