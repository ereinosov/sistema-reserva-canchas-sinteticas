using System;
using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a HORARIOS mediante sp_ConsultarDisponibilidad (RF11 / RN05 / RN08).
    /// </summary>
    public interface IHorarioRepository
    {
        /// <summary>Lista las franjas de una cancha en una fecha, generando las que falten.</summary>
        /// <param name="idCancha">Cancha consultada.</param>
        /// <param name="fecha">Día a consultar.</param>
        /// <returns>Franjas con estado libre u ocupada.</returns>
        IList<Horario> ConsultarDisponibilidad(int idCancha, DateTime fecha);
    }
}
