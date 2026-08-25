using System;
using System.Collections.Generic;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Autenticación de dos fases y administración de cuentas (RF14, RF15, RF16, RN11, RN12).
    /// </summary>
    public sealed class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IGestorConexion _gestorConexion;

        private Usuario _sesionActual;

        /// <summary>
        /// Composición por defecto: repositorio real y gestor singleton. La presentación no instancia Datos.
        /// </summary>
        public UsuarioService()
            : this(new UsuarioRepository(GestorConexion.Instancia), GestorConexion.Instancia)
        {
        }

        /// <summary>
        /// Constructor para pruebas unitarias con dobles.
        /// </summary>
        /// <param name="usuarioRepository">Origen de credenciales.</param>
        /// <param name="gestorConexion">Gestor de conexión de dos fases.</param>
        internal UsuarioService(IUsuarioRepository usuarioRepository, IGestorConexion gestorConexion)
        {
            if (usuarioRepository == null)
            {
                throw new ArgumentNullException(nameof(usuarioRepository));
            }

            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _usuarioRepository = usuarioRepository;
            _gestorConexion = gestorConexion;
        }

        public Usuario ValidarCredenciales(string usuarioLogin, string claveApp)
        {
            if (string.IsNullOrWhiteSpace(usuarioLogin) || string.IsNullOrEmpty(claveApp))
            {
                throw new CredencialesInvalidasException();
            }

            string login = usuarioLogin.Trim();
            Usuario usuario = ObtenerCredencialesSeguro(login);
            if (usuario == null)
            {
                throw new CredencialesInvalidasException();
            }

            // RN12: un usuario inactivo no puede iniciar sesión.
            if (!string.Equals(usuario.EstadoUsuario, ValoresDominio.EstadoUsuario.Activo, StringComparison.Ordinal))
            {
                throw new UsuarioInactivoException();
            }

            if (!VerificarClaveAplicacion(claveApp, usuario.ClaveAppHash))
            {
                throw new CredencialesInvalidasException();
            }

            string claveBdPlana = DescifrarClaveMotor(usuario.ClaveBdEnc);
            EstablecerSesionMotor(usuario.UsuarioBd, claveBdPlana);

            _sesionActual = SanitizarParaSesion(usuario);
            return _sesionActual;
        }

        public Usuario ObtenerSesionActual()
        {
            if (_sesionActual == null)
            {
                throw new SesionNoIniciadaException();
            }

            return _sesionActual;
        }

        public void CerrarSesion()
        {
            _sesionActual = null;
            _gestorConexion.CerrarSesion();
        }

        public int RegistrarUsuario(string nombreUsuario, string usuarioLogin, string claveApp, string nombreRol)
        {
            ExigirAdministrador();
            return RegistrarInterno(
                nombreUsuario,
                usuarioLogin,
                claveApp,
                nombreRol,
                (usuario, claveBdPlana, rol) => _usuarioRepository.Insertar(usuario, claveBdPlana, rol));
        }

        public int RegistrarAdministradorInicial(string nombreUsuario, string usuarioLogin, string claveApp)
        {
            IList<Usuario> existentes = EjecutarDatos(
                () => _usuarioRepository.ObtenerTodosDesdeInstalacion(),
                "No se pudo comprobar si ya existe un administrador. Ejecute el script A11 en SQL Server " +
                "con su cuenta de Windows (sysadmin) e inténtelo de nuevo.");

            if (existentes.Count > 0)
            {
                throw new ValidacionNegocioException(
                    "Ya existe al menos un usuario. Use el inicio de sesión; no se puede repetir la instalación inicial.");
            }

            return RegistrarInterno(
                nombreUsuario,
                usuarioLogin,
                claveApp,
                ValoresDominio.Rol.Administrador,
                (usuario, claveBdPlana, rol) => _usuarioRepository.InsertarDesdeInstalacion(usuario, claveBdPlana, rol));
        }

        public void DesactivarUsuario(int idUsuario)
        {
            Usuario sesion = ExigirAdministrador();
            if (idUsuario == sesion.IdUsuario)
            {
                throw new OperacionNoPermitidaException("No puede desactivar la cuenta con la que está autenticado.");
            }

            IList<Usuario> usuarios = ObtenerTodos();
            Usuario objetivo = BuscarPorId(usuarios, idUsuario);
            if (objetivo == null)
            {
                throw new ValidacionNegocioException("El usuario indicado no existe.");
            }

            if (EsAdministradorActivo(objetivo) && ContarAdministradoresActivos(usuarios) <= 1)
            {
                throw new OperacionNoPermitidaException("No se puede desactivar al único administrador activo.");
            }

            EjecutarDatos(
                () => _usuarioRepository.Desactivar(idUsuario),
                "No se pudo desactivar el usuario.");
        }

        public void ActivarUsuario(int idUsuario)
        {
            ExigirAdministrador();
            IList<Usuario> usuarios = ObtenerTodos();
            Usuario objetivo = BuscarPorId(usuarios, idUsuario);
            if (objetivo == null)
            {
                throw new ValidacionNegocioException("El usuario indicado no existe.");
            }

            if (string.Equals(objetivo.EstadoUsuario, ValoresDominio.EstadoUsuario.Activo, StringComparison.Ordinal))
            {
                throw new ValidacionNegocioException("El usuario ya se encuentra activo.");
            }

            EjecutarDatos(
                () => _usuarioRepository.Activar(idUsuario),
                "No se pudo activar el usuario.");
        }

        public void CambiarClaveUsuario(int idUsuario, string claveApp)
        {
            ExigirAdministrador();
            if (idUsuario <= 0)
            {
                throw new ValidacionNegocioException("Seleccione un usuario de la lista.");
            }

            ValidadorUsuario.ValidarClaveApp(claveApp);
            string hash = BCrypt.Net.BCrypt.HashPassword(claveApp, ValoresDominio.CostoHashAplicacion);
            EjecutarDatos(
                () => _usuarioRepository.CambiarClave(idUsuario, hash),
                "No se pudo cambiar la clave del usuario.");
        }

        public void ActualizarNombreUsuario(int idUsuario, string nombreUsuario)
        {
            ExigirAdministrador();
            if (idUsuario <= 0)
            {
                throw new ValidacionNegocioException("Seleccione un usuario de la lista.");
            }

            ValidadorUsuario.ValidarNombre(nombreUsuario);
            EjecutarDatos(
                () => _usuarioRepository.ActualizarNombre(idUsuario, nombreUsuario.Trim()),
                "No se pudo actualizar el nombre del usuario.");
        }

        public bool ExisteAlgunUsuario()
        {
            IList<Usuario> existentes = EjecutarDatos(
                () => _usuarioRepository.ObtenerTodosDesdeInstalacion(),
                "No se pudo comprobar si ya existe un usuario.");
            return existentes.Count > 0;
        }

        public IList<Usuario> ObtenerTodos()
        {
            ExigirAdministrador();
            return EjecutarDatos(
                () => _usuarioRepository.ObtenerTodos(),
                "No se pudo consultar el listado de usuarios.");
        }

        private int RegistrarInterno(
            string nombreUsuario,
            string usuarioLogin,
            string claveApp,
            string nombreRol,
            Func<Usuario, string, string, int> persistir)
        {
            ValidadorUsuario.ValidarNombre(nombreUsuario);
            ValidadorUsuario.ValidarLogin(usuarioLogin);
            ValidadorUsuario.ValidarClaveApp(claveApp);
            ValidadorUsuario.ValidarRol(nombreRol);

            string login = usuarioLogin.Trim();
            string claveBdPlana = GeneradorClaveMotor.Generar();
            string claveBdEnc;
            try
            {
                claveBdEnc = _gestorConexion.CifrarClaveBd(claveBdPlana);
            }
            catch (ErrorCifradoException ex)
            {
                throw new ErrorInfraestructuraException(
                    "No se pudo cifrar la clave de motor del nuevo usuario.",
                    ex);
            }

            Usuario fila = new Usuario
            {
                NombreUsuario = nombreUsuario.Trim(),
                UsuarioLogin = login,
                ClaveAppHash = BCrypt.Net.BCrypt.HashPassword(claveApp, ValoresDominio.CostoHashAplicacion),
                UsuarioBd = IdentificadorSql.DesdeLogin(login),
                ClaveBdEnc = claveBdEnc,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo,
                NombreRol = nombreRol
            };

            return EjecutarDatos(
                () => persistir(fila, claveBdPlana, nombreRol),
                "No se pudo registrar el usuario. Verifique que SQL Server esté en ejecución y que el script A11 se haya aplicado.");
        }

        private Usuario ExigirAdministrador()
        {
            Usuario sesion = ObtenerSesionActual();
            if (!EsAdministradorActivo(sesion))
            {
                throw new OperacionNoPermitidaException(
                    "Solo el perfil administrador puede gestionar cuentas de usuario.");
            }

            return sesion;
        }

        private Usuario ObtenerCredencialesSeguro(string usuarioLogin)
        {
            return EjecutarDatos(
                () => _usuarioRepository.ObtenerCredenciales(usuarioLogin),
                "No se pudo conectar con el servidor de base de datos. Verifique la conexión e inténtelo de nuevo.");
        }

        private static bool VerificarClaveAplicacion(string claveApp, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(claveApp, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        private string DescifrarClaveMotor(string claveCifrada)
        {
            try
            {
                return _gestorConexion.DescifrarClaveBd(claveCifrada);
            }
            catch (ErrorCifradoException ex)
            {
                throw new ErrorInfraestructuraException(
                    "No se pudo preparar la conexión de la sesión. Verifique la clave de cifrado de esta estación.",
                    ex);
            }
        }

        private void EstablecerSesionMotor(string usuarioBd, string claveBdPlana)
        {
            try
            {
                _gestorConexion.EstablecerSesion(usuarioBd, claveBdPlana);
            }
            catch (ErrorAccesoDatosException ex)
            {
                throw new ErrorInfraestructuraException(
                    "Las credenciales de aplicación son válidas, pero no se pudo conectar con el login de base de datos. " +
                    "Verifique que SQL Server esté en ejecución y que el login individual exista.",
                    ex);
            }
            catch (ConfiguracionInvalidaException ex)
            {
                throw new ErrorInfraestructuraException(ex.Message, ex);
            }
        }

        private T EjecutarDatos<T>(Func<T> operacion, string mensajeInfraestructura)
        {
            try
            {
                return operacion();
            }
            catch (ErrorAccesoDatosException ex)
            {
                if (ex.NumeroSql == CodigosSql.UsuarioDuplicado)
                {
                    throw new ValidacionNegocioException("Ya existe un usuario registrado con ese usuario de acceso.");
                }

                if (ex.NumeroSql == CodigosSql.RolInexistente)
                {
                    throw new ValidacionNegocioException("El rol indicado no existe.");
                }

                if (ex.NumeroSql == CodigosSql.UsuarioNoExiste)
                {
                    throw new ValidacionNegocioException("El usuario indicado no existe.");
                }

                if (ex.NumeroSql == CodigosSql.UsuarioYaActivo)
                {
                    throw new ValidacionNegocioException("El usuario ya se encuentra activo.");
                }

                throw new ErrorInfraestructuraException(mensajeInfraestructura + DetalleMotor(ex), ex);
            }
            catch (ConfiguracionInvalidaException ex)
            {
                throw new ErrorInfraestructuraException(ex.Message, ex);
            }
            catch (SesionSqlNoIniciadaException)
            {
                throw new SesionNoIniciadaException();
            }
        }

        private static string DetalleMotor(Exception excepcion)
        {
            Exception actual = excepcion;
            while (actual.InnerException != null)
            {
                actual = actual.InnerException;
            }

            if (actual != excepcion && !string.IsNullOrWhiteSpace(actual.Message))
            {
                return Environment.NewLine + actual.Message;
            }

            return string.Empty;
        }

        private static bool EsAdministradorActivo(Usuario usuario)
        {
            return usuario != null &&
                   string.Equals(usuario.NombreRol, ValoresDominio.Rol.Administrador, StringComparison.Ordinal) &&
                   string.Equals(usuario.EstadoUsuario, ValoresDominio.EstadoUsuario.Activo, StringComparison.Ordinal);
        }

        private static int ContarAdministradoresActivos(IList<Usuario> usuarios)
        {
            int total = 0;
            for (int i = 0; i < usuarios.Count; i++)
            {
                if (EsAdministradorActivo(usuarios[i]))
                {
                    total++;
                }
            }

            return total;
        }

        private static Usuario BuscarPorId(IList<Usuario> usuarios, int idUsuario)
        {
            for (int i = 0; i < usuarios.Count; i++)
            {
                if (usuarios[i].IdUsuario == idUsuario)
                {
                    return usuarios[i];
                }
            }

            return null;
        }

        private static Usuario SanitizarParaSesion(Usuario usuario)
        {
            return new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                UsuarioLogin = usuario.UsuarioLogin,
                IdRol = usuario.IdRol,
                NombreRol = usuario.NombreRol,
                EstadoUsuario = usuario.EstadoUsuario,
                ClaveAppHash = null,
                UsuarioBd = null,
                ClaveBdEnc = null
            };
        }
    }
}
