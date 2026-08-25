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
        private static readonly TimeSpan HoraInicio = TimeSpan.FromHours(ValoresDominio.HoraInicioFranja);
        private static readonly TimeSpan HoraFin = TimeSpan.FromHours(ValoresDominio.HoraFinOperacion);
        [TestMethod]
        public void RegistrarCancha_SinSesion_LanzaSesionNoIniciada()
        {
            CanchaService servicio = new CanchaService(new CanchaRepositoryFake(), new UsuarioServiceFake());

            try
            {
                servicio.RegistrarCancha("Cancha 1", HoraInicio, HoraFin);
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCancha_Empleado_LanzaOperacionNoPermitida()
        {
            CanchaService servicio = CrearServicio(CrearSesionEmpleado(), new CanchaRepositoryFake());

            try
            {
                servicio.RegistrarCancha("Cancha 1", HoraInicio, HoraFin);
                Assert.Fail("Debió lanzar OperacionNoPermitidaException.");
            }
            catch (OperacionNoPermitidaException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCancha_NombreVacio_LanzaValidacion()
        {
            CanchaService servicio = CrearServicioAdmin(new CanchaRepositoryFake());

            try
            {
                servicio.RegistrarCancha("   ", HoraInicio, HoraFin);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCancha_NombreValido_PersisteRecortado()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            CanchaService servicio = CrearServicioAdmin(repositorio);

            int id = servicio.RegistrarCancha("  Cancha Norte  ", HoraInicio, HoraFin);

            Assert.AreEqual(1, id);
            Assert.AreEqual("Cancha Norte", repositorio.UltimaInsertada.NombreCancha);
            Assert.AreEqual(ValoresDominio.EstadoCancha.Activa, repositorio.UltimaInsertada.EstadoCancha);
            Assert.AreEqual(HoraInicio, repositorio.UltimaInsertada.HoraInicioOperacion);
            Assert.AreEqual(HoraFin, repositorio.UltimaInsertada.HoraFinOperacion);
        }

        [TestMethod]
        public void RegistrarCancha_NombreDuplicado_LanzaValidacion()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicada", CodigosSql.CanchaDuplicada)
            };
            CanchaService servicio = CrearServicioAdmin(repositorio);

            try
            {
                servicio.RegistrarCancha("Cancha 1", HoraInicio, HoraFin);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void DesactivarCancha_YaInactiva_LanzaValidacion()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 3,
                NombreCancha = "Sur",
                EstadoCancha = ValoresDominio.EstadoCancha.Inactiva
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            try
            {
                servicio.DesactivarCancha(3);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void DesactivarCancha_Activa_Desactiva()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 2,
                NombreCancha = "Norte",
                EstadoCancha = ValoresDominio.EstadoCancha.Activa
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            servicio.DesactivarCancha(2);

            Assert.AreEqual(2, repositorio.IdDesactivado);
        }

        [TestMethod]
        public void ActivarCancha_YaActiva_LanzaValidacion()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 2,
                NombreCancha = "Norte",
                EstadoCancha = ValoresDominio.EstadoCancha.Activa
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            try
            {
                servicio.ActivarCancha(2);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ActivarCancha_Inactiva_Activa()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 3,
                NombreCancha = "Sur",
                EstadoCancha = ValoresDominio.EstadoCancha.Inactiva
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            servicio.ActivarCancha(3);

            Assert.AreEqual(3, repositorio.IdActivado);
        }

        [TestMethod]
        public void CanchaActiva_SegunEstado()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 1,
                NombreCancha = "Norte",
                EstadoCancha = ValoresDominio.EstadoCancha.Activa
            });
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 2,
                NombreCancha = "Sur",
                EstadoCancha = ValoresDominio.EstadoCancha.Inactiva
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            Assert.IsTrue(servicio.CanchaActiva(1));
            Assert.IsFalse(servicio.CanchaActiva(2));
            Assert.IsFalse(servicio.CanchaActiva(99));
        }

        [TestMethod]
        public void ModificarCancha_ActualizaNombre()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            CanchaService servicio = CrearServicioAdmin(repositorio);

            servicio.ModificarCancha(4, "  Cancha Central  ", HoraInicio, HoraFin);

            Assert.AreEqual(4, repositorio.UltimaActualizada.IdCancha);
            Assert.AreEqual("Cancha Central", repositorio.UltimaActualizada.NombreCancha);
            Assert.AreEqual(HoraInicio, repositorio.UltimaActualizada.HoraInicioOperacion);
            Assert.AreEqual(HoraFin, repositorio.UltimaActualizada.HoraFinOperacion);
        }

        [TestMethod]
        public void RegistrarCancha_HorarioInvertido_LanzaValidacion()
        {
            CanchaService servicio = CrearServicioAdmin(new CanchaRepositoryFake());

            try
            {
                servicio.RegistrarCancha("Cancha 1", HoraFin, HoraInicio);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void ObtenerActivas_FiltraInactivas()
        {
            CanchaRepositoryFake repositorio = new CanchaRepositoryFake();
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 1,
                NombreCancha = "Norte",
                EstadoCancha = ValoresDominio.EstadoCancha.Activa
            });
            repositorio.Canchas.Add(new Cancha
            {
                IdCancha = 2,
                NombreCancha = "Sur",
                EstadoCancha = ValoresDominio.EstadoCancha.Inactiva
            });
            CanchaService servicio = CrearServicioAdmin(repositorio);

            Assert.AreEqual(1, servicio.ObtenerActivas().Count);
            Assert.AreEqual(2, servicio.ObtenerTodas().Count);
        }

        private static CanchaService CrearServicioAdmin(CanchaRepositoryFake repositorio)
        {
            return CrearServicio(CrearSesionAdmin(), repositorio);
        }

        private static CanchaService CrearServicio(Usuario sesion, CanchaRepositoryFake repositorio)
        {
            return new CanchaService(repositorio, new UsuarioServiceFake { Sesion = sesion });
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
