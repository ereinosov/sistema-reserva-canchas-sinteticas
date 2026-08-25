using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de pagos (RF09-RF10 / RN04).
    /// </summary>
    public sealed class PagoRepository : IPagoRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de la conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public PagoRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public int Insertar(Pago pago)
        {
            if (pago == null)
            {
                throw new ArgumentNullException(nameof(pago));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_RegistrarPago", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_reserva", pago.IdReserva));
                    comando.Parameters.Add(ParametroSql.Decimal("@monto_pago", 9, 2, pago.MontoPago.GetValueOrDefault()));
                    comando.Parameters.Add(ParametroSql.Fecha("@fecha_pago", pago.FechaPago));
                    comando.Parameters.Add(ParametroSql.Char("@estado_pago", 10, pago.EstadoPago));
                    SqlParameter idSalida = ParametroSql.EnteroSalida("@id_pago_nuevo");
                    comando.Parameters.Add(idSalida);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(idSalida.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo registrar el pago.", ex);
            }
        }

        public IList<Pago> ObtenerTodos(DateTime? fecha, int? idCliente, int? idCancha, string estadoReserva)
        {
            List<Pago> resultado = new List<Pago>();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarEstadoPago", conexion))
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
                throw new ErrorAccesoDatosException("No se pudo consultar el estado de pago.", ex);
            }

            return resultado;
        }

        private static Pago MapearConsulta(SqlDataReader lector)
        {
            return new Pago
            {
                IdReserva = LectorSql.Entero(lector, "id_reserva"),
                NombreCliente = LectorSql.Cadena(lector, "nombre_cliente"),
                NombreCancha = LectorSql.Cadena(lector, "nombre_cancha"),
                FechaHorario = LectorSql.Fecha(lector, "fecha_horario"),
                HoraInicioHorario = LectorSql.Hora(lector, "hora_inicio_horario"),
                EstadoPago = LectorSql.CadenaFija(lector, "estado_pago"),
                MontoPago = LectorSql.DecimalNulo(lector, "monto_pago"),
                FechaPago = LectorSql.FechaNula(lector, "fecha_pago"),
                EstadoReserva = LectorSql.CadenaFija(lector, "estado_reserva")
            };
        }
    }
}
