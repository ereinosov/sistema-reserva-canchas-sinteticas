using System;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de ingresos (RF12 / RN09).
    /// </summary>
    public sealed class IngresoRepository : IIngresoRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de la conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public IngresoRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public ConsultaIngresos Consultar(DateTime fechaInicio, DateTime fechaFin)
        {
            ConsultaIngresos resultado = new ConsultaIngresos();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarIngresos", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Fecha("@fecha_inicio", fechaInicio.Date));
                    comando.Parameters.Add(ParametroSql.Fecha("@fecha_fin", fechaFin.Date));
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            resultado.TotalIngresos = LectorSql.Decimal(lector, "total_ingresos");
                        }

                        if (lector.NextResult())
                        {
                            while (lector.Read())
                            {
                                resultado.Detalle.Add(Mapear(lector));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo consultar los ingresos.", ex);
            }

            return resultado;
        }

        private static Ingreso Mapear(SqlDataReader lector)
        {
            return new Ingreso
            {
                IdPago = LectorSql.Entero(lector, "id_pago"),
                IdReserva = LectorSql.Entero(lector, "id_reserva"),
                NombreCliente = LectorSql.Cadena(lector, "nombre_cliente"),
                NombreCancha = LectorSql.Cadena(lector, "nombre_cancha"),
                FechaHorario = LectorSql.Fecha(lector, "fecha_horario"),
                HoraInicioHorario = LectorSql.Hora(lector, "hora_inicio_horario"),
                MontoPago = LectorSql.Decimal(lector, "monto_pago"),
                FechaPago = LectorSql.FechaNula(lector, "fecha_pago")
            };
        }
    }
}
