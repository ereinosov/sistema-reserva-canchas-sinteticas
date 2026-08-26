using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SistemaCanchas.Datos;
using SistemaCanchas.Datos.Excepciones;

namespace SistemaCanchas.Tests
{
    [TestClass]
    public class CifradorAesTests
    {
        private static readonly byte[] Clave = ClaveFija();

        [TestMethod]
        public void CifrarYDescifrar_DevuelveElMismoTexto()
        {
            CifradorAes cifrador = new CifradorAes(Clave);
            const string plano = "ClaveSql#2026-Prueba";

            string cifrado = cifrador.Cifrar(plano);

            Assert.AreEqual(plano, cifrador.Descifrar(cifrado));
        }

        [TestMethod]
        public void CifrarDosVeces_DaValoresDistintos()
        {
            CifradorAes cifrador = new CifradorAes(Clave);
            const string plano = "misma-clave";

            string primero = cifrador.Cifrar(plano);
            string segundo = cifrador.Cifrar(plano);

            Assert.AreNotEqual(primero, segundo);
            Assert.AreEqual(plano, cifrador.Descifrar(primero));
            Assert.AreEqual(plano, cifrador.Descifrar(segundo));
        }

        [TestMethod]
        public void Descifrar_TextoInvalido_Falla()
        {
            CifradorAes cifrador = new CifradorAes(Clave);

            Assert.ThrowsException<ErrorCifradoException>(
                () => cifrador.Descifrar("esto no es base64 $$$"));
        }

        [TestMethod]
        public void Descifrar_ValorCorto_Falla()
        {
            CifradorAes cifrador = new CifradorAes(Clave);
            string corto = Convert.ToBase64String(new byte[8]);

            Assert.ThrowsException<ErrorCifradoException>(
                () => cifrador.Descifrar(corto));
        }

        [TestMethod]
        public void Descifrar_OtraClave_Falla()
        {
            string cifrado = new CifradorAes(Clave).Cifrar("secreto");
            byte[] otraClave = new byte[32];
            for (int i = 0; i < otraClave.Length; i++)
            {
                otraClave[i] = (byte)(i + 1);
            }

            Assert.ThrowsException<ErrorCifradoException>(
                () => new CifradorAes(otraClave).Descifrar(cifrado));
        }

        [TestMethod]
        public void Constructor_ClaveCorta_Falla()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new CifradorAes(new byte[16]));
        }

        private static byte[] ClaveFija()
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
