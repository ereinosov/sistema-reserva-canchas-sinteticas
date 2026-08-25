using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de usuarios (RF14, RF15, RF16).
    /// </summary>
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de conexión de dos fases.
        /// </summary>
        /// <param name="gestorConexion">Gestor de conexiones bootstrap, sesión e instalación.</param>
        public UsuarioRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public Usuario ObtenerCredenciales(string usuarioLogin)
        {
            if (string.IsNullOrWhiteSpace(usuarioLogin))
            {
                throw new ArgumentException("El usuario de acceso es obligatorio.", nameof(usuarioLogin));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionBootstrap())
                using (SqlCommand comando = new SqlCommand("sp_ObtenerCredencialesLogin", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.VarChar("@usuario_login", 30, usuarioLogin.Trim()));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (!lector.Read())
                        {
                            return null;
                        }

                        return MapearCredenciales(lector, usuarioLogin.Trim());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException(
                    "No se pudo consultar las credenciales de inicio de sesión.",
                    ex);
            }
        }

        public IList<Usuario> ObtenerTodos()
        {
            return ConsultarTodos(() => _gestorConexion.ObtenerConexionActiva());
        }

        public IList<Usuario> ObtenerTodosDesdeInstalacion()
        {
            return ConsultarTodos(() => _gestorConexion.ObtenerConexionInstalacion());
        }

        public int Insertar(Usuario usuario, string claveBdPlana, string nombreRol)
        {
            return InsertarInterno(usuario, claveBdPlana, nombreRol, () => _gestorConexion.ObtenerConexionActiva());
        }

        public int InsertarDesdeInstalacion(Usuario usuario, string claveBdPlana, string nombreRol)
        {
            return InsertarInterno(usuario, claveBdPlana, nombreRol, () => _gestorConexion.ObtenerConexionInstalacion());
        }

        public bool Desactivar(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "El identificador de usuario debe ser positivo.");
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_DesactivarUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_usuario", idUsuario));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo desactivar el usuario.", ex);
            }
        }

        public bool Activar(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "El identificador de usuario debe ser positivo.");
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ActivarUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_usuario", idUsuario));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo activar el usuario.", ex);
            }
        }

        public bool CambiarClave(int idUsuario, string claveAppHash)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "El identificador de usuario debe ser positivo.");
            }

            if (string.IsNullOrEmpty(claveAppHash))
            {
                throw new ArgumentException("El hash de la clave es obligatorio.", nameof(claveAppHash));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_CambiarClaveUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_usuario", idUsuario));
                    comando.Parameters.Add(ParametroSql.VarChar("@clave_app_hash", 255, claveAppHash));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo cambiar la clave del usuario.", ex);
            }
        }

        public bool ActualizarNombre(int idUsuario, string nombreUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idUsuario), "El identificador de usuario debe ser positivo.");
            }

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombreUsuario));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ActualizarNombreUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_usuario", idUsuario));
                    comando.Parameters.Add(ParametroSql.NVarChar("@nombre_usuario", 100, nombreUsuario));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo actualizar el nombre del usuario.", ex);
            }
        }

        private int InsertarInterno(
            Usuario usuario,
            string claveBdPlana,
            string nombreRol,
            Func<SqlConnection> fabricarConexion)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            if (string.IsNullOrEmpty(claveBdPlana))
            {
                throw new ArgumentException("La clave de motor es obligatoria.", nameof(claveBdPlana));
            }

            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                throw new ArgumentException("El rol es obligatorio.", nameof(nombreRol));
            }

            try
            {
                using (SqlConnection conexion = fabricarConexion())
                using (SqlCommand comando = new SqlCommand("sp_RegistrarUsuario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 60;
                    comando.Parameters.Add(ParametroSql.NVarChar("@nombre_usuario", 100, usuario.NombreUsuario));
                    comando.Parameters.Add(ParametroSql.VarChar("@usuario_login", 30, usuario.UsuarioLogin));
                    comando.Parameters.Add(ParametroSql.VarChar("@clave_app_hash", 255, usuario.ClaveAppHash));
                    comando.Parameters.Add(ParametroSql.VarChar("@usuario_bd", 30, usuario.UsuarioBd));
                    comando.Parameters.Add(ParametroSql.VarChar("@clave_bd_plana", 128, claveBdPlana));
                    comando.Parameters.Add(ParametroSql.VarChar("@clave_bd_enc", 255, usuario.ClaveBdEnc));
                    comando.Parameters.Add(ParametroSql.Char("@nombre_rol", 15, nombreRol));
                    SqlParameter idSalida = ParametroSql.EnteroSalida("@id_usuario_nuevo");
                    comando.Parameters.Add(idSalida);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(idSalida.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo registrar el usuario.", ex);
            }
        }

        private IList<Usuario> ConsultarTodos(Func<SqlConnection> fabricarConexion)
        {
            List<Usuario> resultado = new List<Usuario>();

            try
            {
                using (SqlConnection conexion = fabricarConexion())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarUsuarios", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            resultado.Add(MapearConsulta(lector));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar el listado de usuarios.", ex);
            }

            return resultado;
        }

        private static Usuario MapearCredenciales(SqlDataReader lector, string usuarioLogin)
        {
            return new Usuario
            {
                IdUsuario = LectorSql.Entero(lector, "id_usuario"),
                NombreUsuario = LectorSql.Cadena(lector, "nombre_usuario"),
                UsuarioLogin = usuarioLogin,
                ClaveAppHash = LectorSql.Cadena(lector, "clave_app_hash"),
                UsuarioBd = LectorSql.Cadena(lector, "usuario_bd"),
                ClaveBdEnc = LectorSql.Cadena(lector, "clave_bd_enc"),
                NombreRol = LectorSql.CadenaFija(lector, "nombre_rol"),
                EstadoUsuario = LectorSql.CadenaFija(lector, "estado_usuario")
            };
        }

        private static Usuario MapearConsulta(SqlDataReader lector)
        {
            return new Usuario
            {
                IdUsuario = LectorSql.Entero(lector, "id_usuario"),
                NombreUsuario = LectorSql.Cadena(lector, "nombre_usuario"),
                UsuarioLogin = LectorSql.Cadena(lector, "usuario_login"),
                NombreRol = LectorSql.CadenaFija(lector, "nombre_rol"),
                EstadoUsuario = LectorSql.CadenaFija(lector, "estado_usuario")
            };
        }
    }
}
