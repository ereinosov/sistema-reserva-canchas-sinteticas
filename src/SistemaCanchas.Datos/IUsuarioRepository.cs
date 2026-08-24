using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a USUARIOS mediante los procedimientos del artefacto A11.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtiene las credenciales de aplicación y de motor para un login (sp_ObtenerCredencialesLogin, RF16).
        /// </summary>
        /// <param name="usuarioLogin">Nombre de acceso de aplicación.</param>
        /// <returns>El usuario si existe; null si no hay fila.</returns>
        Usuario ObtenerCredenciales(string usuarioLogin);

        /// <summary>
        /// Lista cuentas sin hashes ni claves (sp_ConsultarUsuarios, RF14/RF15).
        /// </summary>
        /// <returns>Usuarios registrados.</returns>
        IList<Usuario> ObtenerTodos();

        /// <summary>
        /// Igual que <see cref="ObtenerTodos"/>, usando la conexión de instalación (Windows).
        /// </summary>
        /// <returns>Usuarios registrados.</returns>
        IList<Usuario> ObtenerTodosDesdeInstalacion();

        /// <summary>
        /// Crea la cuenta y el login de SQL Server (sp_RegistrarUsuario, RF14).
        /// </summary>
        /// <param name="usuario">Fila a persistir (hash y clave de motor ya resueltos).</param>
        /// <param name="claveBdPlana">Clave de SQL Server, solo en tránsito.</param>
        /// <param name="nombreRol">administrador o empleado.</param>
        /// <returns>Id generado.</returns>
        int Insertar(Usuario usuario, string claveBdPlana, string nombreRol);

        /// <summary>
        /// Igual que <see cref="Insertar"/>, usando la conexión de instalación (primer administrador).
        /// </summary>
        /// <param name="usuario">Fila a persistir.</param>
        /// <param name="claveBdPlana">Clave de SQL Server, solo en tránsito.</param>
        /// <param name="nombreRol">Debe ser administrador en la instalación inicial.</param>
        /// <returns>Id generado.</returns>
        int InsertarDesdeInstalacion(Usuario usuario, string claveBdPlana, string nombreRol);

        /// <summary>
        /// Desactiva la cuenta y deshabilita el login (sp_DesactivarUsuario, RF15 / RN12).
        /// </summary>
        /// <param name="idUsuario">Identificador de USUARIOS.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Desactivar(int idUsuario);
    }
}
