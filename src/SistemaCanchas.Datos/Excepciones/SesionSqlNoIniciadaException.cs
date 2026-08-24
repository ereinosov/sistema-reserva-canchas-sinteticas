using System;

namespace SistemaCanchas.Datos.Excepciones
{
    /// <summary>
    /// Se pidió la conexión de sesión SQL antes de completar el inicio de sesión de dos fases.
    /// </summary>
    public sealed class SesionSqlNoIniciadaException : Exception
    {
        /// <summary>Inicializa la excepción con el mensaje estándar.</summary>
        public SesionSqlNoIniciadaException()
            : base("No hay una sesión de base de datos activa. El usuario debe iniciar sesión.")
        {
        }
    }
}
