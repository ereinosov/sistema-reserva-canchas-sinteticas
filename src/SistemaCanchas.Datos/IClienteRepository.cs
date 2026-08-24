using System.Collections.Generic;

using SistemaCanchas.Entidades;

namespace SistemaCanchas.Datos
{
    /// <summary>
    /// Acceso a CLIENTES mediante los procedimientos del artefacto A11 (RF01-RF04).
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>Registra un cliente (sp_RegistrarCliente, RF01).</summary>
        /// <param name="cliente">Datos a persistir.</param>
        /// <returns>Id generado.</returns>
        int Insertar(Cliente cliente);

        /// <summary>Busca por documento y/o nombre (sp_ConsultarClientes, RF02).</summary>
        /// <param name="numeroDocumento">Número exacto o null.</param>
        /// <param name="nombre">Fragmento de nombre o null.</param>
        /// <returns>Clientes encontrados.</returns>
        IList<Cliente> ObtenerTodos(string numeroDocumento, string nombre);

        /// <summary>Actualiza un cliente (sp_ModificarCliente, RF03).</summary>
        /// <param name="cliente">Cliente con id y datos nuevos.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Actualizar(Cliente cliente);

        /// <summary>Elimina un cliente si RN02 lo permite (sp_EliminarCliente, RF04).</summary>
        /// <param name="idCliente">Identificador.</param>
        /// <returns>true si el procedimiento terminó sin error.</returns>
        bool Eliminar(int idCliente);
    }
}
