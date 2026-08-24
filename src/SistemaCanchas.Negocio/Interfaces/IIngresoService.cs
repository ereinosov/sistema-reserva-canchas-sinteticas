using System;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Consulta de ingresos (RF12 / RN09). Exclusiva del administrador.
    /// </summary>
    public interface IIngresoService
    {
        /// <summary>Consulta pagos en estado pagado cuyo horario cae en el rango. Requiere administrador.</summary>
        /// <param name="fechaInicio">Inicio del rango (inclusive).</param>
        /// <param name="fechaFin">Fin del rango (inclusive).</param>
        /// <returns>Total y detalle.</returns>
        ConsultaIngresos ConsultarIngresos(DateTime fechaInicio, DateTime fechaFin);
    }
}
