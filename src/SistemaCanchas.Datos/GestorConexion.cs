using System;
using System.Data.SqlClient;
using System.Security;

using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Login de dos fases: bootstrap → sp_ObtenerCredencialesLogin → AES → reconexión (A1 §2.4, A3).
    /// Singleton: la sesión debe ser la misma para todos los repositorios de la aplicación.
    /// </summary>
    public sealed class GestorConexion : IGestorConexion
    {
        private static readonly GestorConexion InstanciaUnica = new GestorConexion();

        private readonly object _candado = new object();
        private readonly CifradorAes _cifrador;

        private string _usuarioBd;
        private SecureString _claveBd;

        private GestorConexion()
        {
            _cifrador = new CifradorAes(AlmacenClaveAes.ObtenerOCrear());
        }

        /// <summary>Instancia única del gestor de conexión.</summary>
        public static GestorConexion Instancia
        {
            get { return InstanciaUnica; }
        }

        public bool SesionActiva
        {
            get
            {
                lock (_candado)
                {
                    return !string.IsNullOrEmpty(_usuarioBd) && _claveBd != null;
                }
            }
        }

        public SqlConnection ObtenerConexionBootstrap()
        {
            return new SqlConnection(ConfiguracionApp.ObtenerCadenaBootstrap());
        }

        public SqlConnection ObtenerConexionActiva()
        {
            lock (_candado)
            {
                if (string.IsNullOrEmpty(_usuarioBd) || _claveBd == null)
                {
                    throw new SesionSqlNoIniciadaException();
                }

                return CrearConexionDeSesion(_usuarioBd, _claveBd);
            }
        }

        public SqlConnection ObtenerConexionInstalacion()
        {
            SqlConnectionStringBuilder constructor = new SqlConnectionStringBuilder(ConfiguracionApp.ObtenerCadenaBootstrap());
            constructor.Remove("User ID");
            constructor.Remove("Password");
            constructor.Remove("UID");
            constructor.Remove("PWD");
            constructor.IntegratedSecurity = true;
            return new SqlConnection(constructor.ConnectionString);
        }

        public void EstablecerSesion(string usuarioBd, string claveBdPlana)
        {
            if (string.IsNullOrWhiteSpace(usuarioBd))
            {
                throw new ArgumentException("El login de SQL Server es obligatorio.", nameof(usuarioBd));
            }

            if (string.IsNullOrEmpty(claveBdPlana))
            {
                throw new ArgumentException("La clave de SQL Server es obligatoria.", nameof(claveBdPlana));
            }

            SecureString claveSegura = CrearSecureString(claveBdPlana);
            using (SqlConnection prueba = CrearConexionDeSesion(usuarioBd.Trim(), claveSegura))
            {
                try
                {
                    prueba.Open();
                }
                catch (SqlException ex)
                {
                    claveSegura.Dispose();
                    throw new ErrorAccesoDatosException(
                        "No se pudo abrir la conexión con el login individual de SQL Server.",
                        ex);
                }
            }

            lock (_candado)
            {
                if (_claveBd != null)
                {
                    _claveBd.Dispose();
                }

                _usuarioBd = usuarioBd.Trim();
                _claveBd = claveSegura;
            }
        }

        public void CerrarSesion()
        {
            lock (_candado)
            {
                _usuarioBd = null;
                if (_claveBd != null)
                {
                    _claveBd.Dispose();
                    _claveBd = null;
                }
            }
        }

        public string CifrarClaveBd(string clavePlana)
        {
            if (string.IsNullOrEmpty(clavePlana))
            {
                throw new ArgumentException("La clave de motor no puede estar vacía.", nameof(clavePlana));
            }

            return _cifrador.Cifrar(clavePlana);
        }

        public string DescifrarClaveBd(string claveCifrada)
        {
            return _cifrador.Descifrar(claveCifrada);
        }

        private static SqlConnection CrearConexionDeSesion(string usuarioBd, SecureString claveBd)
        {
            SqlConnectionStringBuilder constructor = new SqlConnectionStringBuilder(ConfiguracionApp.ObtenerCadenaBootstrap());
            constructor.Remove("User ID");
            constructor.Remove("Password");
            constructor.Remove("UID");
            constructor.Remove("PWD");
            constructor.IntegratedSecurity = false;

            SqlConnection conexion = new SqlConnection(constructor.ConnectionString);
            conexion.Credential = new SqlCredential(usuarioBd, claveBd);
            return conexion;
        }

        private static SecureString CrearSecureString(string texto)
        {
            SecureString seguro = new SecureString();
            foreach (char caracter in texto)
            {
                seguro.AppendChar(caracter);
            }

            seguro.MakeReadOnly();
            return seguro;
        }
    }
} 
