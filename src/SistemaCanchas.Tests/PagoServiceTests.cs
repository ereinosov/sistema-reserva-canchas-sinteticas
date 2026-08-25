using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class PagoServiceTests
    {
        [TestMethod]
        public void RegistrarPago_SinSesion_LanzaSesionNoIniciada()
        {
            PagoService servicio = new PagoService(new PagoRepositoryFake(), new UsuarioServiceFake());

            try
            {
                servicio.RegistrarPago(1, 20m, DateTime.Today, ValoresDominio.EstadoPago.Pagado);
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void RegistrarPago_MontoCero_LanzaValidacion()
        {
            PagoService servicio = CrearServicioEmpleado(new PagoRepositoryFake());

            try
            {
                servicio.RegistrarPago(1, 0m, DateTime.Today, ValoresDominio.EstadoPago.Pagado);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarPago_DatosValidos_PersisteRedondeado()
        {
            PagoRepositoryFake repositorio = new PagoRepositoryFake();
            PagoService servicio = CrearServicioEmpleado(repositorio);
            DateTime fecha = new DateTime(2026, 8, 24, 15, 30, 0);

            int id = servicio.RegistrarPago(7, 25.456m, fecha, ValoresDominio.EstadoPago.Pagado);

            Assert.AreEqual(1, id);
            Assert.AreEqual(7, repositorio.UltimoInsertado.IdReserva);
            Assert.AreEqual(25.46m, repositorio.UltimoInsertado.MontoPago);
            Assert.AreEqual(new DateTime(2026, 8, 24), repositorio.UltimoInsertado.FechaPago);
            Assert.AreEqual(ValoresDominio.EstadoPago.Pagado, repositorio.UltimoInsertado.EstadoPago);
        }

        [TestMethod]
        public void RegistrarPago_ReservaInactiva_LanzaValidacion()
        {
            PagoRepositoryFake repositorio = new PagoRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("inactiva", CodigosSql.ReservaNoActivaParaPago)
            };
            PagoService servicio = CrearServicioEmpleado(repositorio);

            try
            {
                servicio.RegistrarPago(3, 10m, DateTime.Today, ValoresDominio.EstadoPago.Pagado);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarPago_YaRegistrado_LanzaValidacion()
        {
            PagoRepositoryFake repositorio = new PagoRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicado", CodigosSql.PagoYaRegistrado)
            };
            PagoService servicio = CrearServicioEmpleado(repositorio);

            try
            {
                servicio.RegistrarPago(3, 10m, DateTime.Today, ValoresDominio.EstadoPago.Pagado);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ConsultarEstadoPago_SinFiltro_PasaNulo()
        {
            PagoRepositoryFake repositorio = new PagoRepositoryFake();
            repositorio.Pagos.Add(new Pago { IdReserva = 1, EstadoPago = ValoresDominio.EstadoPago.Pendiente });
            PagoService servicio = CrearServicioEmpleado(repositorio);

            IList<Pago> resultado = servicio.ConsultarEstadoPago(null, 0, 0, "   ");

            Assert.AreEqual(1, resultado.Count);
            Assert.IsNull(repositorio.UltimaFechaFiltro);
            Assert.IsNull(repositorio.UltimoClienteFiltro);
            Assert.IsNull(repositorio.UltimaCanchaFiltro);
            Assert.IsNull(repositorio.UltimoEstadoFiltro);
        }

        [TestMethod]
        public void ConsultarEstadoPago_ConFiltros_PasaValores()
        {
            PagoRepositoryFake repositorio = new PagoRepositoryFake();
            PagoService servicio = CrearServicioEmpleado(repositorio);
            DateTime fecha = new DateTime(2026, 8, 24);

            servicio.ConsultarEstadoPago(fecha, 4, 2, ValoresDominio.EstadoReserva.Activa);

            Assert.AreEqual(fecha, repositorio.UltimaFechaFiltro);
            Assert.AreEqual(4, repositorio.UltimoClienteFiltro);
            Assert.AreEqual(2, repositorio.UltimaCanchaFiltro);
            Assert.AreEqual(ValoresDominio.EstadoReserva.Activa, repositorio.UltimoEstadoFiltro);
        }

        private static PagoService CrearServicioEmpleado(PagoRepositoryFake repositorio)
        {
            return new PagoService(repositorio, new UsuarioServiceFake { Sesion = CrearSesionEmpleado() });
        }

        private static Usuario CrearSesionEmpleado()
        {
            return new Usuario
            {
                IdUsuario = 2,
                NombreUsuario = "Ana",
                UsuarioLogin = "ana",
                NombreRol = ValoresDominio.Rol.Empleado,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo
            };
        }
    }
}
