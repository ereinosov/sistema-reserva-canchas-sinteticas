using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using SistemaCanchas.Datos.Excepciones;
using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Repositorio de clientes (RF01-RF04 / RN02 / RN07).
    /// </summary>
    public sealed class ClienteRepository : IClienteRepository
    {
        private readonly IGestorConexion _gestorConexion;

        /// <summary>
        /// Inicializa el repositorio con el gestor de la conexión de sesión.
        /// </summary>
        /// <param name="gestorConexion">Gestor de la conexión individual activa.</param>
        public ClienteRepository(IGestorConexion gestorConexion)
        {
            if (gestorConexion == null)
            {
                throw new ArgumentNullException(nameof(gestorConexion));
            }

            _gestorConexion = gestorConexion;
        }

        public int Insertar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_RegistrarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    AgregarDatos(comando, cliente);
                    SqlParameter idSalida = ParametroSql.EnteroSalida("@id_cliente_nuevo");
                    comando.Parameters.Add(idSalida);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return Convert.ToInt32(idSalida.Value);
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo registrar el cliente.", ex);
            }
        }

        public IList<Cliente> ObtenerTodos(string numeroDocumento, string nombre)
        {
            List<Cliente> resultado = new List<Cliente>();

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ConsultarClientes", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.VarChar("@numero_documento_cliente", 20, numeroDocumento));
                    comando.Parameters.Add(ParametroSql.NVarChar("@nombre_cliente", 100, nombre));
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
                throw new ErrorAccesoDatosException("No se pudo consultar los clientes.", ex);
            }

            return resultado;
        }

        public bool Actualizar(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente));
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_ModificarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cliente", cliente.IdCliente));
                    AgregarDatos(comando, cliente);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo modificar el cliente.", ex);
            }
        }

        public bool Eliminar(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idCliente), "El identificador de cliente debe ser positivo.");
            }

            try
            {
                using (SqlConnection conexion = _gestorConexion.ObtenerConexionActiva())
                using (SqlCommand comando = new SqlCommand("sp_EliminarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(ParametroSql.Entero("@id_cliente", idCliente));
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new ErrorAccesoDatosException("No se pudo eliminar el cliente.", ex);
            }
        }

        private static void AgregarDatos(SqlCommand comando, Cliente cliente)
        {
            comando.Parameters.Add(ParametroSql.NVarChar("@nombre_cliente", 100, cliente.NombreCliente));
            comando.Parameters.Add(ParametroSql.Char("@tipo_documento_cliente", 10, cliente.TipoDocumentoCliente));
            comando.Parameters.Add(ParametroSql.VarChar("@numero_documento_cliente", 20, cliente.NumeroDocumentoCliente));
            comando.Parameters.Add(ParametroSql.VarChar("@telefono_cliente", 15, cliente.TelefonoCliente));
            comando.Parameters.Add(ParametroSql.VarChar("@correo_cliente", 100, cliente.CorreoCliente));
        }

        private static Cliente Mapear(SqlDataReader lector)
        {
            return new Cliente
            {
                IdCliente = LectorSql.Entero(lector, "id_cliente"),
                NombreCliente = LectorSql.Cadena(lector, "nombre_cliente"),
                TipoDocumentoCliente = LectorSql.CadenaFija(lector, "tipo_documento_cliente"),
                NumeroDocumentoCliente = LectorSql.Cadena(lector, "numero_documento_cliente"),
                TelefonoCliente = LectorSql.Cadena(lector, "telefono_cliente"),
                CorreoCliente = LectorSql.Cadena(lector, "correo_cliente")
            };
        }
    }
}
