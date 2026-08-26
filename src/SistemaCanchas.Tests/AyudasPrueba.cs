using SistemaCanchas.Entidades;

namespace SistemaCanchas.Tests
{
    // Usuarios de sesión que se reutilizan en varios tests.
    internal static class SesionPrueba
    {
        internal static Usuario Admin()
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

        internal static Usuario Empleado()
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
