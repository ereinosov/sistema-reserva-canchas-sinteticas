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
    /// Reglas de canchas (RF13 / RN10). La presentación no instancia el repositorio.
    /// </summary>
    public sealed class CanchaService : ICanchaService
    {
        private readonly ICanchaRepository _canchaRepository;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Composición de la aplicación: repositorio real y la misma sesión de usuario.
        /// </summary>
        /// <param name="usuarioService">Sesión autenticada (no crear una instancia nueva).</param>
        public CanchaService(IUsuarioService usuarioService)
            : this(new CanchaRepository(GestorConexion.Instancia), usuarioService)
        {
        }

        /// <summary>Constructor para pruebas unitarias con dobles.</summary>
        /// <param name="canchaRepository">Origen de persistencia.</param>
        /// <param name="usuarioService">Sesión autenticada.</param>
        internal CanchaService(ICanchaRepository canchaRepository, IUsuarioService usuarioService)
        {
            if (canchaRepository == null)
            {
                throw new ArgumentNullException(nameof(canchaRepository));
            }

            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _canchaRepository = canchaRepository;
            _usuarioService = usuarioService;
        }

        /// <inheritdoc />
        public int RegistrarCancha(string nombreCancha)
        {
            ExigirAdministrador();
            string nombre = NormalizarNombre(nombreCancha);
            Cancha cancha = new Cancha
            {
                NombreCancha = nombre,
                EstadoCancha = ValoresDominio.EstadoCancha.Activa
            };

            return EjecutarDatos(
                () => _canchaRepository.Insertar(cancha),
                "No se pudo registrar la cancha.");
        }

        /// <inheritdoc />
        public IList<Cancha> ObtenerTodas()
        {
            _usuarioService.ObtenerSesionActual();
            return EjecutarDatos(
                () => _canchaRepository.ObtenerTodos(null),
                "No se pudo consultar las canchas.");
        }

        /// <inheritdoc />
        public IList<Cancha> ObtenerActivas()
        {
            _usuarioService.ObtenerSesionActual();
            return EjecutarDatos(
                () => _canchaRepository.ObtenerTodos(ValoresDominio.EstadoCancha.Activa),
                "No se pudo consultar las canchas activas.");
        }

        /// <inheritdoc />
        public void ModificarCancha(int idCancha, string nombreCancha)
        {
            ExigirAdministrador();
            if (idCancha <= 0)
            {
                throw new ValidacionNegocioException("Seleccione una cancha de la lista.");
            }

            string nombre = NormalizarNombre(nombreCancha);
            Cancha cancha = new Cancha
            {
                IdCancha = idCancha,
                NombreCancha = nombre
            };

            EjecutarDatos(
                () => _canchaRepository.Actualizar(cancha),
                "No se pudo modificar la cancha.");
        }

        /// <inheritdoc />
        public void DesactivarCancha(int idCancha)
        {
            ExigirAdministrador();
            if (idCancha <= 0)
            {
                throw new ValidacionNegocioException("Seleccione una cancha de la lista.");
            }

            Cancha actual = Buscar(idCancha);
            if (actual == null)
            {
                throw new ValidacionNegocioException("La cancha indicada no existe.");
            }

            if (string.Equals(actual.EstadoCancha, ValoresDominio.EstadoCancha.Inactiva, StringComparison.Ordinal))
            {
                throw new ValidacionNegocioException("La cancha ya se encuentra inactiva.");
            }

            // RN10: la desactivación no cancela reservas previas; solo impide nuevas.
            EjecutarDatos(
                () => _canchaRepository.Desactivar(idCancha),
                "No se pudo desactivar la cancha.");
        }

        /// <inheritdoc />
        public bool CanchaActiva(int idCancha)
        {
            _usuarioService.ObtenerSesionActual();
            Cancha cancha = Buscar(idCancha);
            return cancha != null &&
                   string.Equals(cancha.EstadoCancha, ValoresDominio.EstadoCancha.Activa, StringComparison.Ordinal);
        }

        private Cancha Buscar(int idCancha)
        {
            IList<Cancha> canchas = EjecutarDatos(
                () => _canchaRepository.ObtenerTodos(null),
                "No se pudo consultar las canchas.");

            for (int i = 0; i < canchas.Count; i++)
            {
                if (canchas[i].IdCancha == idCancha)
                {
                    return canchas[i];
                }
            }

            return null;
        }

        private void ExigirAdministrador()
        {
            Usuario sesion = _usuarioService.ObtenerSesionActual();
            if (!string.Equals(sesion.NombreRol, ValoresDominio.Rol.Administrador, StringComparison.Ordinal))
            {
                throw new OperacionNoPermitidaException("Solo el perfil administrador puede gestionar canchas.");
            }
        }

        private static string NormalizarNombre(string nombreCancha)
        {
            if (string.IsNullOrWhiteSpace(nombreCancha))
            {
                throw new ValidacionNegocioException("El nombre de la cancha es obligatorio.");
            }

            string nombre = nombreCancha.Trim();
            if (nombre.Length > ValoresDominio.LongitudMaximaNombreCancha)
            {
                throw new ValidacionNegocioException("El nombre de la cancha no puede superar 60 caracteres.");
            }

            return nombre;
        }

        private static T EjecutarDatos<T>(Func<T> operacion, string mensajeInfraestructura)
        {
            try
            {
                return operacion();
            }
            catch (ErrorAccesoDatosException ex)
            {
                if (ex.NumeroSql == CodigosSql.CanchaDuplicada)
                {
                    throw new ValidacionNegocioException("Ya existe una cancha registrada con ese nombre.");
                }

                if (ex.NumeroSql == CodigosSql.CanchaNoExiste)
                {
                    throw new ValidacionNegocioException("La cancha indicada no existe.");
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
    }
}
