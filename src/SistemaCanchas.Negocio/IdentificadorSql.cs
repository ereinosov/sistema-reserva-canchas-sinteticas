using System.Text;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Nombre del login de SQL Server a partir del usuario de aplicación (VARCHAR(30)).
    /// </summary>
    internal static class IdentificadorSql
    {
        internal static string DesdeLogin(string usuarioLogin)
        {
            string normalizado = usuarioLogin.Trim().ToLowerInvariant();
            StringBuilder constructor = new StringBuilder("u_");
            for (int i = 0; i < normalizado.Length && constructor.Length < ValoresDominio.LongitudMaximaUsuarioLogin; i++)
            {
                char caracter = normalizado[i];
                if ((caracter >= 'a' && caracter <= 'z') ||
                    (caracter >= '0' && caracter <= '9') ||
                    caracter == '_')
                {
                    constructor.Append(caracter);
                }
            }

            return constructor.ToString();
        }
    }
}
