using System.Text.RegularExpressions;

using SistemaCanchas.Entidades;
using SistemaCanchas.Negocio.Excepciones;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Validaciones de cliente (RN07). La unicidad la confirma el motor.
    /// </summary>
    internal static class ValidadorCliente
    {
        private static readonly Regex SoloDigitos = new Regex(@"^[0-9]+$", RegexOptions.CultureInvariant);
        private static readonly Regex Telefono = new Regex(@"^\+?[0-9]{7,14}$", RegexOptions.CultureInvariant);
        private static readonly Regex Correo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        internal static Cliente Normalizar(
            string nombre,
            string tipoDocumento,
            string numeroDocumento,
            string telefono,
            string correo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ValidacionNegocioException("El nombre del cliente es obligatorio.");
            }

            string nombreNormalizado = nombre.Trim();
            if (nombreNormalizado.Length > ValoresDominio.LongitudMaximaNombreCliente)
            {
                throw new ValidacionNegocioException("El nombre no puede superar 100 caracteres.");
            }

            if (!EsTipoDocumento(tipoDocumento))
            {
                throw new ValidacionNegocioException("El tipo de documento debe ser cédula, pasaporte o RUC.");
            }

            if (string.IsNullOrWhiteSpace(numeroDocumento))
            {
                throw new ValidacionNegocioException("El número de documento es obligatorio.");
            }

            string numero = numeroDocumento.Trim();
            if (numero.Length > ValoresDominio.LongitudMaximaNumeroDocumento)
            {
                throw new ValidacionNegocioException("El número de documento no puede superar 20 caracteres.");
            }

            // RN07: cédula ecuatoriana (provincia, persona natural y dígito verificador).
            if (string.Equals(tipoDocumento, ValoresDominio.TipoDocumento.Cedula, System.StringComparison.Ordinal))
            {
                ValidarCedulaEcuatoriana(numero);
            }

            if (string.IsNullOrWhiteSpace(telefono) || !Telefono.IsMatch(telefono.Trim()))
            {
                throw new ValidacionNegocioException("Ingrese un teléfono válido (7 a 15 dígitos; puede iniciar con +).");
            }

            if (string.IsNullOrWhiteSpace(correo) || !Correo.IsMatch(correo.Trim()))
            {
                throw new ValidacionNegocioException("Ingrese un correo electrónico válido.");
            }

            return new Cliente
            {
                NombreCliente = nombreNormalizado,
                TipoDocumentoCliente = tipoDocumento,
                NumeroDocumentoCliente = numero,
                TelefonoCliente = telefono.Trim(),
                CorreoCliente = correo.Trim()
            };
        }

        private static void ValidarCedulaEcuatoriana(string numero)
        {
            if (numero.Length != ValoresDominio.LongitudCedula || !SoloDigitos.IsMatch(numero))
            {
                throw new ValidacionNegocioException("La cédula debe contener exactamente 10 dígitos numéricos.");
            }

            int provincia = (numero[0] - '0') * 10 + (numero[1] - '0');
            if (provincia < 1 || provincia > 24)
            {
                throw new ValidacionNegocioException("El código de provincia de la cédula debe estar entre 01 y 24.");
            }

            int tercerDigito = numero[2] - '0';
            if (tercerDigito < 0 || tercerDigito > 5)
            {
                throw new ValidacionNegocioException("El tercer dígito de la cédula debe estar entre 0 y 5 (persona natural).");
            }

            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int suma = 0;
            for (int i = 0; i < 9; i++)
            {
                int producto = (numero[i] - '0') * coeficientes[i];
                if (producto > 9)
                {
                    producto -= 9;
                }

                suma += producto;
            }

            int verificador = (10 - (suma % 10)) % 10;
            if (numero[9] - '0' != verificador)
            {
                throw new ValidacionNegocioException("El dígito verificador de la cédula no es válido.");
            }
        }

        private static bool EsTipoDocumento(string tipo)
        {
            return string.Equals(tipo, ValoresDominio.TipoDocumento.Cedula, System.StringComparison.Ordinal) ||
                   string.Equals(tipo, ValoresDominio.TipoDocumento.Pasaporte, System.StringComparison.Ordinal) ||
                   string.Equals(tipo, ValoresDominio.TipoDocumento.Ruc, System.StringComparison.Ordinal);
        }

        internal static string EscaparLike(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            return texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }
    }
}
