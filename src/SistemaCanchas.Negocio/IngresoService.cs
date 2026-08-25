using System;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;
using SistemaCanchas.Negocio.Interfaces;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Reglas de consulta de ingresos (RF12 / RN09).
    /// </summary>
    public sealed class IngresoService : IIngresoService
    {
        private readonly IIngresoRepository _ingresoRepository;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Composición de la aplicación: repositorio real y la misma sesión de usuario.
        /// </summary>
        /// <param name="usuarioService">Sesión autenticada (no crear una instancia nueva).</param>
        public IngresoService(IUsuarioService usuarioService)
            : this(new IngresoRepository(GestorConexion.Instancia), usuarioService)
        {
        }

        /// <summary>Constructor para pruebas unitarias con dobles.</summary>
        /// <param name="ingresoRepository">Origen de persistencia.</param>
        /// <param name="usuarioService">Sesión autenticada.</param>
        internal IngresoService(IIngresoRepository ingresoRepository, IUsuarioService usuarioService)
        {
            if (ingresoRepository == null)
            {
                throw new ArgumentNullException(nameof(ingresoRepository));
            }

            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _ingresoRepository = ingresoRepository;
            _usuarioService = usuarioService;
        }

        public ConsultaIngresos ConsultarIngresos(DateTime fechaInicio, DateTime fechaFin)
        {
            Usuario sesion = _usuarioService.ObtenerSesionActual();
            if (!string.Equals(sesion.NombreRol, ValoresDominio.Rol.Administrador, StringComparison.Ordinal))
            {
                throw new OperacionNoPermitidaException("Solo el perfil administrador puede consultar ingresos.");
            }

            DateTime inicio = fechaInicio.Date;
            DateTime fin = fechaFin.Date;
            if (inicio > fin)
            {
                throw new ValidacionNegocioException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }

            try
            {
                return _ingresoRepository.Consultar(inicio, fin);
            }
            catch (ErrorAccesoDatosException ex)
            {
                throw new ErrorInfraestructuraException("No se pudo consultar los ingresos." + DetalleMotor(ex), ex);
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
