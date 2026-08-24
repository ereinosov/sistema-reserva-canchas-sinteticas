using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class CifradorAesTests
    {
        private static readonly byte[] ClavePrueba = CrearClaveFija();

        [TestMethod]
        public void Cifrar_Descifrar_DevuelveElTextoOriginal()
        {
            CifradorAes cifrador = new CifradorAes(ClavePrueba);
            const string plano = "ClaveSql#2026-Prueba";

            string cifrado = cifrador.Cifrar(plano);
            string recuperado = cifrador.Descifrar(cifrado);

            Assert.AreEqual(plano, recuperado);
        }

        [TestMethod]
        public void Cifrar_DosVecesElMismoTexto_ProduceValoresDistintosPorIvAleatorio()
        {
            CifradorAes cifrador = new CifradorAes(ClavePrueba);
            const string plano = "misma-clave";

            string primero = cifrador.Cifrar(plano);
            string segundo = cifrador.Cifrar(plano);

            Assert.AreNotEqual(primero, segundo);
            Assert.AreEqual(plano, cifrador.Descifrar(primero));
            Assert.AreEqual(plano, cifrador.Descifrar(segundo));
        }

        [TestMethod]
        public void Descifrar_TextoNoBase64_LanzaErrorCifrado()
        {
            CifradorAes cifrador = new CifradorAes(ClavePrueba);

            try
            {
                cifrador.Descifrar("esto no es base64 $$$");
                Assert.Fail("Debió lanzar ErrorCifradoException.");
            }
            catch (ErrorCifradoException)
            {
            }
        }

        [TestMethod]
        public void Descifrar_ValorCorto_LanzaErrorCifrado()
        {
            CifradorAes cifrador = new CifradorAes(ClavePrueba);
            string corto = Convert.ToBase64String(new byte[8]);

            try
            {
                cifrador.Descifrar(corto);
                Assert.Fail("Debió lanzar ErrorCifradoException.");
            }
            catch (ErrorCifradoException)
            {
            }
        }

        [TestMethod]
        public void Descifrar_ClaveDistinta_LanzaErrorCifrado()
        {
            CifradorAes cifradorOrigen = new CifradorAes(ClavePrueba);
            string cifrado = cifradorOrigen.Cifrar("secreto");

            byte[] otraClave = new byte[32];
            for (int i = 0; i < otraClave.Length; i++)
            {
                otraClave[i] = (byte)(i + 1);
            }

            CifradorAes cifradorAjeno = new CifradorAes(otraClave);

            try
            {
                cifradorAjeno.Descifrar(cifrado);
                Assert.Fail("Debió lanzar ErrorCifradoException.");
            }
            catch (ErrorCifradoException)
            {
            }
        }

        [TestMethod]
        public void Constructor_ClaveDeTamanoIncorrecto_LanzaArgumentException()
        {
            try
            {
                CifradorAes cifrador = new CifradorAes(new byte[16]);
                Assert.Fail("Debió lanzar ArgumentException.");
            }
            catch (ArgumentException)
            {
            }
        }

        private static byte[] CrearClaveFija()
        {
            byte[] clave = new byte[32];
            for (int i = 0; i < clave.Length; i++)
            {
                clave[i] = (byte)(200 - i);
            }

            return clave;
        }
    }
}
