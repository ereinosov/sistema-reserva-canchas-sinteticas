using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a CANCHAS mediante los procedimientos del artefacto A11 (RF13).
    /// </summary>
    public interface ICanchaRepository
    {
        /// <summary>Registra una cancha (sp_RegistrarCancha).</summary>
        /// <param name="cancha">Datos a persistir.</param>
        /// <returns>Id generado.</returns>
        int Insertar(Cancha cancha);

        /// <summary>Lista canchas, con filtro opcional de estado (sp_ConsultarCanchas).</summary>
        /// <param name="estadoCancha">activa, inactiva o null para todas.</param>
        /// <returns>Canchas encontradas.</returns>
        IList<Cancha> ObtenerTodos(string estadoCancha);

        /// <summary>Actualiza el nombre (sp_ModificarCancha).</summary>
        /// <param name="cancha">Cancha con id y nuevo nombre.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Actualizar(Cancha cancha);

        /// <summary>Pasa la cancha a inactiva (sp_DesactivarCancha, RN10).</summary>
        /// <param name="idCancha">Identificador.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Desactivar(int idCancha);
    }
}
