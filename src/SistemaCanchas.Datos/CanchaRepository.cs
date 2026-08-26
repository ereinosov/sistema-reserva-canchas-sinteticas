using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de canchas (RF13 / RN10).
    /// </summary>
    public sealed class CanchaRepository : ICanchaRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public CanchaRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public int Insertar(Cancha cancha)
        {
            if (cancha == null)
            {
                throw new ArgumentNullException(nameof(cancha));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_RegistrarCancha", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.NVarChar("@nombre_cancha", 60, cancha.NombreCancha));
                    comando.Parameters.Add(ParametroSql.Hora("@hora_inicio_operacion", cancha.HoraInicioOperacion));
                    comando.Parameters.Add(ParametroSql.Hora("@hora_fin_operacion", cancha.HoraFinOperacion));
                    SqlParameter idSalida = ParametroSql.EnteroSalida("@id_cancha_nueva");
                    comando.Parameters.Add(idSalida);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(idSalida.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo registrar la cancha.", ex);
            }
        }

        public IList<Cancha> ObtenerTodos(string estadoCancha)
        {
            List<Cancha> resultado = new List<Cancha>();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarCanchas", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Char("@estado_cancha", 10, estadoCancha));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            resultado.Add(Mapear(lector));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar las canchas.", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar las canchas.", ex);
            }

            return resultado;
        }

        public bool Actualizar(Cancha cancha)
        {
            if (cancha == null)
            {
                throw new ArgumentNullException(nameof(cancha));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ModificarCancha", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cancha", cancha.IdCancha));
                    comando.Parameters.Add(ParametroSql.NVarChar("@nombre_cancha", 60, cancha.NombreCancha));
                    comando.Parameters.Add(ParametroSql.Hora("@hora_inicio_operacion", cancha.HoraInicioOperacion));
                    comando.Parameters.Add(ParametroSql.Hora("@hora_fin_operacion", cancha.HoraFinOperacion));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo modificar la cancha.", ex);
            }
        }

        public bool Desactivar(int idCancha)
        {
            if (idCancha <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idCancha), "El identificador de cancha debe ser positivo.");
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_DesactivarCancha", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cancha", idCancha));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo desactivar la cancha.", ex);
            }
        }

        public bool Activar(int idCancha)
        {
            if (idCancha <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idCancha), "El identificador de cancha debe ser positivo.");
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ActivarCancha", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cancha", idCancha));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo activar la cancha.", ex);
            }
        }

        private static Cancha Mapear(SqlDataReader lector)
        {
            return new Cancha
            {
                IdCancha = LectorSql.Entero(lector, "id_cancha"),
                NombreCancha = LectorSql.Cadena(lector, "nombre_cancha"),
                EstadoCancha = LectorSql.CadenaFija(lector, "estado_cancha"),
                HoraInicioOperacion = LectorSql.Hora(lector, "hora_inicio_operacion"),
                HoraFinOperacion = LectorSql.Hora(lector, "hora_fin_operacion")
            };
        }
    }
}
