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
        private const string ClaveAppValida = "Clave.App#2026";
        private const int CostoBcryptPrueba = 4;

        [TestMethod]
        public void ValidarCredenciales_UsuarioOClaveVacios_LanzaCredencialesInvalidas()
        {
            UsuarioService servicio = CrearServicio(new UsuarioRepositoryFake(), new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("   ", ClaveAppValida);
                Assert.Fail("Debió lanzar CredencialesInvalidasException.");
            }
            catch (CredencialesInvalidasException)
            {
            }

            try
            {
                servicio.ValidarCredenciales("admin", string.Empty);
                Assert.Fail("Debió lanzar CredencialesInvalidasException.");
            }
            catch (CredencialesInvalidasException)
            {
            }
        }

        [TestMethod]
        public void ValidarCredenciales_UsuarioInexistente_LanzaCredencialesInvalidas()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake { UsuarioADevolver = null };
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("nadie", ClaveAppValida);
                Assert.Fail("Debió lanzar CredencialesInvalidasException.");
            }
            catch (CredencialesInvalidasException)
            {
            }

            Assert.AreEqual("nadie", repositorio.UltimoLoginConsultado);
        }

        [TestMethod]
        public void ValidarCredenciales_UsuarioInactivo_LanzaUsuarioInactivo()
        {
            Usuario usuario = CrearUsuarioPersistido();
            usuario.EstadoUsuario = ValoresDominio.EstadoUsuario.Inactivo;
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = usuario },
                new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("admin", ClaveAppValida);
                Assert.Fail("Debió lanzar UsuarioInactivoException.");
            }
            catch (UsuarioInactivoException)
            {
            }
        }

        [TestMethod]
        public void ValidarCredenciales_ClaveIncorrecta_LanzaCredencialesInvalidas()
        {
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = CrearUsuarioPersistido() },
                new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("admin", "clave-incorrecta");
                Assert.Fail("Debió lanzar CredencialesInvalidasException.");
            }
            catch (CredencialesInvalidasException)
            {
            }
        }

        [TestMethod]
        public void ValidarCredenciales_HashMalFormado_LanzaCredencialesInvalidas()
        {
            Usuario usuario = CrearUsuarioPersistido();
            usuario.ClaveAppHash = "esto-no-es-bcrypt";
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = usuario },
                new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("admin", ClaveAppValida);
                Assert.Fail("Debió lanzar CredencialesInvalidasException.");
            }
            catch (CredencialesInvalidasException)
            {
            }
        }

        [TestMethod]
        public void ValidarCredenciales_CredencialesCorrectas_AbreSesionYOcultaSecretos()
        {
            GestorConexionFake gestor = new GestorConexionFake { ClaveDescifrada = "Sql#Motor" };
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = CrearUsuarioPersistido() },
                gestor);

            Usuario sesion = servicio.ValidarCredenciales("  admin  ", ClaveAppValida);

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
            Assert.AreEqual("Administrador", servicio.ObtenerSesionActual().NombreUsuario);
        }

        [TestMethod]
        public void ValidarCredenciales_FalloDeCifrado_LanzaErrorInfraestructura()
        {
            GestorConexionFake gestor = new GestorConexionFake
            {
                ErrorDescifrado = new ErrorCifradoException("clave AES distinta")
            };
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = CrearUsuarioPersistido() },
                gestor);

            try
            {
                servicio.ValidarCredenciales("admin", ClaveAppValida);
                Assert.Fail("Debió lanzar ErrorInfraestructuraException.");
            }
            catch (ErrorInfraestructuraException)
            {
            }
        }

        [TestMethod]
        public void CerrarSesion_LimpiaSesionDeAplicacionYDeMotor()
        {
            GestorConexionFake gestor = new GestorConexionFake();
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = CrearUsuarioPersistido() },
                gestor);

            servicio.ValidarCredenciales("admin", ClaveAppValida);
            servicio.CerrarSesion();

            Assert.IsFalse(gestor.SesionActiva);
            Assert.AreEqual(1, gestor.VecesCerrarSesion);

            try
            {
                servicio.ObtenerSesionActual();
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void ValidarCredenciales_ErrorDeBaseDeDatos_LanzaErrorInfraestructura()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake
            {
                ExcepcionALanzar = new ErrorAccesoDatosException("caída del motor")
            };
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());

            try
            {
                servicio.ValidarCredenciales("admin", ClaveAppValida);
                Assert.Fail("Debió lanzar ErrorInfraestructuraException.");
            }
            catch (ErrorInfraestructuraException)
            {
            }
        }

        [TestMethod]
        public void RegistrarUsuario_SinSesion_LanzaSesionNoIniciada()
        {
            UsuarioService servicio = CrearServicio(new UsuarioRepositoryFake(), new GestorConexionFake());

            try
            {
                servicio.RegistrarUsuario("Ana", "ana", ClaveAppValida, ValoresDominio.Rol.Empleado);
                Assert.Fail("Debió lanzar SesionNoIniciadaException.");
            }
            catch (SesionNoIniciadaException)
            {
            }
        }

        [TestMethod]
        public void RegistrarUsuario_LoginInvalido_LanzaValidacion()
        {
            UsuarioService servicio = CrearServicioConSesionAdmin();

            try
            {
                servicio.RegistrarUsuario("Ana", "1invalido", ClaveAppValida, ValoresDominio.Rol.Empleado);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarUsuario_ClaveCorta_LanzaValidacion()
        {
            UsuarioService servicio = CrearServicioConSesionAdmin();

            try
            {
                servicio.RegistrarUsuario("Ana", "ana", "123", ValoresDominio.Rol.Empleado);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void RegistrarUsuario_DatosValidos_PersisteHashYCifraClaveDeMotor()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake
            {
                UsuarioADevolver = CrearUsuarioPersistido()
            };
            GestorConexionFake gestor = new GestorConexionFake();
            UsuarioService servicio = CrearServicio(repositorio, gestor);
            servicio.ValidarCredenciales("admin", ClaveAppValida);

            int id = servicio.RegistrarUsuario("Ana López", "ana", ClaveAppValida, ValoresDominio.Rol.Empleado);

            Assert.AreEqual(10, id);
            Assert.IsFalse(repositorio.InsertoDesdeInstalacion);
            Assert.AreEqual("Ana López", repositorio.UltimoInsertado.NombreUsuario);
            Assert.AreEqual("ana", repositorio.UltimoInsertado.UsuarioLogin);
            Assert.AreEqual("u_ana", repositorio.UltimoInsertado.UsuarioBd);
            Assert.AreEqual(ValoresDominio.Rol.Empleado, repositorio.UltimoRolInsertado);
            Assert.IsTrue(repositorio.UltimoInsertado.ClaveBdEnc.StartsWith("enc:"));
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(ClaveAppValida, repositorio.UltimoInsertado.ClaveAppHash));
            Assert.IsFalse(string.IsNullOrEmpty(repositorio.UltimaClaveBdPlana));
        }

        [TestMethod]
        public void RegistrarAdministradorInicial_CuandoNoHayUsuarios_UsaConexionDeInstalacion()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake();
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());

            int id = servicio.RegistrarAdministradorInicial("Admin", "admin", ClaveAppValida);

            Assert.AreEqual(10, id);
            Assert.IsTrue(repositorio.InsertoDesdeInstalacion);
            Assert.AreEqual(ValoresDominio.Rol.Administrador, repositorio.UltimoRolInsertado);
            Assert.AreEqual("u_admin", repositorio.UltimoInsertado.UsuarioBd);
        }

        [TestMethod]
        public void RegistrarAdministradorInicial_SiYaHayUsuarios_LanzaValidacion()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake();
            repositorio.UsuariosExistentes.Add(CrearUsuarioPersistido());
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());

            try
            {
                servicio.RegistrarAdministradorInicial("Admin", "admin", ClaveAppValida);
                Assert.Fail("Debió lanzar ValidacionNegocioException.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void DesactivarUsuario_ASiMismo_LanzaOperacionNoPermitida()
        {
            UsuarioService servicio = CrearServicioConSesionAdmin();

            try
            {
                servicio.DesactivarUsuario(1);
                Assert.Fail("Debió lanzar OperacionNoPermitidaException.");
            }
            catch (OperacionNoPermitidaException)
            {
            }
        }

        [TestMethod]
        public void DesactivarUsuario_UnicoAdministrador_LanzaOperacionNoPermitida()
        {
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake
            {
                UsuarioADevolver = CrearUsuarioPersistido()
            };
            repositorio.UsuariosExistentes.Add(CrearUsuarioPersistido());
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveAppValida);

            try
            {
                servicio.DesactivarUsuario(99);
                Assert.Fail("Debió lanzar ValidacionNegocioException porque el id 99 no está en la lista.");
            }
            catch (ValidacionNegocioException)
            {
            }
        }

        [TestMethod]
        public void DesactivarUsuario_Empleado_Desactiva()
        {
            Usuario empleado = new Usuario
            {
                IdUsuario = 2,
                NombreUsuario = "Ana",
                UsuarioLogin = "ana",
                NombreRol = ValoresDominio.Rol.Empleado,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo
            };
            UsuarioRepositoryFake repositorio = new UsuarioRepositoryFake
            {
                UsuarioADevolver = CrearUsuarioPersistido()
            };
            repositorio.UsuariosExistentes.Add(CrearUsuarioPersistido());
            repositorio.UsuariosExistentes.Add(empleado);
            UsuarioService servicio = CrearServicio(repositorio, new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveAppValida);

            servicio.DesactivarUsuario(2);

            Assert.AreEqual(2, repositorio.IdDesactivado);
        }

        [TestMethod]
        public void IdentificadorSql_NormalizaLogin()
        {
            Assert.AreEqual("u_admin", IdentificadorSql.DesdeLogin("Admin"));
            Assert.AreEqual("u_ana_1", IdentificadorSql.DesdeLogin("ana_1"));
        }

        [TestMethod]
        public void GeneradorClaveMotor_CumpleLongitudYDiversidad()
        {
            string clave = GeneradorClaveMotor.Generar();
            Assert.AreEqual(24, clave.Length);
            Assert.IsTrue(ContieneCategoria(clave, "ABCDEFGHJKLMNPQRSTUVWXYZ"));
            Assert.IsTrue(ContieneCategoria(clave, "abcdefghijkmnopqrstuvwxyz"));
            Assert.IsTrue(ContieneCategoria(clave, "23456789"));
            Assert.IsTrue(ContieneCategoria(clave, "#@%*-_!"));
        }

        private static bool ContieneCategoria(string texto, string alfabeto)
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

        private static UsuarioService CrearServicioConSesionAdmin()
        {
            UsuarioService servicio = CrearServicio(
                new UsuarioRepositoryFake { UsuarioADevolver = CrearUsuarioPersistido() },
                new GestorConexionFake());
            servicio.ValidarCredenciales("admin", ClaveAppValida);
            return servicio;
        }

        private static UsuarioService CrearServicio(UsuarioRepositoryFake repositorio, GestorConexionFake gestor)
        {
            return new UsuarioService(repositorio, gestor);
        }

        private static Usuario CrearUsuarioPersistido()
        {
            return new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "Administrador",
                UsuarioLogin = "admin",
                ClaveAppHash = BCrypt.Net.BCrypt.HashPassword(ClaveAppValida, CostoBcryptPrueba),
                UsuarioBd = "login_admin",
                ClaveBdEnc = "valor-cifrado",
                NombreRol = ValoresDominio.Rol.Administrador,
                EstadoUsuario = ValoresDominio.EstadoUsuario.Activo
            };
        }
    }
}
