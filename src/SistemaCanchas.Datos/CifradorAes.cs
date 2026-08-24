using System;
using System.Security.Cryptography;
using System.Text;

using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// AES-256 en modo CBC con IV aleatorio prefijado y salida Base64 (A12 §10.2).
    /// Independiente del DPAPI que protege la cadena de conexión (RNF12).
    /// </summary>
    internal sealed class CifradorAes
    {
        private const int TamanoIv = 16;
        private const int TamanoClave = 32;

        private readonly byte[] _clave;

        internal CifradorAes(byte[] clave)
        {
            if (clave == null || clave.Length != TamanoClave)
            {
                throw new ArgumentException("La clave AES debe tener 32 bytes (AES-256).", nameof(clave));
            }

            _clave = (byte[])clave.Clone();
        }

        internal string Cifrar(string textoPlano)
        {
            if (textoPlano == null)
            {
                throw new ArgumentNullException(nameof(textoPlano));
            }

            using (Aes aes = CrearAes())
            {
                aes.GenerateIV();
                using (ICryptoTransform transformador = aes.CreateEncryptor())
                {
                    byte[] plano = Encoding.UTF8.GetBytes(textoPlano);
                    byte[] cifrado = transformador.TransformFinalBlock(plano, 0, plano.Length);

                    byte[] resultado = new byte[aes.IV.Length + cifrado.Length];
                    Buffer.BlockCopy(aes.IV, 0, resultado, 0, aes.IV.Length);
                    Buffer.BlockCopy(cifrado, 0, resultado, aes.IV.Length, cifrado.Length);
                    return Convert.ToBase64String(resultado);
                }
            }
        }

        internal string Descifrar(string textoCifrado)
        {
            if (string.IsNullOrWhiteSpace(textoCifrado))
            {
                throw new ErrorCifradoException("El valor cifrado de la clave de motor está vacío.");
            }

            byte[] combinado;
            try
            {
                combinado = Convert.FromBase64String(textoCifrado);
            }
            catch (FormatException ex)
            {
                throw new ErrorCifradoException("El valor cifrado de la clave de motor no es Base64 válido.", ex);
            }

            if (combinado.Length <= TamanoIv)
            {
                throw new ErrorCifradoException("El valor cifrado de la clave de motor es demasiado corto.");
            }

            try
            {
                using (Aes aes = CrearAes())
                {
                    byte[] iv = new byte[TamanoIv];
                    byte[] cifrado = new byte[combinado.Length - TamanoIv];
                    Buffer.BlockCopy(combinado, 0, iv, 0, TamanoIv);
                    Buffer.BlockCopy(combinado, TamanoIv, cifrado, 0, cifrado.Length);
                    aes.IV = iv;

                    using (ICryptoTransform transformador = aes.CreateDecryptor())
                    {
                        byte[] plano = transformador.TransformFinalBlock(cifrado, 0, cifrado.Length);
                        return Encoding.UTF8.GetString(plano);
                    }
                }
            }
            catch (CryptographicException ex)
            {
                throw new ErrorCifradoException(
                    "No se pudo descifrar la clave de motor. La clave AES de esta estación no coincide o el valor está dañado.",
                    ex);
            }
        }

        private Aes CrearAes()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _clave;
            return aes;
        }
    }
}
