using System;
using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Negocio.Interfaces
{
    /// <summary>
    /// Gestión de canchas (RF13 / RN10).
    /// </summary>
    public interface ICanchaService
    {
        /// <summary>Registra una cancha activa. Requiere administrador.</summary>
        /// <param name="nombreCancha">Nombre único.</param>
        /// <param name="horaInicioOperacion">Inicio operativo de la cancha.</param>
        /// <param name="horaFinOperacion">Fin operativo de la cancha.</param>
        /// <returns>Id generado.</returns>
        int RegistrarCancha(string nombreCancha, TimeSpan horaInicioOperacion, TimeSpan horaFinOperacion);

        /// <summary>Lista canchas. Requiere sesión iniciada.</summary>
        /// <returns>Todas las canchas.</returns>
        IList<Cancha> ObtenerTodas();

        /// <summary>Lista solo canchas activas. Requiere sesión iniciada.</summary>
        /// <returns>Canchas en estado activa.</returns>
        IList<Cancha> ObtenerActivas();

        /// <summary>Actualiza el nombre. Requiere administrador.</summary>
        /// <param name="idCancha">Identificador.</param>
        /// <param name="nombreCancha">Nuevo nombre único.</param>
        /// <param name="horaInicioOperacion">Inicio operativo de la cancha.</param>
        /// <param name="horaFinOperacion">Fin operativo de la cancha.</param>
        void ModificarCancha(int idCancha, string nombreCancha, TimeSpan horaInicioOperacion, TimeSpan horaFinOperacion);

        /// <summary>
        /// Desactiva la cancha (RN10): no recibe reservas nuevas; las ya registradas no se alteran.
        /// Requiere administrador.
        /// </summary>
        /// <param name="idCancha">Identificador.</param>
        void DesactivarCancha(int idCancha);

        void ActivarCancha(int idCancha);

        /// <summary>Indica si la cancha existe y está activa (RN10).</summary>
        /// <param name="idCancha">Identificador.</param>
        /// <returns>true si está activa.</returns>
        bool CanchaActiva(int idCancha);
    }
}
