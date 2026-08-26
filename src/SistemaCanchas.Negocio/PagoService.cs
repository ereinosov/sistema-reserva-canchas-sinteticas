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
    /// Reglas de pagos (RF09-RF10 / RN04).
    /// </summary>
    public sealed class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Composición de la aplicación: repositorio real y la misma sesión de usuario.
        /// </summary>
        /// <param name="usuarioService">Sesión autenticada (no crear una instancia nueva).</param>
        public PagoService(IUsuarioService usuarioService)
            : this(new PagoRepository(GestorConexion.Instancia), usuarioService)
        {
        }

        /// <summary>Constructor para pruebas unitarias con dobles.</summary>
        /// <param name="pagoRepository">Origen de persistencia.</param>
        /// <param name="usuarioService">Sesión autenticada.</param>
        internal PagoService(IPagoRepository pagoRepository, IUsuarioService usuarioService)
        {
            if (pagoRepository == null)
            {
                throw new ArgumentNullException(nameof(pagoRepository));
            }

            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _pagoRepository = pagoRepository;
            _usuarioService = usuarioService;
        }

        public int RegistrarPago(int idReserva, decimal monto, DateTime fechaPago, string estadoPago)
        {
            _usuarioService.ObtenerSesionActual();
            Pago pago = ValidadorPago.Normalizar(idReserva, monto, fechaPago, estadoPago);
            return EjecutarDatos(
                () => _pagoRepository.Insertar(pago),
                "No se pudo registrar el pago.");
        }

        public IList<Pago> ConsultarEstadoPago(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva)
        {
            _usuarioService.ObtenerSesionActual();
            int? cliente = idCliente.HasValue && idCliente.Value > 0 ? idCliente : null;
            int? cancha = idCancha.HasValue && idCancha.Value > 0 ? idCancha : null;
            string estado = ValidadorReserva.NormalizarEstado(estadoReserva);
            return EjecutarDatos(
                () => _pagoRepository.ObtenerTodos(fecha, cliente, cancha, estado),
                "No se pudo consultar el estado de pago.");
        }

        private static T EjecutarDatos<T>(Func<T> operacion, string mensajeInfraestructura)
        {
            try
            {
                return operacion();
            }
            catch (ErrorAccesoDatosException ex)
            {
                if (ex.NumeroSql == CodigosSql.ReservaNoActivaParaPago)
                {
                    throw new ValidacionNegocioException("La reserva indicada no existe o no se encuentra activa.");
                }

                if (ex.NumeroSql == CodigosSql.PagoYaRegistrado ||
                    ex.NumeroSql == CodigosSql.IndiceUnicoDuplicado ||
                    ex.NumeroSql == CodigosSql.RestriccionUnicaDuplicada)
                {
                    throw new ValidacionNegocioException("La reserva indicada ya tiene un pago registrado.");
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
