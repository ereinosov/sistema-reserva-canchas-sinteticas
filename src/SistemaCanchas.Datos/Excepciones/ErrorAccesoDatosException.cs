using System;
using System.Data.SqlClient;

namespace SistemaCanchas.Datos.Excepciones
{
    /// <summary>
    /// Falla de ADO.NET o del motor SQL. La presentación no debe mostrar el detalle interno.
    /// </summary>
    public sealed class ErrorAccesoDatosException : Exception
    {
        /// <summary>Número de error de SQL Server, si la causa es SqlException (p. ej. 50030).</summary>
        public int NumeroSql { get; private set; }

        /// <summary>Inicializa la excepción con contexto y la causa original.</summary>
        /// <param name="mensaje">Mensaje seguro para capas superiores.</param>
        /// <param name="interna">Excepción de ADO.NET o del motor.</param>
        public ErrorAccesoDatosException(string mensaje, Exception interna)
            : base(mensaje, interna)
        {
            NumeroSql = ExtraerNumero(interna);
        }

        /// <summary>Inicializa la excepción con un mensaje seguro.</summary>
        /// <param name="mensaje">Mensaje seguro para capas superiores.</param>
        public ErrorAccesoDatosException(string mensaje)
            : base(mensaje)
        {
        }

        /// <summary>Inicializa la excepción con mensaje y código THROW de SQL Server.</summary>
        /// <param name="mensaje">Mensaje seguro para capas superiores.</param>
        /// <param name="numeroSql">Número de error del motor (p. ej. 50020).</param>
        public ErrorAccesoDatosException(string mensaje, int numeroSql)
            : base(mensaje)
        {
            NumeroSql = numeroSql;
        }

        private static int ExtraerNumero(Exception excepcion)
        {
            SqlException sql = excepcion as SqlException;
            if (sql != null)
            {
                return sql.Number;
            }

            if (excepcion != null && excepcion.InnerException != null)
            {
                return ExtraerNumero(excepcion.InnerException);
            }

            return 0;
        }
    }
}
