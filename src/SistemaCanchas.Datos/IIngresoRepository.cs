using System;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a la consulta de ingresos (sp_ConsultarIngresos, RF12 / RN09).
    /// </summary>
    public interface IIngresoRepository
    {
        /// <summary>Consulta el total y el detalle de pagos pagados en un rango de fechas de franja.</summary>
        /// <param name="fechaInicio">Inicio del rango (inclusive).</param>
        /// <param name="fechaFin">Fin del rango (inclusive).</param>
        /// <returns>Total y filas de detalle.</returns>
        ConsultaIngresos Consultar(DateTime fechaInicio, DateTime fechaFin);
    }
}
