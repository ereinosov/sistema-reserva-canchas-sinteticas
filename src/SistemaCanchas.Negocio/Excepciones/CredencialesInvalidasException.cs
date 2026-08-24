using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// Usuario o clave de aplicación incorrectos (RF16). El mensaje no revela si el login existe.
    /// </summary>
    public sealed class CredencialesInvalidasException : Exception
    {
        /// <summary>Inicializa la excepción con el mensaje estándar de login.</summary>
        public CredencialesInvalidasException()
            : base("Usuario o clave incorrectos.")
        {
        }
    }
}
