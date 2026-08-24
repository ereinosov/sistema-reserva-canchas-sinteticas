using System;
using System.Security.Cryptography;

namespace SistemaCanchas.Negocio
{
    /// <summary>
    /// Clave aleatoria del login de SQL Server. Cumple CHECK_POLICY (mayúscula, minúscula, dígito y símbolo).
    /// </summary>
    internal static class GeneradorClaveMotor
    {
        private const int Longitud = 24;
        private const string Mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Minusculas = "abcdefghijkmnopqrstuvwxyz";
        private const string Digitos = "23456789";
        private const string Simbolos = "#@%*-_!";

        internal static string Generar()
        {
            char[] resultado = new char[Longitud];
            using (RandomNumberGenerator generador = RandomNumberGenerator.Create())
            {
                resultado[0] = Elegir(generador, Mayusculas);
                resultado[1] = Elegir(generador, Minusculas);
                resultado[2] = Elegir(generador, Digitos);
                resultado[3] = Elegir(generador, Simbolos);

                string alfabeto = Mayusculas + Minusculas + Digitos + Simbolos;
                for (int i = 4; i < Longitud; i++)
                {
                    resultado[i] = Elegir(generador, alfabeto);
                }

                Barajar(generador, resultado);
            }

            return new string(resultado);
        }

        private static char Elegir(RandomNumberGenerator generador, string alfabeto)
        {
            byte[] buffer = new byte[4];
            generador.GetBytes(buffer);
            int indice = (int)(BitConverter.ToUInt32(buffer, 0) % (uint)alfabeto.Length);
            return alfabeto[indice];
        }

        private static void Barajar(RandomNumberGenerator generador, char[] valores)
        {
            for (int i = valores.Length - 1; i > 0; i--)
            {
                byte[] buffer = new byte[4];
                generador.GetBytes(buffer);
                int j = (int)(BitConverter.ToUInt32(buffer, 0) % (uint)(i + 1));
                char temporal = valores[i];
                valores[i] = valores[j];
                valores[j] = temporal;
            }
        }
    }
}
