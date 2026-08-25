using System;
using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a RESERVAS mediante los procedimientos del artefacto A11 (RF05-RF08).
    /// </summary>
    public interface IReservaRepository
    {
        /// <summary>Crea una reserva activa (sp_CrearReserva, RF05).</summary>
        /// <param name="reserva">Cliente y usuario que registra.</param>
        /// <param name="idsHorario">Franjas a reservar en la misma operación.</param>
        /// <returns>Id generado.</returns>
        int Insertar(Reserva reserva, IList<int> idsHorario);

        /// <summary>Lista reservas con filtros opcionales (sp_ConsultarReservas, RF06).</summary>
        /// <param name="fecha">Fecha de la franja o null.</param>
        /// <param name="idCliente">Cliente o null.</param>
        /// <param name="idCancha">Cancha o null.</param>
        /// <param name="estadoReserva">activa, cancelada o null.</param>
        /// <returns>Reservas encontradas.</returns>
        IList<Reserva> ObtenerTodos(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva);

        /// <summary>Cambia el horario de una reserva activa (sp_ModificarReservaHorario, RF07).</summary>
        /// <param name="idReserva">Identificador.</param>
        /// <param name="nuevoIdHorario">Nueva franja libre.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool ActualizarHorario(int idReserva, int nuevoIdHorario);

        /// <summary>Pasa la reserva a cancelada (sp_CancelarReserva, RF08 / RN03).</summary>
        /// <param name="idReserva">Identificador.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Cancelar(int idReserva);
    }
}
