using System;
using System.Data;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Lectura segura de columnas. Las CHAR del A11 llegan rellenas con espacios.
    /// </summary>
    internal static class LectorSql
    {
        internal static string Cadena(IDataRecord fila, string columna)
        {
            int indice = fila.GetOrdinal(columna);
            if (fila.IsDBNull(indice))
            {
                return string.Empty;
            }

            return fila.GetString(indice);
        }

        internal static string CadenaFija(IDataRecord fila, string columna)
        {
            return Cadena(fila, columna).TrimEnd();
        }

        internal static int Entero(IDataRecord fila, string columna)
        {
            return fila.GetInt32(fila.GetOrdinal(columna));
        }

        internal static decimal Decimal(IDataRecord fila, string columna)
        {
            return fila.GetDecimal(fila.GetOrdinal(columna));
        }

        internal static decimal? DecimalNulo(IDataRecord fila, string columna)
        {
            int indice = fila.GetOrdinal(columna);
            if (fila.IsDBNull(indice))
            {
                return null;
            }

            return fila.GetDecimal(indice);
        }

        internal static DateTime Fecha(IDataRecord fila, string columna)
        {
            return fila.GetDateTime(fila.GetOrdinal(columna));
        }

        internal static DateTime? FechaNula(IDataRecord fila, string columna)
        {
            int indice = fila.GetOrdinal(columna);
            if (fila.IsDBNull(indice))
            {
                return null;
            }

            return fila.GetDateTime(indice);
        }

        internal static TimeSpan Hora(IDataRecord fila, string columna)
        {
            object valor = fila.GetValue(fila.GetOrdinal(columna));
            if (valor is TimeSpan)
            {
                return (TimeSpan)valor;
            }

            DateTime fechaHora = Convert.ToDateTime(valor);
            return fechaHora.TimeOfDay;
        }
    }
}
