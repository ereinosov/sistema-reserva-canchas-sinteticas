using System;
using System.Data.SqlClient;
using System.Security;

using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Contrato del gestor de conexión de dos fases (A3 / A12).
    /// </summary>
    public interface IGestorConexion
    {
        /// <summary>Indica si ya se estableció la sesión con el login individual de SQL Server.</summary>
        bool SesionActiva { get; }

        /// <summary>
        /// Devuelve una conexión cerrada con login_bootstrap (solo sp_ObtenerCredencialesLogin).
        /// </summary>
        /// <returns>Conexión lista para abrir dentro de un using.</returns>
        SqlConnection ObtenerConexionBootstrap();

        /// <summary>
        /// Devuelve una conexión cerrada con el login individual de la sesión (RF16).
        /// </summary>
        /// <returns>Conexión lista para abrir dentro de un using.</returns>
        /// <exception cref="SesionSqlNoIniciadaException">Si aún no hay sesión.</exception>
        SqlConnection ObtenerConexionActiva();

        /// <summary>
        /// Conexión con autenticación de Windows sobre la misma instancia (A1 §2.5, instalación).
        /// Sirve para crear el primer administrador cuando aún no hay sesión de aplicación.
        /// </summary>
        /// <returns>Conexión lista para abrir dentro de un using.</returns>
        SqlConnection ObtenerConexionInstalacion();

        /// <summary>
        /// Guarda las credenciales de motor y verifica que el login individual pueda conectar.
        /// </summary>
        /// <param name="usuarioBd">Login de SQL Server (usuario_bd).</param>
        /// <param name="claveBdPlana">Clave de motor ya descifrada. No se persiste en claro.</param>
        void EstablecerSesion(string usuarioBd, string claveBdPlana);

        /// <summary>Elimina de memoria las credenciales de la sesión activa.</summary>
        void CerrarSesion();

        /// <summary>Cifra la clave de motor con AES-256 CBC (A12 §10.2).</summary>
        /// <param name="clavePlana">Clave de SQL Server en claro.</param>
        /// <returns>IV + cifrado en Base64, para persistir en clave_bd_enc.</returns>
        string CifrarClaveBd(string clavePlana);

        /// <summary>Descifra clave_bd_enc con AES-256 CBC (A12 §10.2).</summary>
        /// <param name="claveCifrada">Valor persistido en USUARIOS.clave_bd_enc.</param>
        /// <returns>Clave de motor en claro, solo para reconectar.</returns>
        string DescifrarClaveBd(string claveCifrada);
    }
}
