using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Construcción de SqlParameter tipados. Prohibido concatenar SQL (RNF11).
    /// </summary>
    internal static class ParametroSql
    {
        internal static SqlParameter VarChar(string nombre, int tamano, string valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.VarChar, tamano);
            parametro.Value = ValorONulo(valor);
            return parametro;
        }

        internal static SqlParameter NVarChar(string nombre, int tamano, string valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.NVarChar, tamano);
            parametro.Value = ValorONulo(valor);
            return parametro;
        }

        internal static SqlParameter Char(string nombre, int tamano, string valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Char, tamano);
            parametro.Value = ValorONulo(valor);
            return parametro;
        }

        internal static SqlParameter Entero(string nombre, int valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Int);
            parametro.Value = valor;
            return parametro;
        }

        internal static SqlParameter EnteroNulo(string nombre, int? valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Int);
            parametro.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
            return parametro;
        }

        internal static SqlParameter EnteroSalida(string nombre)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Int);
            parametro.Direction = ParameterDirection.Output;
            return parametro;
        }

        internal static SqlParameter Decimal(string nombre, byte precision, byte escala, decimal valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Decimal);
            parametro.Precision = precision;
            parametro.Scale = escala;
            parametro.Value = valor;
            return parametro;
        }

        internal static SqlParameter Fecha(string nombre, DateTime? valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Date);
            parametro.Value = valor.HasValue ? (object)valor.Value.Date : DBNull.Value;
            return parametro;
        }

        internal static SqlParameter Hora(string nombre, TimeSpan valor)
        {
            SqlParameter parametro = new SqlParameter(nombre, SqlDbType.Time);
            parametro.Value = valor;
            return parametro;
        }

        private static object ValorONulo(string valor)
        {
            if (valor == null)
            {
                return DBNull.Value;
            }

            return valor;
        }
    }
}
