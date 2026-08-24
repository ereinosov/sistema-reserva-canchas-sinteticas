using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using SistemaCanchas.Datos;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Tests
{
    internal sealed class UsuarioRepositoryFake : IUsuarioRepository
    {
        public Usuario UsuarioADevolver { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public string UltimoLoginConsultado { get; private set; }

        public IList<Usuario> UsuariosExistentes { get; set; }

        public Usuario UltimoInsertado { get; private set; }

        public string UltimaClaveBdPlana { get; private set; }

        public string UltimoRolInsertado { get; private set; }

        public bool InsertoDesdeInstalacion { get; private set; }

        public int SiguienteId { get; set; }

        public int IdDesactivado { get; private set; }

        public UsuarioRepositoryFake()
        {
            UsuariosExistentes = new List<Usuario>();
            SiguienteId = 10;
        }

        public Usuario ObtenerCredenciales(string usuarioLogin)
        {
            UltimoLoginConsultado = usuarioLogin;
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            return UsuarioADevolver;
        }

        public IList<Usuario> ObtenerTodos()
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            return UsuariosExistentes;
        }

        public IList<Usuario> ObtenerTodosDesdeInstalacion()
        {
            return ObtenerTodos();
        }

        public int Insertar(Usuario usuario, string claveBdPlana, string nombreRol)
        {
            return Registrar(usuario, claveBdPlana, nombreRol, false);
        }

        public int InsertarDesdeInstalacion(Usuario usuario, string claveBdPlana, string nombreRol)
        {
            return Registrar(usuario, claveBdPlana, nombreRol, true);
        }

        public bool Desactivar(int idUsuario)
        {
            IdDesactivado = idUsuario;
            return true;
        }

        private int Registrar(Usuario usuario, string claveBdPlana, string nombreRol, bool instalacion)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoInsertado = usuario;
            UltimaClaveBdPlana = claveBdPlana;
            UltimoRolInsertado = nombreRol;
            InsertoDesdeInstalacion = instalacion;
            return SiguienteId;
        }
    }

    internal sealed class GestorConexionFake : IGestorConexion
    {
        public bool SesionActiva { get; private set; }

        public string UsuarioBdAsignado { get; private set; }

        public string ClaveBdAsignada { get; private set; }

        public string ClaveDescifrada { get; set; }

        public Exception ErrorDescifrado { get; set; }

        public Exception ErrorSesion { get; set; }

        public int VecesCerrarSesion { get; private set; }

        public SqlConnection ObtenerConexionBootstrap()
        {
            throw new NotSupportedException();
        }

        public SqlConnection ObtenerConexionActiva()
        {
            throw new NotSupportedException();
        }

        public SqlConnection ObtenerConexionInstalacion()
        {
            throw new NotSupportedException();
        }

        public void EstablecerSesion(string usuarioBd, string claveBdPlana)
        {
            if (ErrorSesion != null)
            {
                throw ErrorSesion;
            }

            UsuarioBdAsignado = usuarioBd;
            ClaveBdAsignada = claveBdPlana;
            SesionActiva = true;
        }

        public void CerrarSesion()
        {
            SesionActiva = false;
            VecesCerrarSesion++;
        }

        public string CifrarClaveBd(string clavePlana)
        {
            return "enc:" + clavePlana;
        }

        public string DescifrarClaveBd(string claveCifrada)
        {
            if (ErrorDescifrado != null)
            {
                throw ErrorDescifrado;
            }

            return ClaveDescifrada ?? "clave-motor-prueba";
        }
    }

    internal sealed class UsuarioServiceFake : SistemaCanchas.Negocio.Interfaces.IUsuarioService
    {
        public Usuario Sesion { get; set; }

        public Usuario ValidarCredenciales(string usuarioLogin, string claveApp)
        {
            throw new NotSupportedException();
        }

        public Usuario ObtenerSesionActual()
        {
            if (Sesion == null)
            {
                throw new SistemaCanchas.Negocio.Excepciones.SesionNoIniciadaException();
            }

            return Sesion;
        }

        public void CerrarSesion()
        {
        }

        public int RegistrarUsuario(string nombreUsuario, string usuarioLogin, string claveApp, string nombreRol)
        {
            throw new NotSupportedException();
        }

        public int RegistrarAdministradorInicial(string nombreUsuario, string usuarioLogin, string claveApp)
        {
            throw new NotSupportedException();
        }

        public void DesactivarUsuario(int idUsuario)
        {
            throw new NotSupportedException();
        }

        public IList<Usuario> ObtenerTodos()
        {
            throw new NotSupportedException();
        }
    }

    internal sealed class CanchaRepositoryFake : ICanchaRepository
    {
        public IList<Cancha> Canchas { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public Cancha UltimaInsertada { get; private set; }

        public Cancha UltimaActualizada { get; private set; }

        public int IdDesactivado { get; private set; }

        public int SiguienteId { get; set; }

        public CanchaRepositoryFake()
        {
            Canchas = new List<Cancha>();
            SiguienteId = 1;
        }

        public int Insertar(Cancha cancha)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimaInsertada = cancha;
            int id = SiguienteId;
            SiguienteId++;
            return id;
        }

        public IList<Cancha> ObtenerTodos(string estadoCancha)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            if (string.IsNullOrEmpty(estadoCancha))
            {
                return Canchas;
            }

            List<Cancha> filtradas = new List<Cancha>();
            for (int i = 0; i < Canchas.Count; i++)
            {
                if (string.Equals(Canchas[i].EstadoCancha, estadoCancha, StringComparison.Ordinal))
                {
                    filtradas.Add(Canchas[i]);
                }
            }

            return filtradas;
        }

        public bool Actualizar(Cancha cancha)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimaActualizada = cancha;
            return true;
        }

        public bool Desactivar(int idCancha)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            IdDesactivado = idCancha;
            return true;
        }
    }

    internal sealed class ClienteRepositoryFake : IClienteRepository
    {
        public IList<Cliente> Clientes { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public Cliente UltimoInsertado { get; private set; }

        public Cliente UltimoActualizado { get; private set; }

        public int IdEliminado { get; private set; }

        public string UltimoDocumentoFiltro { get; private set; }

        public string UltimoNombreFiltro { get; private set; }

        public int SiguienteId { get; set; }

        public ClienteRepositoryFake()
        {
            Clientes = new List<Cliente>();
            SiguienteId = 1;
        }

        public int Insertar(Cliente cliente)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoInsertado = cliente;
            int id = SiguienteId;
            SiguienteId++;
            return id;
        }

        public IList<Cliente> ObtenerTodos(string numeroDocumento, string nombre)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoDocumentoFiltro = numeroDocumento;
            UltimoNombreFiltro = nombre;
            return Clientes;
        }

        public bool Actualizar(Cliente cliente)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoActualizado = cliente;
            return true;
        }

        public bool Eliminar(int idCliente)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            IdEliminado = idCliente;
            return true;
        }
    }

    internal sealed class ReservaRepositoryFake : IReservaRepository
    {
        public IList<Reserva> Reservas { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public Reserva UltimaInsertada { get; private set; }

        public int IdHorarioActualizado { get; private set; }

        public int IdReservaActualizada { get; private set; }

        public int IdCancelada { get; private set; }

        public DateTime? UltimaFechaFiltro { get; private set; }

        public int? UltimoClienteFiltro { get; private set; }

        public int? UltimaCanchaFiltro { get; private set; }

        public string UltimoEstadoFiltro { get; private set; }

        public int SiguienteId { get; set; }

        public ReservaRepositoryFake()
        {
            Reservas = new List<Reserva>();
            SiguienteId = 1;
        }

        public int Insertar(Reserva reserva)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimaInsertada = reserva;
            int id = SiguienteId;
            SiguienteId++;
            return id;
        }

        public IList<Reserva> ObtenerTodos(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimaFechaFiltro = fecha;
            UltimoClienteFiltro = idCliente;
            UltimaCanchaFiltro = idCancha;
            UltimoEstadoFiltro = estadoReserva;
            return Reservas;
        }

        public bool ActualizarHorario(int idReserva, int nuevoIdHorario)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            IdReservaActualizada = idReserva;
            IdHorarioActualizado = nuevoIdHorario;
            return true;
        }

        public bool Cancelar(int idReserva)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            IdCancelada = idReserva;
            return true;
        }
    }

    internal sealed class HorarioRepositoryFake : IHorarioRepository
    {
        public IList<Horario> Franjas { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public int UltimoIdCancha { get; private set; }

        public DateTime UltimaFecha { get; private set; }

        public HorarioRepositoryFake()
        {
            Franjas = new List<Horario>();
        }

        public IList<Horario> ConsultarDisponibilidad(int idCancha, DateTime fecha)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoIdCancha = idCancha;
            UltimaFecha = fecha;
            return Franjas;
        }
    }

    internal sealed class PagoRepositoryFake : IPagoRepository
    {
        public IList<Pago> Pagos { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public Pago UltimoInsertado { get; private set; }

        public int? UltimoIdReservaFiltro { get; private set; }

        public int SiguienteId { get; set; }

        public PagoRepositoryFake()
        {
            Pagos = new List<Pago>();
            SiguienteId = 1;
        }

        public int Insertar(Pago pago)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoInsertado = pago;
            int id = SiguienteId;
            SiguienteId++;
            return id;
        }

        public IList<Pago> ObtenerTodos(int? idReserva)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimoIdReservaFiltro = idReserva;
            return Pagos;
        }
    }

    internal sealed class IngresoRepositoryFake : IIngresoRepository
    {
        public ConsultaIngresos Resultado { get; set; }

        public Exception ExcepcionALanzar { get; set; }

        public DateTime UltimaFechaInicio { get; private set; }

        public DateTime UltimaFechaFin { get; private set; }

        public IngresoRepositoryFake()
        {
            Resultado = new ConsultaIngresos();
        }

        public ConsultaIngresos Consultar(DateTime fechaInicio, DateTime fechaFin)
        {
            if (ExcepcionALanzar != null)
            {
                throw ExcepcionALanzar;
            }

            UltimaFechaInicio = fechaInicio;
            UltimaFechaFin = fechaFin;
            return Resultado;
        }
    }
}
