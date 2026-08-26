using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class CanchaServiceTests
    {
        private static readonly TimeSpan Abre = TimeSpan.FromHours(ValoresDominio.HoraInicioFranja);
        private static readonly TimeSpan Cierra = TimeSpan.FromHours(ValoresDominio.HoraFinOperacion);

        [TestMethod]
        public void Registrar_SinSesion_Falla()
        {
            CanchaService servicio = new CanchaService(new CanchaRepositoryFake(), new UsuarioServiceFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.RegistrarCancha("Cancha 1", Abre, Cierra));
        }

        [TestMethod]
        public void Registrar_Empleado_NoPuede()
        {
            CanchaService servicio = new CanchaService(
                new CanchaRepositoryFake(),
                new UsuarioServiceFake { Sesion = SesionPrueba.Empleado() });

            Assert.ThrowsException<OperacionNoPermitidaException>(
                () => servicio.RegistrarCancha("Cancha 1", Abre, Cierra));
        }

        [TestMethod]
        public void Registrar_NombreVacio_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin().RegistrarCancha("   ", Abre, Cierra));
        }

        [TestMethod]
        public void Registrar_HorarioInvertido_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin().RegistrarCancha("Cancha 1", Cierra, Abre));
        }

        [TestMethod]
        public void Registrar_NombreValido_GuardaSinEspaciosDeMas()
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake();

            int id = ServicioAdmin(repo).RegistrarCancha("  Cancha Norte  ", Abre, Cierra);

            Assert.AreEqual(1, id);
            Assert.AreEqual("Cancha Norte", repo.UltimaInsertada.NombreCancha);
            Assert.AreEqual(ValoresDominio.EstadoCancha.Activa, repo.UltimaInsertada.EstadoCancha);
            Assert.AreEqual(Abre, repo.UltimaInsertada.HoraInicioOperacion);
            Assert.AreEqual(Cierra, repo.UltimaInsertada.HoraFinOperacion);
        }

        [TestMethod]
        public void Registrar_NombreDuplicado_Falla()
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicada", CodigosSql.CanchaDuplicada)
            };

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin(repo).RegistrarCancha("Cancha 1", Abre, Cierra));
        }

        [TestMethod]
        public void Modificar_ActualizaNombreYHorario()
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake();

            ServicioAdmin(repo).ModificarCancha(4, "  Cancha Central  ", Abre, Cierra);

            Assert.AreEqual(4, repo.UltimaActualizada.IdCancha);
            Assert.AreEqual("Cancha Central", repo.UltimaActualizada.NombreCancha);
            Assert.AreEqual(Abre, repo.UltimaActualizada.HoraInicioOperacion);
        }

        [TestMethod]
        public void Desactivar_YaInactiva_Falla()
        {
            CanchaRepositoryFake repo = Cancha(3, "Sur", ValoresDominio.EstadoCancha.Inactiva);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin(repo).DesactivarCancha(3));
        }

        [TestMethod]
        public void Desactivar_Activa_Desactiva()
        {
            CanchaRepositoryFake repo = Cancha(2, "Norte", ValoresDominio.EstadoCancha.Activa);

            ServicioAdmin(repo).DesactivarCancha(2);

            Assert.AreEqual(2, repo.IdDesactivado);
        }

        [TestMethod]
        public void Activar_YaActiva_Falla()
        {
            CanchaRepositoryFake repo = Cancha(2, "Norte", ValoresDominio.EstadoCancha.Activa);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin(repo).ActivarCancha(2));
        }

        [TestMethod]
        public void Activar_Inactiva_Activa()
        {
            CanchaRepositoryFake repo = Cancha(3, "Sur", ValoresDominio.EstadoCancha.Inactiva);

            ServicioAdmin(repo).ActivarCancha(3);

            Assert.AreEqual(3, repo.IdActivado);
        }

        [TestMethod]
        public void CanchaActiva_SegunEstado()
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake();
            repo.Canchas.Add(new Cancha { IdCancha = 1, NombreCancha = "Norte", EstadoCancha = ValoresDominio.EstadoCancha.Activa });
            repo.Canchas.Add(new Cancha { IdCancha = 2, NombreCancha = "Sur", EstadoCancha = ValoresDominio.EstadoCancha.Inactiva });
            CanchaService servicio = ServicioAdmin(repo);

            Assert.IsTrue(servicio.CanchaActiva(1));
            Assert.IsFalse(servicio.CanchaActiva(2));
            Assert.IsFalse(servicio.CanchaActiva(99));
        }

        [TestMethod]
        public void ObtenerActivas_NoIncluyeInactivas()
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake();
            repo.Canchas.Add(new Cancha { IdCancha = 1, NombreCancha = "Norte", EstadoCancha = ValoresDominio.EstadoCancha.Activa });
            repo.Canchas.Add(new Cancha { IdCancha = 2, NombreCancha = "Sur", EstadoCancha = ValoresDominio.EstadoCancha.Inactiva });
            CanchaService servicio = ServicioAdmin(repo);

            Assert.AreEqual(1, servicio.ObtenerActivas().Count);
            Assert.AreEqual(2, servicio.ObtenerTodas().Count);
        }

        private static CanchaRepositoryFake Cancha(int id, string nombre, string estado)
        {
            CanchaRepositoryFake repo = new CanchaRepositoryFake();
            repo.Canchas.Add(new Cancha { IdCancha = id, NombreCancha = nombre, EstadoCancha = estado });
            return repo;
        }

        private static CanchaService ServicioAdmin()
        {
            return ServicioAdmin(new CanchaRepositoryFake());
        }

        private static CanchaService ServicioAdmin(CanchaRepositoryFake repo)
        {
            return new CanchaService(repo, new UsuarioServiceFake { Sesion = SesionPrueba.Admin() });
        }
    }
}
