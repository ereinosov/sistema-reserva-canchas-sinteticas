using System;
using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Gestión de reservas y consulta de disponibilidad (RF05-RF08 / RF11).
    /// </summary>
    public interface IReservaService
    {
        /// <summary>Registra una reserva activa. Requiere sesión iniciada.</summary>
        /// <param name="idCliente">Cliente existente.</param>
        /// <param name="idHorario">Franja libre.</param>
        /// <returns>Id generado.</returns>
        int CrearReserva(int idCliente, int idHorario);

        /// <summary>Lista reservas con filtros opcionales. Requiere sesión iniciada.</summary>
        /// <param name="fecha">Fecha de la franja; null para no filtrar.</param>
        /// <param name="idCliente">Cliente; null para no filtrar.</param>
        /// <param name="idCancha">Cancha; null para no filtrar.</param>
        /// <param name="estadoReserva">activa, cancelada o vacío para no filtrar.</param>
        /// <returns>Reservas encontradas.</returns>
        IList<Reserva> ConsultarReservas(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva);

        /// <summary>Reprograma el horario de una reserva activa. Requiere sesión iniciada.</summary>
        /// <param name="idReserva">Identificador.</param>
        /// <param name="nuevoIdHorario">Nueva franja libre.</param>
        void ModificarHorario(int idReserva, int nuevoIdHorario);

        /// <summary>Cancela una reserva activa (RN03). Requiere sesión iniciada.</summary>
        /// <param name="idReserva">Identificador.</param>
        void CancelarReserva(int idReserva);

        /// <summary>Consulta las 16 franjas de una cancha en una fecha (RF11 / RN08). Requiere sesión iniciada.</summary>
        /// <param name="idCancha">Cancha.</param>
        /// <param name="fecha">Día a consultar.</param>
        /// <returns>Franjas con estado libre u ocupada.</returns>
        IList<Horario> ConsultarDisponibilidad(int idCancha, DateTime fecha);
    }
}
