using System;

namespace SistemaCanchas.Negocio.Excepciones
{
    /// <summary>
    /// El usuario existe pero está inactivo (RN12).
    /// </summary>
    public sealed class UsuarioInactivoException : Exception
    {
        /// <summary>Inicializa la excepción con el mensaje de RN12.</summary>
        public UsuarioInactivoException()
            : base("El usuario se encuentra inactivo y no puede iniciar sesión.")
        {
        }
    }
}
