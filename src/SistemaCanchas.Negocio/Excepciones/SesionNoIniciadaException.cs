using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// No hay una sesión de aplicación vigente.
    /// </summary>
    public sealed class SesionNoIniciadaException : Exception
    {
        /// <summary>Inicializa la excepción con el mensaje estándar.</summary>
        public SesionNoIniciadaException()
            : base("No hay una sesión iniciada.")
        {
        }
    }
}
