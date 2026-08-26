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
        public void Registrar_SinSesion_Falla()
        {
            PagoService servicio = new PagoService(new PagoRepositoryFake(), new UsuarioServiceFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.RegistrarPago(1, 20m, DateTime.Today, ValoresDominio.EstadoPago.Pagado));
        }

        [TestMethod]
        public void Registrar_MontoCero_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio().RegistrarPago(1, 0m, DateTime.Today, ValoresDominio.EstadoPago.Pagado));
        }

        [TestMethod]
        public void Registrar_DatosValidos_RedondeaMontoYFecha()
        {
            PagoRepositoryFake repo = new PagoRepositoryFake();
            DateTime fecha = new DateTime(2026, 8, 24, 15, 30, 0);

            int id = Servicio(repo).RegistrarPago(7, 25.456m, fecha, ValoresDominio.EstadoPago.Pagado);

            Assert.AreEqual(1, id);
            Assert.AreEqual(7, repo.UltimoInsertado.IdReserva);
            Assert.AreEqual(25.46m, repo.UltimoInsertado.MontoPago);
            Assert.AreEqual(new DateTime(2026, 8, 24), repo.UltimoInsertado.FechaPago);
            Assert.AreEqual(ValoresDominio.EstadoPago.Pagado, repo.UltimoInsertado.EstadoPago);
        }

        [TestMethod]
        public void Registrar_ReservaInactiva_Falla()
        {
            PagoRepositoryFake repo = RepoConError(CodigosSql.ReservaNoActivaParaPago);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).RegistrarPago(3, 10m, DateTime.Today, ValoresDominio.EstadoPago.Pagado));
        }

        [TestMethod]
        public void Registrar_YaTienePago_Falla()
        {
            PagoRepositoryFake repo = RepoConError(CodigosSql.PagoYaRegistrado);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).RegistrarPago(3, 10m, DateTime.Today, ValoresDominio.EstadoPago.Pagado));
        }

        [TestMethod]
        public void Consultar_SinFiltro_PasaNulo()
        {
            PagoRepositoryFake repo = new PagoRepositoryFake();
            repo.Pagos.Add(new Pago { IdReserva = 1, EstadoPago = ValoresDominio.EstadoPago.Pendiente });

            IList<Pago> resultado = Servicio(repo).ConsultarEstadoPago(null, 0, 0, "   ");

            Assert.AreEqual(1, resultado.Count);
            Assert.IsNull(repo.UltimaFechaFiltro);
            Assert.IsNull(repo.UltimoClienteFiltro);
        }

        [TestMethod]
        public void Consultar_ConFiltros_LosPasaAlRepositorio()
        {
            PagoRepositoryFake repo = new PagoRepositoryFake();
            DateTime fecha = new DateTime(2026, 8, 24);

            Servicio(repo).ConsultarEstadoPago(fecha, 4, 2, ValoresDominio.EstadoReserva.Activa);

            Assert.AreEqual(fecha, repo.UltimaFechaFiltro);
            Assert.AreEqual(4, repo.UltimoClienteFiltro);
            Assert.AreEqual(2, repo.UltimaCanchaFiltro);
            Assert.AreEqual(ValoresDominio.EstadoReserva.Activa, repo.UltimoEstadoFiltro);
        }

        private static PagoRepositoryFake RepoConError(int codigo)
        {
            return new PagoRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("error", codigo)
            };
        }

        private static PagoService Servicio()
        {
            return Servicio(new PagoRepositoryFake());
        }

        private static PagoService Servicio(PagoRepositoryFake repo)
        {
            return new PagoService(repo, new UsuarioServiceFake { Sesion = SesionPrueba.Empleado() });
        }
    }
}
