using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class IngresoServiceTests
    {
        [TestMethod]
        public void ConsultarIngresos_SinSesion_LanzaSesionNoIniciada()
        {
            IngresoService servicio = new IngresoService(new IngresoRepositoryFake(), new UsuarioServiceFake());

            try
            {
                servicio.ConsultarIngresos(DateTime.Today, DateTime.Today);
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void ConsultarIngresos_Empleado_LanzaOperacionNoPermitida()
        {
            IngresoService servicio = new IngresoService(
                new IngresoRepositoryFake(),
                new UsuarioServiceFake { Sesion = CrearSesionEmpleado() });

            try
            {
                servicio.ConsultarIngresos(DateTime.Today, DateTime.Today);
                Assert.Fail("Debió lanzar OperacionNoPermitidaException.");
            }
            catch (OperacionNoPermitidaException)
            {
            }
        }

        [TestMethod]
        public void ConsultarIngresos_RangoInvertido_LanzaValidacion()
        {
            IngresoService servicio = CrearServicioAdmin(new IngresoRepositoryFake());

            try
            {
                servicio.ConsultarIngresos(new DateTime(2026, 8, 24), new DateTime(2026, 8, 1));
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ConsultarIngresos_Administrador_DevuelveTotalYDetalle()
        {
            IngresoRepositoryFake repositorio = new IngresoRepositoryFake();
            repositorio.Resultado.TotalIngresos = 50.00m;
            repositorio.Resultado.Detalle.Add(new Ingreso
            {
                IdPago = 1,
                IdReserva = 3,
                MontoPago = 50.00m
            });
            IngresoService servicio = CrearServicioAdmin(repositorio);
            DateTime inicio = new DateTime(2026, 8, 1, 10, 0, 0);
            DateTime fin = new DateTime(2026, 8, 24, 18, 0, 0);

            ConsultaIngresos consulta = servicio.ConsultarIngresos(inicio, fin);

            Assert.AreEqual(50.00m, consulta.TotalIngresos);
            Assert.AreEqual(1, consulta.Detalle.Count);
            Assert.AreEqual(new DateTime(2026, 8, 1), repositorio.UltimaFechaInicio);
            Assert.AreEqual(new DateTime(2026, 8, 24), repositorio.UltimaFechaFin);
        }

        private static IngresoService CrearServicioAdmin(IngresoRepositoryFake repositorio)
        {
            return new IngresoService(repositorio, new UsuarioServiceFake { Sesion = CrearSesionAdmin() });
        }

        private static Usuario CrearSesionAdmin()
        {
            return new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "John",
                UsuarioLogin = "admin",
                NombreRol = ValoresDominio.Rol.Administrador,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo
            };
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
