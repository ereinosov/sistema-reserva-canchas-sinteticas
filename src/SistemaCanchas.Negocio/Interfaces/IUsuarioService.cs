using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Autenticación, sesión y administración de cuentas (RF14, RF15, RF16).
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Valida la credencial de aplicación (bcrypt), descifra el login de motor y abre la sesión real.
        /// </summary>
        /// <param name="usuarioLogin">Usuario de aplicación.</param>
        /// <param name="claveApp">Clave de aplicación en claro, solo en memoria.</param>
        /// <returns>Usuario de sesión, sin hashes ni claves de motor.</returns>
        Usuario ValidarCredenciales(string usuarioLogin, string claveApp);

        /// <summary>Devuelve el usuario autenticado en esta instancia de servicio.</summary>
        /// <returns>Usuario de sesión.</returns>
        Usuario ObtenerSesionActual();

        /// <summary>Cierra la sesión de aplicación y la conexión individual de SQL Server.</summary>
        void CerrarSesion();

        /// <summary>
        /// Registra una cuenta (RF14 / RN11). Requiere sesión de administrador.
        /// Crea también el login de SQL Server con el rol indicado.
        /// </summary>
        /// <param name="nombreUsuario">Nombre visible.</param>
        /// <param name="usuarioLogin">Usuario de acceso de aplicación.</param>
        /// <param name="claveApp">Clave de aplicación en claro.</param>
        /// <param name="nombreRol">administrador o empleado.</param>
        /// <returns>Id del usuario creado.</returns>
        int RegistrarUsuario(string nombreUsuario, string usuarioLogin, string claveApp, string nombreRol);

        /// <summary>
        /// Crea el primer administrador durante la instalación (A1 §2.5).
        /// Usa autenticación de Windows; no requiere sesión de aplicación.
        /// </summary>
        /// <param name="nombreUsuario">Nombre visible.</param>
        /// <param name="usuarioLogin">Usuario de acceso de aplicación.</param>
        /// <param name="claveApp">Clave de aplicación en claro.</param>
        /// <returns>Id del administrador creado.</returns>
        int RegistrarAdministradorInicial(string nombreUsuario, string usuarioLogin, string claveApp);

        /// <summary>
        /// Desactiva una cuenta y su login de SQL Server (RF15 / RN12).
        /// </summary>
        /// <param name="idUsuario">Identificador de USUARIOS.</param>
        void DesactivarUsuario(int idUsuario);

        /// <summary>
        /// Lista las cuentas sin secretos. Requiere sesión de administrador.
        /// </summary>
        /// <returns>Usuarios registrados.</returns>
        IList<Usuario> ObtenerTodos();
    }
}
