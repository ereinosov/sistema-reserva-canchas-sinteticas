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
        private const string CedulaValida = "0912345675";

        [TestMethod]
        public void Registrar_SinSesion_Falla()
        {
            ClienteService servicio = new ClienteService(new ClienteRepositoryFake(), new UsuarioServiceFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, CedulaValida, "0987654321", "ana@uteq.edu.ec"));
        }

        [TestMethod]
        public void Registrar_CedulaCorta_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado().RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "123", "0987654321", "ana@uteq.edu.ec"));
        }

        [TestMethod]
        public void Registrar_CedulaProvinciaInvalida_Falla()
        {
            ValidacionNegocioException error = Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado().RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "2501234567", "0987654321", "ana@uteq.edu.ec"));

            StringAssert.Contains(error.Message, "provincia");
        }

        [TestMethod]
        public void Registrar_CedulaTercerDigitoInvalido_Falla()
        {
            ValidacionNegocioException error = Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado().RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0960000000", "0987654321", "ana@uteq.edu.ec"));

            StringAssert.Contains(error.Message, "tercer dígito");
        }

        [TestMethod]
        public void Registrar_CedulaVerificadorInvalido_Falla()
        {
            ValidacionNegocioException error = Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado().RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, "0912345670", "0987654321", "ana@uteq.edu.ec"));

            StringAssert.Contains(error.Message, "verificador");
        }

        [TestMethod]
        public void Registrar_DatosValidos_GuardaSinEspaciosDeMas()
        {
            ClienteRepositoryFake repo = new ClienteRepositoryFake();

            int id = ServicioEmpleado(repo).RegistrarCliente(
                "  Ana Pérez  ",
                ValoresDominio.TipoDocumento.Cedula,
                " " + CedulaValida + " ",
                "0987654321",
                "  ana@uteq.edu.ec ");

            Assert.AreEqual(1, id);
            Assert.AreEqual("Ana Pérez", repo.UltimoInsertado.NombreCliente);
            Assert.AreEqual(CedulaValida, repo.UltimoInsertado.NumeroDocumentoCliente);
            Assert.AreEqual("ana@uteq.edu.ec", repo.UltimoInsertado.CorreoCliente);
        }

        [TestMethod]
        public void Registrar_DocumentoDuplicado_Falla()
        {
            ClienteRepositoryFake repo = RepoConError(CodigosSql.ClienteDuplicado);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado(repo).RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, CedulaValida, "0987654321", "ana@uteq.edu.ec"));
        }

        [TestMethod]
        public void Registrar_TelefonoDuplicado_Falla()
        {
            ClienteRepositoryFake repo = RepoConError(CodigosSql.ClienteTelefonoDuplicado);

            ValidacionNegocioException error = Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado(repo).RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, CedulaValida, "0987654321", "ana@uteq.edu.ec"));

            StringAssert.Contains(error.Message, "teléfono");
        }

        [TestMethod]
        public void Registrar_CorreoDuplicado_Falla()
        {
            ClienteRepositoryFake repo = RepoConError(CodigosSql.ClienteCorreoDuplicado);

            ValidacionNegocioException error = Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioEmpleado(repo).RegistrarCliente("Ana Pérez", ValoresDominio.TipoDocumento.Cedula, CedulaValida, "0987654321", "ana@uteq.edu.ec"));

            StringAssert.Contains(error.Message, "correo");
        }

        [TestMethod]
        public void Consultar_EscapaComodinesDelFiltro()
        {
            ClienteRepositoryFake repo = new ClienteRepositoryFake();

            ServicioEmpleado(repo).ConsultarClientes(" 0102030405 ", "Ana_Pérez%");

            Assert.AreEqual("0102030405", repo.UltimoDocumentoFiltro);
            Assert.AreEqual("Ana[_]Pérez[%]", repo.UltimoNombreFiltro);
        }

        [TestMethod]
        public void Consultar_DevuelveLaLista()
        {
            ClienteRepositoryFake repo = new ClienteRepositoryFake();
            repo.Clientes.Add(new Cliente
            {
                IdCliente = 1,
                NombreCliente = "Ana Pérez",
                TipoDocumentoCliente = ValoresDominio.TipoDocumento.Cedula,
                NumeroDocumentoCliente = "0102030405"
            });

            IList<Cliente> resultado = ServicioEmpleado(repo).ConsultarClientes(null, null);

            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod]
        public void Modificar_ActualizaLosDatos()
        {
            ClienteRepositoryFake repo = new ClienteRepositoryFake();

            ServicioEmpleado(repo).ModificarCliente(
                4,
                "  Carlos Mora  ",
                ValoresDominio.TipoDocumento.Pasaporte,
                "A1234567",
                "+593987654321",
                "carlos@uteq.edu.ec");

            Assert.AreEqual(4, repo.UltimoActualizado.IdCliente);
            Assert.AreEqual("Carlos Mora", repo.UltimoActualizado.NombreCliente);
        }

        [TestMethod]
        public void Eliminar_Empleado_NoPuede()
        {
            Assert.ThrowsException<OperacionNoPermitidaException>(
                () => ServicioEmpleado().EliminarCliente(1));
        }

        [TestMethod]
        public void Eliminar_ConReservasActivas_Falla()
        {
            ClienteRepositoryFake repo = RepoConError(CodigosSql.ClienteConReservasActivas);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin(repo).EliminarCliente(8));
        }

        [TestMethod]
        public void Eliminar_ConPagosPendientes_Falla()
        {
            ClienteRepositoryFake repo = RepoConError(CodigosSql.ClienteConPagosPendientes);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioAdmin(repo).EliminarCliente(8));
        }

        [TestMethod]
        public void Eliminar_Administrador_Elimina()
        {
            ClienteRepositoryFake repo = new ClienteRepositoryFake();

            ServicioAdmin(repo).EliminarCliente(8);

            Assert.AreEqual(8, repo.IdEliminado);
        }

        private static ClienteRepositoryFake RepoConError(int codigo)
        {
            return new ClienteRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("error", codigo)
            };
        }

        private static ClienteService ServicioEmpleado()
        {
            return ServicioEmpleado(new ClienteRepositoryFake());
        }

        private static ClienteService ServicioEmpleado(ClienteRepositoryFake repo)
        {
            return new ClienteService(repo, new UsuarioServiceFake { Sesion = SesionPrueba.Empleado() });
        }

        private static ClienteService ServicioAdmin(ClienteRepositoryFake repo)
        {
            return new ClienteService(repo, new UsuarioServiceFake { Sesion = SesionPrueba.Admin() });
        }
    }
}
