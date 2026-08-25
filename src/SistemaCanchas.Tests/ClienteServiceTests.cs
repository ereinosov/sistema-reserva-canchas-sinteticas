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
    public class ClienteServiceTests
    {
        [TestMethod]
        public void RegistrarCliente_SinSesion_LanzaSesionNoIniciada()
        {
            ClienteService servicio = new ClienteService(new ClienteRepositoryFake(), new UsuarioServiceFake());

            try
            {
                servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0102030405", "0987654321", "ana@uteq.edu.ec");
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCliente_CedulaInvalida_LanzaValidacion()
        {
            ClienteService servicio = CrearServicioEmpleado(new ClienteRepositoryFake());

            try
            {
                servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "123", "0987654321", "ana@uteq.edu.ec");
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCliente_DatosValidos_PersisteRecortado()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake();
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            int id = servicio.RegistrarCliente(
                "  Ana Pérez  ",
                ValoresDominio.TipoDocumento.Cedula,
                " 0102030405 ",
                "0987654321",
                "  ana@uteq.edu.ec ");

            Assert.AreEqual(1, id);
            Assert.AreEqual("Ana Pérez", repositorio.UltimoInsertado.NombreCliente);
            Assert.AreEqual("0102030405", repositorio.UltimoInsertado.NumeroDocumentoCliente);
            Assert.AreEqual("ana@uteq.edu.ec", repositorio.UltimoInsertado.CorreoCliente);
        }

        [TestMethod]
        public void RegistrarCliente_DocumentoDuplicado_LanzaValidacion()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicado", CodigosSql.ClienteDuplicado)
            };
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            try
            {
                servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0102030405", "0987654321", "ana@uteq.edu.ec");
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarCliente_TelefonoDuplicado_LanzaValidacion()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicado", CodigosSql.ClienteTelefonoDuplicado)
            };
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            try
            {
                servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0102030405", "0987654321", "ana@uteq.edu.ec");
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException ex)
            {
                StringAssert.Contains(ex.Message, "teléfono");
            }
        }

        [TestMethod]
        public void RegistrarCliente_CorreoDuplicado_LanzaValidacion()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("duplicado", CodigosSql.ClienteCorreoDuplicado)
            };
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            try
            {
                servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0102030405", "0987654321", "ana@uteq.edu.ec");
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException ex)
            {
                StringAssert.Contains(ex.Message, "correo");
            }
        }

        [TestMethod]
        public void ConsultarClientes_EscapaComodinesLike()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake();
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            servicio.ConsultarClientes(" 0102030405 ", "Ana_Pérez%");

            Assert.AreEqual("0102030405", repositorio.UltimoDocumentoFiltro);
            Assert.AreEqual("Ana[_]Pérez[%]", repositorio.UltimoNombreFiltro);
        }

        [TestMethod]
        public void ModificarCliente_ActualizaDatos()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake();
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            servicio.ModificarCliente(
                4,
                "  Carlos Mora  ",
                ValoresDominio.TipoDocumento.Pasaporte,
                "A1234567",
                "+593987654321",
                "carlos@uteq.edu.ec");

            Assert.AreEqual(4, repositorio.UltimoActualizado.IdCliente);
            Assert.AreEqual("Carlos Mora", repositorio.UltimoActualizado.NombreCliente);
            Assert.AreEqual(ValoresDominio.TipoDocumento.Pasaporte, repositorio.UltimoActualizado.TipoDocumentoCliente);
        }

        [TestMethod]
        public void EliminarCliente_Empleado_LanzaOperacionNoPermitida()
        {
            ClienteService servicio = CrearServicioEmpleado(new ClienteRepositoryFake());

            try
            {
                servicio.EliminarCliente(1);
                Assert.Fail("Debió lanzar OperacionNoPermitidaException.");
            }
            catch (OperacionNoPermitidaException)
            {
            }
        }

        [TestMethod]
        public void EliminarCliente_ConReservasActivas_LanzaValidacion()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("reservas", CodigosSql.ClienteConReservasActivas)
            };
            ClienteService servicio = CrearServicioAdmin(repositorio);

            try
            {
                servicio.EliminarCliente(8);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void EliminarCliente_ConPagosPendientes_LanzaValidacion()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("pagos", CodigosSql.ClienteConPagosPendientes)
            };
            ClienteService servicio = CrearServicioAdmin(repositorio);

            try
            {
                servicio.EliminarCliente(8);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void EliminarCliente_Administrador_Elimina()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake();
            ClienteService servicio = CrearServicioAdmin(repositorio);

            servicio.EliminarCliente(8);

            Assert.AreEqual(8, repositorio.IdEliminado);
        }

        [TestMethod]
        public void ConsultarClientes_DevuelveLista()
        {
            ClienteRepositoryFake repositorio = new ClienteRepositoryFake();
            repositorio.Clientes.Add(new Cliente
            {
                IdCliente = 1,
                NombreCliente = "Ana Pérez",
                TipoDocumentoCliente = ValoresDominio.TipoDocumento.Cedula,
                NumeroDocumentoCliente = "0102030405"
            });
            ClienteService servicio = CrearServicioEmpleado(repositorio);

            IList<Cliente> resultado = servicio.ConsultarClientes(null, null);

            Assert.AreEqual(1, resultado.Count);
            Assert.IsNull(repositorio.UltimoDocumentoFiltro);
            Assert.IsNull(repositorio.UltimoNombreFiltro);
        }

        private static ClienteService CrearServicioAdmin(ClienteRepositoryFake repositorio)
        {
            return CrearServicio(CrearSesionAdmin(), repositorio);
        }

        private static ClienteService CrearServicioEmpleado(ClienteRepositoryFake repositorio)
        {
            return CrearServicio(CrearSesionEmpleado(), repositorio);
        }

        private static ClienteService CrearServicio(Usuario sesion, ClienteRepositoryFake repositorio)
        {
            return new ClienteService(repositorio, new UsuarioServiceFake { Sesion = sesion });
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
