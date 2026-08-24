using System;
using System.IO;
using System.Security.Cryptography;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Persistencia de la clave AES-256 protegida con DPAPI (A12 §10.3).
    /// Nunca se versiona ni se escribe en código fuente.
    /// </summary>
    internal static class AlmacenClaveAes
    {
        private const int TamanoClave = 32;
        private const string NombreCarpeta = "SistemaCanchas";
        private const string NombreArchivo = "aes.key";

        internal static byte[] ObtenerOCrear()
        {
            string rutaMaquina = CombinarRuta(Environment.SpecialFolder.CommonApplicationData);
            try
            {
                return LeerOGenerar(rutaMaquina, DataProtectionScope.LocalMachine);
            }
            catch (UnauthorizedAccessException)
            {
                string rutaUsuario = CombinarRuta(Environment.SpecialFolder.LocalApplicationData);
                return LeerOGenerar(rutaUsuario, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException)
            {
                string rutaUsuario = CombinarRuta(Environment.SpecialFolder.LocalApplicationData);
                return LeerOGenerar(rutaUsuario, DataProtectionScope.CurrentUser);
            }
        }

        private static string CombinarRuta(Environment.SpecialFolder carpeta)
        {
            return Path.Combine(Environment.GetFolderPath(carpeta), NombreCarpeta, NombreArchivo);
        }

        private static byte[] LeerOGenerar(string ruta, DataProtectionScope ambito)
        {
            string directorio = Path.GetDirectoryName(ruta);
            if (string.IsNullOrEmpty(directorio))
            {
                throw new InvalidOperationException("No se pudo resolver el directorio de la clave AES.");
            }

            Directory.CreateDirectory(directorio);

            if (File.Exists(ruta))
            {
                byte[] protegido = File.ReadAllBytes(ruta);
                byte[] clave = ProtectedData.Unprotect(protegido, null, ambito);
                ValidarTamano(clave);
                return clave;
            }

            byte[] claveNueva = new byte[TamanoClave];
            using (RandomNumberGenerator generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(claveNueva);
            }

            byte[] protegidoNuevo = ProtectedData.Protect(claveNueva, null, ambito);
            File.WriteAllBytes(ruta, protegidoNuevo);
            return claveNueva;
        }

        private static void ValidarTamano(byte[] clave)
        {
            if (clave == null || clave.Length != TamanoClave)
            {
                throw new CryptographicException("La clave AES almacenada no tiene 32 bytes.");
            }
        }
    }
}
