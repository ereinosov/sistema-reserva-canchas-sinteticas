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
    /// Reglas de reservas (RF05-RF08 / RF11 / RN01 / RN03 / RN06 / RN08).
    /// </summary>
    public sealed class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Composición de la aplicación: repositorios reales y la misma sesión de usuario.
        /// </summary>
        /// <param name="usuarioService">Sesión autenticada (no crear una instancia nueva).</param>
        public ReservaService(IUsuarioService usuarioService)
            : this(
                new ReservaRepository(GestorConexion.Instancia),
                new HorarioRepository(GestorConexion.Instancia),
                usuarioService)
        {
        }

        /// <summary>Constructor para pruebas unitarias con dobles.</summary>
        /// <param name="reservaRepository">Persistencia de reservas.</param>
        /// <param name="horarioRepository">Consulta de franjas.</param>
        /// <param name="usuarioService">Sesión autenticada.</param>
        internal ReservaService(
            IReservaRepository reservaRepository,
            IHorarioRepository horarioRepository,
            IUsuarioService usuarioService)
        {
            if (reservaRepository == null)
            {
                throw new ArgumentNullException(nameof(reservaRepository));
            }

            if (horarioRepository == null)
            {
                throw new ArgumentNullException(nameof(horarioRepository));
            }

            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _reservaRepository = reservaRepository;
            _horarioRepository = horarioRepository;
            _usuarioService = usuarioService;
        }

        public int CrearReserva(int idCliente, IList<int> idsHorario)
        {
            Usuario sesion = _usuarioService.ObtenerSesionActual();
            ValidadorReserva.ExigirId(idCliente, "Seleccione un cliente.");
            IList<int> horarios = NormalizarIdsHorario(idsHorario);

            Reserva reserva = new Reserva
            {
                IdCliente = idCliente,
                IdHorario = horarios[0],
                IdUsuario = sesion.IdUsuario,
                EstadoReserva = ValoresDominio.EstadoReserva.Activa
            };

            return EjecutarDatos(
                () => _reservaRepository.Insertar(reserva, horarios),
                "No se pudo registrar la reserva.");
        }

        public IList<Reserva> ConsultarReservas(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva)
        {
            _usuarioService.ObtenerSesionActual();
            int? cliente = idCliente.HasValue && idCliente.Value > 0 ? idCliente : null;
            int? cancha = idCancha.HasValue && idCancha.Value > 0 ? idCancha : null;
            string estado = ValidadorReserva.NormalizarEstado(estadoReserva);
            return EjecutarDatos(
                () => _reservaRepository.ObtenerTodos(fecha, cliente, cancha, estado),
                "No se pudo consultar las reservas.");
        }

        public void ModificarHorario(int idReserva, int nuevoIdHorario)
        {
            _usuarioService.ObtenerSesionActual();
            ValidadorReserva.ExigirId(idReserva, "Seleccione una reserva de la lista.");
            ValidadorReserva.ExigirId(nuevoIdHorario, "Seleccione una franja horaria libre.");
            EjecutarDatos(
                () => _reservaRepository.ActualizarHorario(idReserva, nuevoIdHorario),
                "No se pudo modificar el horario de la reserva.");
        }

        public void CancelarReserva(int idReserva)
        {
            _usuarioService.ObtenerSesionActual();
            ValidadorReserva.ExigirId(idReserva, "Seleccione una reserva de la lista.");
            EjecutarDatos(
                () => _reservaRepository.Cancelar(idReserva),
                "No se pudo cancelar la reserva.");
        }

        public IList<Horario> ConsultarDisponibilidad(int idCancha, DateTime fecha)
        {
            _usuarioService.ObtenerSesionActual();
            ValidadorReserva.ExigirId(idCancha, "Seleccione una cancha.");
            return EjecutarDatos(
                () => _horarioRepository.ConsultarDisponibilidad(idCancha, fecha.Date),
                "No se pudo consultar la disponibilidad.");
        }

        private static T EjecutarDatos<T>(Func<T> operacion, string mensajeInfraestructura)
        {
            try
            {
                return operacion();
            }
            catch (ErrorAccesoDatosException ex)
            {
                throw Traducir(ex, mensajeInfraestructura);
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

        private static Exception Traducir(ErrorAccesoDatosException ex, string mensajeInfraestructura)
        {
            if (ex.NumeroSql == CodigosSql.ListaHorariosVacia)
            {
                return new ValidacionNegocioException("Debe seleccionar al menos una franja horaria.");
            }

            if (ex.NumeroSql == CodigosSql.ClienteNoExiste)
            {
                return new ValidacionNegocioException("El cliente indicado no existe.");
            }

            if (ex.NumeroSql == CodigosSql.UsuarioRegistroInactivo)
            {
                return new ValidacionNegocioException("El usuario que registra la reserva no existe o está inactivo.");
            }

            if (ex.NumeroSql == CodigosSql.FranjaNoExiste)
            {
                return new ValidacionNegocioException("La franja horaria indicada no existe.");
            }

            if (ex.NumeroSql == CodigosSql.CanchaNoActiva)
            {
                return new ValidacionNegocioException("La cancha de la franja seleccionada no está activa.");
            }

            if (ex.NumeroSql == CodigosSql.CanchaNoExiste)
            {
                return new ValidacionNegocioException("La cancha indicada no existe.");
            }

            if (ex.NumeroSql == CodigosSql.FechaReservaAnterior)
            {
                return new ValidacionNegocioException("No se pueden registrar ni reprogramar reservas con fecha anterior a la actual.");
            }

            if (ex.NumeroSql == CodigosSql.FranjaOcupada ||
                ex.NumeroSql == CodigosSql.IndiceUnicoDuplicado ||
                ex.NumeroSql == CodigosSql.RestriccionUnicaDuplicada)
            {
                return new ValidacionNegocioException("La franja horaria seleccionada ya se encuentra ocupada.");
            }

            if (ex.NumeroSql == CodigosSql.ReservaNoExiste)
            {
                return new ValidacionNegocioException("La reserva indicada no existe.");
            }

            if (ex.NumeroSql == CodigosSql.ReservaNoActivaParaModificar)
            {
                return new ValidacionNegocioException("Solo se puede modificar el horario de una reserva activa.");
            }

            if (ex.NumeroSql == CodigosSql.ReservaYaCancelada)
            {
                return new ValidacionNegocioException("La reserva ya se encuentra cancelada.");
            }

            return new ErrorInfraestructuraException(mensajeInfraestructura + DetalleMotor(ex), ex);
        }

        private static IList<int> NormalizarIdsHorario(IList<int> idsHorario)
        {
            if (idsHorario == null || idsHorario.Count == 0)
            {
                throw new ValidacionNegocioException("Seleccione al menos una franja horaria libre.");
            }

            List<int> unicos = new List<int>();
            for (int i = 0; i < idsHorario.Count; i++)
            {
                ValidadorReserva.ExigirId(idsHorario[i], "Seleccione una franja horaria libre.");
                if (!unicos.Contains(idsHorario[i]))
                {
                    unicos.Add(idsHorario[i]);
                }
            }

            return unicos;
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
