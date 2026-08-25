using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de horarios y disponibilidad (RF11 / RN05 / RN08).
    /// </summary>
    public sealed class HorarioRepository : IHorarioRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de la conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public HorarioRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public IList<Horario> ConsultarDisponibilidad(int idCancha, DateTime fecha)
        {
            List<Horario> resultado = new List<Horario>();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarDisponibilidad", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cancha", idCancha));
                    comando.Parameters.Add(ParametroSql.Fecha("@fecha", fecha.Date));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            resultado.Add(new Horario
                            {
                                IdHorario = LectorSql.Entero(lector, "id_horario"),
                                IdCancha = idCancha,
                                FechaHorario = LectorSql.Fecha(lector, "fecha_horario"),
                                HoraInicioHorario = LectorSql.Hora(lector, "hora_inicio_horario"),
                                HoraFinHorario = LectorSql.Hora(lector, "hora_fin_horario"),
                                EstadoFranja = LectorSql.Cadena(lector, "estado_franja")
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar la disponibilidad.", ex);
            }

            return resultado;
        }
    }
}
