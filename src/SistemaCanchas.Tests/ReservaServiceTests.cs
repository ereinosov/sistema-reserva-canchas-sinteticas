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
        public void CrearReserva_SinSesion_LanzaSesionNoIniciada()
        {
            ReservaService servicio = new ReservaService(
                new ReservaRepositoryFake(),
                new HorarioRepositoryFake(),
                new UsuarioServiceFake());

            try
            {
                servicio.CrearReserva(1, new int[] { 2 });
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void CrearReserva_Empleado_PersisteClienteHorarioYUsuario()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake();
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            int id = servicio.CrearReserva(4, new int[] { 12 });

            Assert.AreEqual(1, id);
            Assert.AreEqual(4, repositorio.UltimaInsertada.IdCliente);
            Assert.AreEqual(12, repositorio.UltimaInsertada.IdHorario);
            Assert.AreEqual(2, repositorio.UltimaInsertada.IdUsuario);
            Assert.AreEqual(ValoresDominio.EstadoReserva.Activa, repositorio.UltimaInsertada.EstadoReserva);
        }

        [TestMethod]
        public void CrearReserva_SinCliente_LanzaValidacion()
        {
            ReservaService servicio = CrearServicioEmpleado(new ReservaRepositoryFake(), new HorarioRepositoryFake());

            try
            {
                servicio.CrearReserva(0, new int[] { 12 });
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void CrearReserva_SinFranjas_LanzaValidacion()
        {
            ReservaService servicio = CrearServicioEmpleado(new ReservaRepositoryFake(), new HorarioRepositoryFake());

            try
            {
                servicio.CrearReserva(1, new int[0]);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void CrearReserva_FranjaOcupada_LanzaValidacion()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("ocupada", CodigosSql.FranjaOcupada)
            };
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            try
            {
                servicio.CrearReserva(1, new int[] { 8 });
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void CrearReserva_FechaAnterior_LanzaValidacion()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("fecha", CodigosSql.FechaReservaAnterior)
            };
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            try
            {
                servicio.CrearReserva(1, new int[] { 8 });
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ConsultarReservas_NormalizaFiltrosVacios()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake();
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            servicio.ConsultarReservas(null, 0, 0, "   ");

            Assert.IsNull(repositorio.UltimoClienteFiltro);
            Assert.IsNull(repositorio.UltimaCanchaFiltro);
            Assert.IsNull(repositorio.UltimoEstadoFiltro);
        }

        [TestMethod]
        public void ModificarHorario_ReservaCancelada_LanzaValidacion()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("cancelada", CodigosSql.ReservaNoActivaParaModificar)
            };
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            try
            {
                servicio.ModificarHorario(3, 9);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ModificarHorario_Activa_ActualizaFranja()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake();
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            servicio.ModificarHorario(3, 15);

            Assert.AreEqual(3, repositorio.IdReservaActualizada);
            Assert.AreEqual(15, repositorio.IdHorarioActualizado);
        }

        [TestMethod]
        public void CancelarReserva_YaCancelada_LanzaValidacion()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("ya cancelada", CodigosSql.ReservaYaCancelada)
            };
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            try
            {
                servicio.CancelarReserva(5);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void CancelarReserva_Activa_Cancela()
        {
            ReservaRepositoryFake repositorio = new ReservaRepositoryFake();
            ReservaService servicio = CrearServicioEmpleado(repositorio, new HorarioRepositoryFake());

            servicio.CancelarReserva(5);

            Assert.AreEqual(5, repositorio.IdCancelada);
        }

        [TestMethod]
        public void ConsultarDisponibilidad_DevuelveFranjas()
        {
            HorarioRepositoryFake horarios = new HorarioRepositoryFake();
            horarios.Franjas.Add(new Horario
            {
                IdHorario = 1,
                IdCancha = 2,
                FechaHorario = new DateTime(2026, 8, 24),
                EstadoFranja = ValoresDominio.EstadoFranja.Libre
            });
            ReservaService servicio = CrearServicioEmpleado(new ReservaRepositoryFake(), horarios);

            IList<Horario> resultado = servicio.ConsultarDisponibilidad(2, new DateTime(2026, 8, 24, 15, 0, 0));

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(2, horarios.UltimoIdCancha);
            Assert.AreEqual(new DateTime(2026, 8, 24), horarios.UltimaFecha);
        }

        [TestMethod]
        public void ConsultarDisponibilidad_CanchaInexistente_LanzaValidacion()
        {
            HorarioRepositoryFake horarios = new HorarioRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("cancha", CodigosSql.CanchaNoExiste)
            };
            ReservaService servicio = CrearServicioEmpleado(new ReservaRepositoryFake(), horarios);

            try
            {
                servicio.ConsultarDisponibilidad(99, DateTime.Today);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        private static ReservaService CrearServicioEmpleado(
            ReservaRepositoryFake reservas,
            HorarioRepositoryFake horarios)
        {
            return new ReservaService(reservas, horarios, new UsuarioServiceFake { Sesion = CrearSesionEmpleado() });
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
