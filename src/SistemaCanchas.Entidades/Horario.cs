using System;

namespace SistemaCanchas.Entidades
{
    /// <summary>
    /// Fila de dbo.HORARIOS (A7 / A11). La hora de fin no se persiste: es derivada (RN05).
    /// </summary>
    public class Horario
    {
        public int IdHorario { get; set; }

        public int IdCancha { get; set; }

        public DateTime FechaHorario { get; set; }

        public TimeSpan HoraInicioHorario { get; set; }

        /// <summary>
        /// Hora de fin derivada (RN05). No se persiste; la calcula dbo.fn_HoraFinFranja.
        /// </summary>
        public TimeSpan HoraFinHorario { get; set; }

        /// <summary>
        /// libre u ocupada según sp_ConsultarDisponibilidad (RN08). No es columna de HORARIOS.
        /// </summary>
        public string EstadoFranja { get; set; }
    }
}
