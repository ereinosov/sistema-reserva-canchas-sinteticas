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
    /// Reglas de clientes (RF01-RF04 / RN02 / RN07).
    /// </summary>
    public sealed class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Composición de la aplicación: repositorio real y la misma sesión de usuario.
        /// </summary>
        /// <param name="usuarioService">Sesión autenticada (no crear una instancia nueva).</param>
        public ClienteService(IUsuarioService usuarioService)
            : this(new ClienteRepository(GestorConexion.Instancia), usuarioService)
        {
        }

        /// <summary>Constructor para pruebas unitarias con dobles.</summary>
        /// <param name="clienteRepository">Origen de persistencia.</param>
        /// <param name="usuarioService">Sesión autenticada.</param>
        internal ClienteService(IClienteRepository clienteRepository, IUsuarioService usuarioService)
        {
            if (clienteRepository == null)
            {
                throw new ArgumentNullException(nameof(clienteRepository));
            }

            if (usuarioService == null)
            {
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _clienteRepository = clienteRepository;
            _usuarioService = usuarioService;
        }

        /// <inheritdoc />
        public int RegistrarCliente(string nombre, string tipoDocumento, string numeroDocumento, string telefono, string correo)
        {
            _usuarioService.ObtenerSesionActual();
            Cliente cliente = ValidadorCliente.Normalizar(nombre, tipoDocumento, numeroDocumento, telefono, correo);
            return EjecutarDatos(
                () => _clienteRepository.Insertar(cliente),
                "No se pudo registrar el cliente.");
        }

        /// <inheritdoc />
        public IList<Cliente> ConsultarClientes(string numeroDocumento, string nombre)
        {
            _usuarioService.ObtenerSesionActual();
            string documento = string.IsNullOrWhiteSpace(numeroDocumento) ? null : numeroDocumento.Trim();
            string nombreFiltro = string.IsNullOrWhiteSpace(nombre) ? null : ValidadorCliente.EscaparLike(nombre.Trim());
            return EjecutarDatos(
                () => _clienteRepository.ObtenerTodos(documento, nombreFiltro),
                "No se pudo consultar los clientes.");
        }

        /// <inheritdoc />
        public void ModificarCliente(int idCliente, string nombre, string tipoDocumento, string numeroDocumento, string telefono, string correo)
        {
            _usuarioService.ObtenerSesionActual();
            if (idCliente <= 0)
            {
                throw new ValidacionNegocioException("Seleccione un cliente de la lista.");
            }

            Cliente cliente = ValidadorCliente.Normalizar(nombre, tipoDocumento, numeroDocumento, telefono, correo);
            cliente.IdCliente = idCliente;
            EjecutarDatos(
                () => _clienteRepository.Actualizar(cliente),
                "No se pudo modificar el cliente.");
        }

        /// <inheritdoc />
        public void EliminarCliente(int idCliente)
        {
            Usuario sesion = _usuarioService.ObtenerSesionActual();
            if (!string.Equals(sesion.NombreRol, ValoresDominio.Rol.Administrador, StringComparison.Ordinal))
            {
                throw new OperacionNoPermitidaException("Solo el perfil administrador puede eliminar clientes.");
            }

            if (idCliente <= 0)
            {
                throw new ValidacionNegocioException("Seleccione un cliente de la lista.");
            }

            // RN02: el motor rechaza la baja si hay reservas activas o pagos pendientes.
            EjecutarDatos(
                () => _clienteRepository.Eliminar(idCliente),
                "No se pudo eliminar el cliente.");
        }

        private static T EjecutarDatos<T>(Func<T> operacion, string mensajeInfraestructura)
        {
            try
            {
                return operacion();
            }
            catch (ErrorAccesoDatosException ex)
            {
                if (ex.NumeroSql == CodigosSql.ClienteDuplicado)
                {
                    throw new ValidacionNegocioException("Ya existe un cliente registrado con ese tipo y número de documento.");
                }

                if (ex.NumeroSql == CodigosSql.ClienteNoExiste)
                {
                    throw new ValidacionNegocioException("El cliente indicado no existe.");
                }

                if (ex.NumeroSql == CodigosSql.ClienteConReservasActivas)
                {
                    throw new ValidacionNegocioException("No se puede eliminar el cliente: tiene reservas activas.");
                }

                if (ex.NumeroSql == CodigosSql.ClienteConPagosPendientes)
                {
                    throw new ValidacionNegocioException("No se puede eliminar el cliente: tiene pagos pendientes.");
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
