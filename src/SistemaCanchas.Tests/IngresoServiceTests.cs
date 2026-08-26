using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class IngresoServiceTests
    {
        [TestMethod]
        public void Consultar_SinSesion_Falla()
        {
            IngresoService servicio = new IngresoService(new IngresoRepositoryFake(), new UsuarioServiceFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.ConsultarIngresos(DateTime.Today, DateTime.Today));
        }

        [TestMethod]
        public void Consultar_Empleado_NoPuede()
        {
            IngresoService servicio = new IngresoService(
                new IngresoRepositoryFake(),
                new UsuarioServiceFake { Sesion = SesionPrueba.Empleado() });

            Assert.ThrowsException<OperacionNoPermitidaException>(
                () => servicio.ConsultarIngresos(DateTime.Today, DateTime.Today));
        }

        [TestMethod]
        public void Consultar_RangoInvertido_Falla()
        {
            IngresoService servicio = ServicioAdmin(new IngresoRepositoryFake());

            Assert.ThrowsException<ValidacionNegocioException>(
                () => servicio.ConsultarIngresos(new DateTime(2026, 8, 24), new DateTime(2026, 8, 1)));
        }

        [TestMethod]
        public void Consultar_Administrador_DevuelveTotalYDetalle()
        {
            IngresoRepositoryFake repo = new IngresoRepositoryFake();
            repo.Resultado.TotalIngresos = 50.00m;
            repo.Resultado.Detalle.Add(new Ingreso { IdPago = 1, IdReserva = 3, MontoPago = 50.00m });
            DateTime inicio = new DateTime(2026, 8, 1, 10, 0, 0);
            DateTime fin = new DateTime(2026, 8, 24, 18, 0, 0);

            ConsultaIngresos consulta = ServicioAdmin(repo).ConsultarIngresos(inicio, fin);

            Assert.AreEqual(50.00m, consulta.TotalIngresos);
            Assert.AreEqual(1, consulta.Detalle.Count);
            Assert.AreEqual(new DateTime(2026, 8, 1), repo.UltimaFechaInicio);
            Assert.AreEqual(new DateTime(2026, 8, 24), repo.UltimaFechaFin);
        }

        private static IngresoService ServicioAdmin(IngresoRepositoryFake repo)
        {
            return new IngresoService(repo, new UsuarioServiceFake { Sesion = SesionPrueba.Admin() });
        }
    }
}
