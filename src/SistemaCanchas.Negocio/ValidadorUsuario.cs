using System.Text.RegularExpressions;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Validaciones de formato de cuentas (RN11). La unicidad la confirma el motor (A11).
    /// </summary>
    internal static class ValidadorUsuario
    {
        private static readonly Regex PatronLogin = new Regex(
            @"^[A-Za-z][A-Za-z0-9_]{2,29}$",
            RegexOptions.CultureInvariant);

        internal static void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ValidacionNegocioException("El nombre del usuario es obligatorio.");
            }

            if (nombre.Trim().Length > ValoresDominio.LongitudMaximaNombreUsuario)
            {
                throw new ValidacionNegocioException("El nombre no puede superar 100 caracteres.");
            }
        }

        internal static void ValidarLogin(string usuarioLogin)
        {
            if (string.IsNullOrWhiteSpace(usuarioLogin))
            {
                throw new ValidacionNegocioException("El usuario de acceso es obligatorio.");
            }

            string login = usuarioLogin.Trim();
            if (login.Length < ValoresDominio.LongitudMinimaUsuarioLogin ||
                login.Length > ValoresDominio.LongitudMaximaUsuarioLogin ||
                !PatronLogin.IsMatch(login))
            {
                throw new ValidacionNegocioException(
                    "El usuario de acceso debe tener entre 3 y 30 caracteres, empezar por una letra " +
                    "y contener solo letras, dígitos o guion bajo.");
            }
        }

        internal static void ValidarClaveApp(string claveApp)
        {
            if (string.IsNullOrEmpty(claveApp))
            {
                throw new ValidacionNegocioException("La clave es obligatoria.");
            }

            if (claveApp.Length < ValoresDominio.LongitudMinimaClaveApp)
            {
                throw new ValidacionNegocioException("La clave debe tener al menos 8 caracteres.");
            }
        }

        internal static void ValidarRol(string nombreRol)
        {
            if (!string.Equals(nombreRol, ValoresDominio.Rol.Administrador, System.StringComparison.Ordinal) &&
                !string.Equals(nombreRol, ValoresDominio.Rol.Empleado, System.StringComparison.Ordinal))
            {
                throw new ValidacionNegocioException("El rol debe ser administrador o empleado.");
            }
        }
    }
}
