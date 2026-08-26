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
    public class ReservaServiceTests
    {
        [TestMethod]
        public void Crear_SinSesion_Falla()
        {
            ReservaService servicio = new ReservaService(
                new ReservaRepositoryFake(),
                new HorarioRepositoryFake(),
                new UsuarioServiceFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.CrearReserva(1, new int[] { 2 }));
        }

        [TestMethod]
        public void Crear_DatosValidos_GuardaClienteHorarioYUsuario()
        {
            ReservaRepositoryFake repo = new ReservaRepositoryFake();

            int id = Servicio(repo).CrearReserva(4, new int[] { 12 });

            Assert.AreEqual(1, id);
            Assert.AreEqual(4, repo.UltimaInsertada.IdCliente);
            Assert.AreEqual(12, repo.UltimaInsertada.IdHorario);
            Assert.AreEqual(2, repo.UltimaInsertada.IdUsuario);
            Assert.AreEqual(ValoresDominio.EstadoReserva.Activa, repo.UltimaInsertada.EstadoReserva);
        }

        [TestMethod]
        public void Crear_SinCliente_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio().CrearReserva(0, new int[] { 12 }));
        }

        [TestMethod]
        public void Crear_SinFranjas_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio().CrearReserva(1, new int[0]));
        }

        [TestMethod]
        public void Crear_FranjaOcupada_Falla()
        {
            ReservaRepositoryFake repo = RepoConError(CodigosSql.FranjaOcupada);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).CrearReserva(1, new int[] { 8 }));
        }

        [TestMethod]
        public void Crear_FechaAnterior_Falla()
        {
            ReservaRepositoryFake repo = RepoConError(CodigosSql.FechaReservaAnterior);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).CrearReserva(1, new int[] { 8 }));
        }

        [TestMethod]
        public void Consultar_FiltrosVacios_PasaNulo()
        {
            ReservaRepositoryFake repo = new ReservaRepositoryFake();

            Servicio(repo).ConsultarReservas(null, 0, 0, "   ");

            Assert.IsNull(repo.UltimoClienteFiltro);
            Assert.IsNull(repo.UltimaCanchaFiltro);
            Assert.IsNull(repo.UltimoEstadoFiltro);
        }

        [TestMethod]
        public void ModificarHorario_ReservaCancelada_Falla()
        {
            ReservaRepositoryFake repo = RepoConError(CodigosSql.ReservaNoActivaParaModificar);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).ModificarHorario(3, 9));
        }

        [TestMethod]
        public void ModificarHorario_Activa_CambiaLaFranja()
        {
            ReservaRepositoryFake repo = new ReservaRepositoryFake();

            Servicio(repo).ModificarHorario(3, 15);

            Assert.AreEqual(3, repo.IdReservaActualizada);
            Assert.AreEqual(15, repo.IdHorarioActualizado);
        }

        [TestMethod]
        public void Cancelar_YaCancelada_Falla()
        {
            ReservaRepositoryFake repo = RepoConError(CodigosSql.ReservaYaCancelada);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(repo).CancelarReserva(5));
        }

        [TestMethod]
        public void Cancelar_Activa_Cancela()
        {
            ReservaRepositoryFake repo = new ReservaRepositoryFake();

            Servicio(repo).CancelarReserva(5);

            Assert.AreEqual(5, repo.IdCancelada);
        }

        [TestMethod]
        public void Disponibilidad_DevuelveFranjas()
        {
            HorarioRepositoryFake horarios = new HorarioRepositoryFake();
            horarios.Franjas.Add(new Horario
            {
                IdHorario = 1,
                IdCancha = 2,
                FechaHorario = new DateTime(2026, 8, 24),
                EstadoFranja = ValoresDominio.EstadoFranja.Libre
            });

            IList<Horario> resultado = Servicio(new ReservaRepositoryFake(), horarios)
                .ConsultarDisponibilidad(2, new DateTime(2026, 8, 24, 15, 0, 0));

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(2, horarios.UltimoIdCancha);
            Assert.AreEqual(new DateTime(2026, 8, 24), horarios.UltimaFecha);
        }

        [TestMethod]
        public void Disponibilidad_CanchaInexistente_Falla()
        {
            HorarioRepositoryFake horarios = new HorarioRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("cancha", CodigosSql.CanchaNoExiste)
            };

            Assert.ThrowsException<ValidacionNegocioException>(
                () => Servicio(new ReservaRepositoryFake(), horarios).ConsultarDisponibilidad(99, DateTime.Today));
        }

        private static ReservaRepositoryFake RepoConError(int codigo)
        {
            return new ReservaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("error", codigo)
            };
        }

        private static ReservaService Servicio()
        {
            return Servicio(new ReservaRepositoryFake());
        }

        private static ReservaService Servicio(ReservaRepositoryFake reservas)
        {
            return Servicio(reservas, new HorarioRepositoryFake());
        }

        private static ReservaService Servicio(ReservaRepositoryFake reservas, HorarioRepositoryFake horarios)
        {
            return new ReservaService(reservas, horarios, new UsuarioServiceFake { Sesion = SesionPrueba.Empleado() });
        }
    }
}
