using System;
using System.Configuration;

using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Lectura de App.config. La cadena de arranque vive en connectionStrings (RNF12).
    /// </summary>
    internal static class ConfiguracionApp
    {
        internal const string NombreCadenaBootstrap = "ReservaCanchasBootstrap";

        internal static string ObtenerCadenaBootstrap()
        {
            ConnectionStringSettings seccion = ConfigurationManager.ConnectionStrings[NombreCadenaBootstrap];
            if (seccion == null || string.IsNullOrWhiteSpace(seccion.ConnectionString))
            {
                throw new ConfiguracionInvalidaException(
                    "No se encontró la cadena de conexión '" + NombreCadenaBootstrap +
                    "' en App.config. Configure el acceso de login_bootstrap antes de iniciar.");
            }

            return seccion.ConnectionString.Trim();
        }
    }
}
