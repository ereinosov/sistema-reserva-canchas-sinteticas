using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de reservas (RF05-RF08 / RN01 / RN03 / RN06 / RN08).
    /// </summary>
    public sealed class ReservaRepository : IReservaRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de la conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public ReservaRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        /// <inheritdoc />
        public int Insertar(Reserva reserva)
        {
            if (reserva == null)
            {
                throw new ArgumentNullException(nameof(reserva));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_CrearReserva", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cliente", reserva.IdCliente));
                    comando.Parameters.Add(ParametroSql.Entero("@id_horario", reserva.IdHorario));
                    comando.Parameters.Add(ParametroSql.Entero("@id_usuario", reserva.IdUsuario));
                    SqlParameter idSalida = ParametroSql.EnteroSalida("@id_reserva_nueva");
                    comando.Parameters.Add(idSalida);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(idSalida.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo registrar la reserva.", ex);
            }
        }

        /// <inheritdoc />
        public IList<Reserva> ObtenerTodos(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva)
        {
            List<Reserva> resultado = new List<Reserva>();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarReservas", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Fecha("@fecha", fecha));
                    comando.Parameters.Add(ParametroSql.EnteroNulo("@id_cliente", idCliente));
                    comando.Parameters.Add(ParametroSql.EnteroNulo("@id_cancha", idCancha));
                    comando.Parameters.Add(ParametroSql.Char("@estado", 10, estadoReserva));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            resultado.Add(MapearConsulta(lector));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar las reservas.", ex);
            }

            return resultado;
        }

        /// <inheritdoc />
        public bool ActualizarHorario(int idReserva, int nuevoIdHorario)
        {
            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ModificarReservaHorario", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_reserva", idReserva));
                    comando.Parameters.Add(ParametroSql.Entero("@nuevo_id_horario", nuevoIdHorario));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo modificar el horario de la reserva.", ex);
            }
        }

        /// <inheritdoc />
        public bool Cancelar(int idReserva)
        {
            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_CancelarReserva", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_reserva", idReserva));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo cancelar la reserva.", ex);
            }
        }

        private static Reserva MapearConsulta(SqlDataReader lector)
        {
            return new Reserva
            {
                IdReserva = LectorSql.Entero(lector, "id_reserva"),
                NombreCliente = LectorSql.Cadena(lector, "nombre_cliente"),
                NombreCancha = LectorSql.Cadena(lector, "nombre_cancha"),
                FechaHorario = LectorSql.Fecha(lector, "fecha_horario"),
                HoraInicioHorario = LectorSql.Hora(lector, "hora_inicio_horario"),
                HoraFinHorario = LectorSql.Hora(lector, "hora_fin_horario"),
                RegistradoPor = LectorSql.Cadena(lector, "registrado_por"),
                EstadoReserva = LectorSql.CadenaFija(lector, "estado_reserva")
            };
        }
    }
}
