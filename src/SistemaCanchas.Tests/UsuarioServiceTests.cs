using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class UsuarioServiceTests
    {
        private const string ClaveOk = "Clave.App#2026";
        private const int CostoBcryptPrueba = 4;

        [TestMethod]
        public void Login_UsuarioOClaveVacios_Falla()
        {
            UsuarioService servicio = Servicio(new UsuarioRepositoryFake(), new GestorConexionFake());

            Assert.ThrowsException<CredencialesInvalidasException>(
                () => servicio.ValidarCredenciales("   ", ClaveOk));
            Assert.ThrowsException<CredencialesInvalidasException>(
                () => servicio.ValidarCredenciales("admin", string.Empty));
        }

        [TestMethod]
        public void Login_UsuarioInexistente_Falla()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = null };
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());

            Assert.ThrowsException<CredencialesInvalidasException>(
                () => servicio.ValidarCredenciales("nadie", ClaveOk));
            Assert.AreEqual("nadie", repo.UltimoLoginConsultado);
        }

        [TestMethod]
        public void Login_UsuarioInactivo_Falla()
        {
            Usuario usuario = UsuarioGuardado();
            usuario.EstadoUsuario = ValoresDominio.EstadoUsuario.Inactivo;
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = usuario },
                new GestorConexionFake());

            Assert.ThrowsException<UsuarioInactivoException>(
                () => servicio.ValidarCredenciales("admin", ClaveOk));
        }

        [TestMethod]
        public void Login_ClaveIncorrecta_Falla()
        {
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() },
                new GestorConexionFake());

            Assert.ThrowsException<CredencialesInvalidasException>(
                () => servicio.ValidarCredenciales("admin", "clave-incorrecta"));
        }

        [TestMethod]
        public void Login_HashRoto_Falla()
        {
            Usuario usuario = UsuarioGuardado();
            usuario.ClaveAppHash = "esto-no-es-bcrypt";
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = usuario },
                new GestorConexionFake());

            Assert.ThrowsException<CredencialesInvalidasException>(
                () => servicio.ValidarCredenciales("admin", ClaveOk));
        }

        [TestMethod]
        public void Login_Correcto_AbreSesionYOcultaSecretos()
        {
            GestorConexionFake gestor = new GestorConexionFake { ClaveDescifrada = "Sql#Motor" };
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() },
                gestor);

            Usuario sesion = servicio.ValidarCredenciales("  admin  ", ClaveOk);

            Assert.AreEqual(1, sesion.IdUsuario);
            Assert.AreEqual("Administrador", sesion.NombreUsuario);
            Assert.AreEqual("admin", sesion.UsuarioLogin);
            Assert.AreEqual(ValoresDominio.Rol.Administrador, sesion.NombreRol);
            Assert.IsNull(sesion.ClaveAppHash);
            Assert.IsNull(sesion.UsuarioBd);
            Assert.IsNull(sesion.ClaveBdEnc);
            Assert.IsTrue(gestor.SesionActiva);
            Assert.AreEqual("login_admin", gestor.UsuarioBdAsignado);
            Assert.AreEqual("Sql#Motor", gestor.ClaveBdAsignada);
        }

        [TestMethod]
        public void Login_FalloDeCifrado_Falla()
        {
            GestorConexionFake gestor = new GestorConexionFake
            {
                ErrorDescifrado = new ErrorCifradoException("clave AES distinta")
            };
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() },
                gestor);

            Assert.ThrowsException<ErrorInfraestructuraException>(
                () => servicio.ValidarCredenciales("admin", ClaveOk));
        }

        [TestMethod]
        public void Login_ErrorDeBase_Falla()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("caída del motor")
            };

            Assert.ThrowsException<ErrorInfraestructuraException>(
                () => Servicio(repo, new GestorConexionFake()).ValidarCredenciales("admin", ClaveOk));
        }

        [TestMethod]
        public void CerrarSesion_LimpiaAplicacionYMotor()
        {
            GestorConexionFake gestor = new GestorConexionFake();
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() },
                gestor);

            servicio.ValidarCredenciales("admin", ClaveOk);
            servicio.CerrarSesion();

            Assert.IsFalse(gestor.SesionActiva);
            Assert.AreEqual(1, gestor.VecesCerrarSesion);
            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.ObtenerSesionActual());
        }

        [TestMethod]
        public void Registrar_SinSesion_Falla()
        {
            UsuarioService servicio = Servicio(new UsuarioRepositoryFake(), new GestorConexionFake());

            Assert.ThrowsException<SesionNoIniciadaException>(
                () => servicio.RegistrarUsuario("Ana", "ana", ClaveOk, ValoresDominio.Rol.Empleado));
        }

        [TestMethod]
        public void Registrar_LoginInvalido_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioConAdmin().RegistrarUsuario("Ana", "1invalido", ClaveOk, ValoresDominio.Rol.Empleado));
        }

        [TestMethod]
        public void Registrar_ClaveCorta_Falla()
        {
            Assert.ThrowsException<ValidacionNegocioException>(
                () => ServicioConAdmin().RegistrarUsuario("Ana", "ana", "123", ValoresDominio.Rol.Empleado));
        }

        [TestMethod]
        public void Registrar_DatosValidos_GuardaHashYCifraClaveDeMotor()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() };
            GestorConexionFake gestor = new GestorConexionFake();
            UsuarioService servicio = Servicio(repo, gestor);
            servicio.ValidarCredenciales("admin", ClaveOk);

            int id = servicio.RegistrarUsuario("Ana López", "ana", ClaveOk, ValoresDominio.Rol.Empleado);

            Assert.AreEqual(10, id);
            Assert.IsFalse(repo.InsertoDesdeInstalacion);
            Assert.AreEqual("Ana López", repo.UltimoInsertado.NombreUsuario);
            Assert.AreEqual("ana", repo.UltimoInsertado.UsuarioLogin);
            Assert.AreEqual("u_ana", repo.UltimoInsertado.UsuarioBd);
            Assert.AreEqual(ValoresDominio.Rol.Empleado, repo.UltimoRolInsertado);
            Assert.IsTrue(repo.UltimoInsertado.ClaveBdEnc.StartsWith("enc:"));
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(ClaveOk, repo.UltimoInsertado.ClaveAppHash));
        }

        [TestMethod]
        public void AdminInicial_SiNoHayUsuarios_UsaConexionDeInstalacion()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake();
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());

            int id = servicio.RegistrarAdministradorInicial("Admin", "admin", ClaveOk);

            Assert.AreEqual(10, id);
            Assert.IsTrue(repo.InsertoDesdeInstalacion);
            Assert.AreEqual(ValoresDominio.Rol.Administrador, repo.UltimoRolInsertado);
            Assert.AreEqual("u_admin", repo.UltimoInsertado.UsuarioBd);
        }

        [TestMethod]
        public void AdminInicial_SiYaHayUsuarios_Falla()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake();
            repo.UsuariosExistentes.Add(UsuarioGuardado());
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());

            Assert.ThrowsException<ValidacionNegocioException>(
                () => servicio.RegistrarAdministradorInicial("Admin", "admin", ClaveOk));
        }

        [TestMethod]
        public void Desactivar_ASiMismo_NoPuede()
        {
            Assert.ThrowsException<OperacionNoPermitidaException>(
                () => ServicioConAdmin().DesactivarUsuario(1));
        }

        [TestMethod]
        public void Desactivar_UsuarioQueNoEstaEnLaLista_Falla()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() };
            repo.UsuariosExistentes.Add(UsuarioGuardado());
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);

            Assert.ThrowsException<ValidacionNegocioException>(
                () => servicio.DesactivarUsuario(99));
        }

        [TestMethod]
        public void Desactivar_Empleado_Desactiva()
        {
            UsuarioRepositoryFake repo = RepoConAdminYEmpleado(ValoresDominio.EstadoUsuario.Activo);
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);

            servicio.DesactivarUsuario(2);

            Assert.AreEqual(2, repo.IdDesactivado);
        }

        [TestMethod]
        public void Activar_Inactivo_Activa()
        {
            UsuarioRepositoryFake repo = RepoConAdminYEmpleado(ValoresDominio.EstadoUsuario.Inactivo);
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);

            servicio.ActivarUsuario(2);

            Assert.AreEqual(2, repo.IdActivado);
        }

        [TestMethod]
        public void CambiarClave_GuardaHashBcrypt()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() };
            repo.UsuariosExistentes.Add(UsuarioGuardado());
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);

            servicio.CambiarClaveUsuario(1, "Nueva.Clave#2026");

            Assert.AreEqual(1, repo.IdClaveCambiada);
            Assert.IsTrue(repo.UltimoHashClave.StartsWith("$2", StringComparison.Ordinal));
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("Nueva.Clave#2026", repo.UltimoHashClave));
        }

        [TestMethod]
        public void ActualizarNombre_GuardaNombreRecortado()
        {
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() };
            repo.UsuariosExistentes.Add(UsuarioGuardado());
            UsuarioService servicio = Servicio(repo, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);

            servicio.ActualizarNombreUsuario(1, "  Admin Actualizado  ");

            Assert.AreEqual(1, repo.IdNombreActualizado);
            Assert.AreEqual("Admin Actualizado", repo.UltimoNombreActualizado);
        }

        [TestMethod]
        public void ExisteAlgunUsuario_ListaVacia_EsFalso()
        {
            Assert.IsFalse(Servicio(new UsuarioRepositoryFake(), new GestorConexionFake()).ExisteAlgunUsuario());
        }

        [TestMethod]
        public void IdentificadorSql_ArmaLoginDeMotor()
        {
            Assert.AreEqual("u_admin", IdentificadorSql.DesdeLogin("Admin"));
            Assert.AreEqual("u_ana_1", IdentificadorSql.DesdeLogin("ana_1"));
        }

        [TestMethod]
        public void GeneradorClaveMotor_TieneLongitudYTiposDeCaracter()
        {
            string clave = GeneradorClaveMotor.Generar();
            Assert.AreEqual(24, clave.Length);
            Assert.IsTrue(TieneAlguno(clave, "ABCDEFGHJKLMNPQRSTUVWXYZ"));
            Assert.IsTrue(TieneAlguno(clave, "abcdefghijkmnopqrstuvwxyz"));
            Assert.IsTrue(TieneAlguno(clave, "23456789"));
            Assert.IsTrue(TieneAlguno(clave, "#@%*-_!"));
        }

        private static bool TieneAlguno(string texto, string alfabeto)
        {
            for (int i = 0; i < texto.Length; i++)
            {
                if (alfabeto.IndexOf(texto[i]) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static UsuarioRepositoryFake RepoConAdminYEmpleado(string estadoEmpleado)
        {
            Usuario empleado = new Usuario
            {
                IdUsuario = 2,
                NombreUsuario = "Ana",
                UsuarioLogin = "ana",
                NombreRol = ValoresDominio.Rol.Empleado,
                EstadoUsuario = estadoEmpleado
            };
            UsuarioRepositoryFake repo = new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() };
            repo.UsuariosExistentes.Add(UsuarioGuardado());
            repo.UsuariosExistentes.Add(empleado);
            return repo;
        }

        private static UsuarioService ServicioConAdmin()
        {
            UsuarioService servicio = Servicio(
                new UsuarioRepositoryFake { UsuarioADevolver = UsuarioGuardado() },
                new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveOk);
            return servicio;
        }

        private static UsuarioService Servicio(UsuarioRepositoryFake repo, GestorConexionFake gestor)
        {
            return new UsuarioService(repo, gestor);
        }

        private static Usuario UsuarioGuardado()
        {
            return new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "Administrador",
                UsuarioLogin = "admin",
                ClaveAppHash = BCrypt.Net.BCrypt.HashPassword(ClaveOk, CostoBcryptPrueba),
                UsuarioBd = "login_admin",
                ClaveBdEnc = "valor-cifrado",
                NombreRol = ValoresDominio.Rol.Administrador,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo
            };
        }
    }
}
